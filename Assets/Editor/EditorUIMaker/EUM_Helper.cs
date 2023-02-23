using System;
using System.Collections.Generic;
using System.IO;
using EditorUIMaker.Widgets;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Helper
    {
        public static EUM_Helper Instance;

        public static float MinWindowHeight = 200;
        public static float MinWindowWidth = 100;
        public static float InitWindowWidth = 400;
        public static float InitWindowHeight = 300;
        public static float MinimumDragToSnapToMoveRotateScaleResize = 2;

        public string FilePath;
        public string WindowName = "EditorUIMaker";
        public Action<EUM_BaseWidget> OnAddItemToWindow;
        public Action OnRemoveItemFromWindow;
        public Action OnItemIndexChange;
        public Action OnSelectWidgetChange;
        public Action<EUM_BaseWidget> OnItemRename;
        public Action OnClearData;
        public Action OnAfterReloadDomain;
        public EUM_Window Window;
        public Rect WindowRect;
        public Rect VitualWindowRect;
        public Rect ViewportRect;
        public List<Rect> MouseRects = new List<Rect>(10);
 
        #region need clear when data change
        public int WidgetID = 1;
        public bool Modified = false;
        public bool CheckCanvasDrag = false;
        public Vector2 StartDragCanvasPosition;
        public bool Preview;
        public int ZoomIndex;
        public EUM_BaseWidget ClipboardWidget;
        public EUM_BaseWidget DraggingWidget;
        public EUM_BaseWidget SelectWidget;
        public EUM_Container DraggingOverContainer;
        public EUM_BaseWidget HoverWidget;
        public List<EUM_Container> Containers = new List<EUM_Container>();
        public Dictionary<int, EUM_BaseWidget> Widgets = new Dictionary<int, EUM_BaseWidget>();
        #endregion
        
        public string WindowTitle = "WindowTitle";
        public float Alpha = 0;

        public float _FadeTime = 0.2f;
        public float _StartFadeTime;
        
        public void LoadData(EUM_Object obj,string filePath)
        {
            if (Modified)
            {
                WarningModified();    
            }

            var fileName = Path.GetFileNameWithoutExtension(filePath);
            WindowTitle = fileName;
            ClearData();
            //wtodo
            FilePath = filePath;

            if (obj.Stash != null && obj.Stash.Widgets != null)
            {
                for (int i = 0; i < obj.Stash.Widgets.Count; i++)
                {
                    var widget = obj.Stash.Widgets[i];
                    AddToContainer(widget.Clone(), Window);
                }
            }

            Modified = false;
        }

        public void ClearData()
        {
            WidgetID = 100;
            Modified = false;
            CheckCanvasDrag = false;
            StartDragCanvasPosition = Vector2.zero;
            Preview = false;
            ZoomIndex = EUM_Helper.DefaultZoomIndex();
            ClipboardWidget = null;
            DraggingWidget = null;
            SelectWidget = null;
            DraggingOverContainer = null;
            HoverWidget = null;
            
            Containers.Clear();
            Containers.Add(Window);
            
            Widgets.Clear();
            Widgets.Add(Window.ID,Window);
            
            OnClearData?.Invoke();
        }

        public void WarningModified()
        {
            if (EditorUtility.DisplayDialog("Warning", "File has been modified!", "Save", "Don't Save"))
            {
                SaveFile();
            }
        }

        public void SaveFile()
        {
            if(!Modified)
                return;
            if (string.IsNullOrEmpty(FilePath))
            {
                //新文件，需要选择保存路径
                var path = EditorUtility.SaveFilePanelInProject("Save File", "NewFile", "asset", "Save File");
                if(!string.IsNullOrEmpty(path))
                    SaveDataToPath(path);
            }
            else
            {
                //已有文件，直接保存
                SaveDataToPath(FilePath);
            }

            Modified = false;
        }
        
        void SaveDataToPath(string filePath)
        {
            var data = ScriptableObject.CreateInstance<EUM_Object>();
            
            data.Stash = new EUM_Stash();
            var window = Window.Clone() as EUM_Window;

            foreach (var widget in window.Widgets)
            {
                data.Stash.Widgets.Add(widget);
            }
            
            AssetDatabase.DeleteAsset(filePath);
            AssetDatabase.CreateAsset(data,filePath);
            AssetDatabase.SaveAssets();
        }
        
        public void OpenFile()
        {
            var path = EditorUtility.OpenFilePanelWithFilters("Open File", "", new[] {"EUM_Object", "asset"});
            if(string.IsNullOrEmpty(path))
                return;
            var filePath = Utility.Utility.GetRelativePathInProject(path);
            var data = AssetDatabase.LoadAssetAtPath<EUM_Object>(filePath);
            LoadData(data,filePath);
        }
        
        public void NewFile()
        {
            var path = EditorUtility.SaveFilePanelInProject("Save File", "NewFile", "asset", "Save File");
            var data = ScriptableObject.CreateInstance<EUM_Object>();
            AssetDatabase.CreateAsset(data,path);
            AssetDatabase.SaveAssets();
            LoadData(data,path);
        }
        
        public void AddToContainer(EUM_BaseWidget widget, EUM_Container container)
        {
            if (container == null)
                return;
            if (container.Widgets.Contains(widget))
                return;
            container.Widgets.Add(widget);
            widget.OnAddToContainer(container);
            OnAddItemToWindow?.Invoke(widget);
        }

        public void ClearFocus()
        {
            GUIUtility.keyboardControl = 0;
        }
        
        public void ResetFade()
        {
            Alpha = 0;
            _StartFadeTime = Time.realtimeSinceStartup;
        }
        
        public void Fade()
        {
            var deltaTime = Time.realtimeSinceStartup - _StartFadeTime;
            var percent = deltaTime/ _FadeTime;
            percent = Mathf.Clamp01(percent);
            Alpha = Mathf.Lerp(0, 1, percent);
        }
        

        public class ZoomScaleData
        {
            public string Display;
            public float ScaleAmount;

            public ZoomScaleData(string display, float scale)
            {
                Display = display;
                ScaleAmount = scale;
            }
        }

        public static ZoomScaleData[] ZoomScales = new ZoomScaleData[]
        {
            new ZoomScaleData("40%", 0.4f),
            new ZoomScaleData("50%", 0.5f),
            new ZoomScaleData("60%", 0.6f),
            new ZoomScaleData("70%", 0.7f),
            new ZoomScaleData("80%", 0.8f),
            new ZoomScaleData("90%", 0.9f),
            new ZoomScaleData("100%", 1.0f),
            new ZoomScaleData("110%", 1.1f),
            new ZoomScaleData("120%", 1.2f),
            new ZoomScaleData("130%", 1.3f),
            new ZoomScaleData("140%", 1.4f),
            new ZoomScaleData("150%", 1.5f),
            new ZoomScaleData("160%", 1.6f),
            new ZoomScaleData("180%", 1.8f),
            new ZoomScaleData("200%", 2.0f),
            new ZoomScaleData("300%", 3.0f),
            new ZoomScaleData("400%", 4.0f),
        };

        public static int DefaultZoomIndex()
        {
            return 6;
        }

        public static float GetZoomScaleFactor()
        {
            return ZoomScales[Instance.ZoomIndex].ScaleAmount;
        }

        public static float GetZoomScaleFactorFromIndex(int index)
        {
            return ZoomScales[index].ScaleAmount;
        }

        public static string[] GetZoomScalesText()
        {
            List<string> zoomText = new List<string>();
            for (int i = 0; i < ZoomScales.Length; ++i)
            {
                zoomText.Add(ZoomScales[i].Display);
            }

            return zoomText.ToArray();
        }
    }
}