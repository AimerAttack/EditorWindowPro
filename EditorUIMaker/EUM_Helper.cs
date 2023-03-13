using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EditorUIMaker.Utility;
using EditorUIMaker.Widgets;
using Scriban;
using Scriban.Runtime;
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
        public Action<EUM_BaseWidget> OnRemoveItemFromWindow;
        public Action OnItemIndexChange;
        public Action OnSelectWidgetChange;
        public Action<EUM_BaseWidget> OnItemRename;
        public Action OnClearData;
        public Action OnBeforeReloadDomain;
        public Action OnAfterReloadDomain;
        public EUM_Window Window;
        public Rect WindowRect;
        public Rect VitualWindowRect;
        public Rect ViewportRect;
        public List<Rect> MouseRects = new List<Rect>(10);
        public Vector2 MousePosition;
        public HierarchyTreeView TreeView;
        public HashSet<string> AdditionNamespace = new HashSet<string>();
        public List<EUM_BaseWidget> CustomWidget = new List<EUM_BaseWidget>();
        public List<EUM_BaseWidget> CustomContainer = new List<EUM_BaseWidget>();

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
        public Dictionary<Type, int> WidgetCount = new Dictionary<Type, int>();
        public string MenuItemPath;

        #endregion

        public string WindowTitle = "WindowTitle";
        public float Alpha = 0;

        public float _FadeTime = 0.2f;
        public float _StartFadeTime;

        public void LoadData(EUM_Object obj, string filePath)
        {
            if (Modified)
            {
                WarningModified();
            }

            var fileName = Path.GetFileNameWithoutExtension(filePath);
            WindowTitle = fileName;
            ClearData();

            FilePath = filePath;

            if (obj.Stash != null && obj.Stash.Widgets != null)
            {
                for (int i = 0; i < obj.Stash.Widgets.Count; i++)
                {
                    var dataWidget = obj.Stash.Widgets[i];
                    var widget = dataWidget.SingleClone();
                    AddToContainer(widget, Window);
                    AddChildrenToWindow(dataWidget, widget);
                }
            }

            MenuItemPath = obj.MenuItemPath;

            Modified = false;
        }

        void AddChildrenToWindow(EUM_BaseWidget dataWidget, EUM_BaseWidget widget)
        {
            if (dataWidget is EUM_Container dataContainer)
            {
                var container = widget as EUM_Container;
                for (int i = 0; i < dataContainer.Widgets.Count; i++)
                {
                    var dataChild = dataContainer.Widgets[i];
                    var child = dataChild.SingleClone();
                    AddToContainer(child, container);
                    AddChildrenToWindow(dataChild, child);
                }
            }
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
            Widgets.Add(Window.ID, Window);

            MenuItemPath = string.Empty;

            OnClearData?.Invoke();
        }

        public bool NameValid(EUM_BaseWidget widget, string newName)
        {
            if (newName.Length <= 0)
                return false;

            //检查是否数字开头
            if (newName.Length > 0 && char.IsDigit(newName[0]))
            {
                return false;
            }

            //检查是否重名
            foreach (var pair in Widgets)
            {
                var item = pair.Value;
                if (item != widget && item.Info.Name == newName)
                {
                    return false;
                }
            }

            return true;
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
            if (string.IsNullOrEmpty(FilePath))
            {
                //新文件，需要选择保存路径
                var path = EditorUtility.SaveFilePanelInProject("Save File", "NewFile", "asset", "Save File");
                if (!string.IsNullOrEmpty(path))
                {
                    FilePath = path;
                    var fileName = Path.GetFileNameWithoutExtension(path);
                    WindowTitle = fileName;
                    MenuItemPath = string.Format("Tools/{0}", fileName);
                    SaveDataToPath(path);
                }
            }
            else
            {
                //已有文件，直接保存
                if (string.IsNullOrEmpty(MenuItemPath))
                {
                    var fileName = Path.GetFileNameWithoutExtension(FilePath);
                    MenuItemPath = string.Format("Tools/{0}", fileName);
                }

                SaveDataToPath(FilePath);
            }

            Modified = false;
        }

        void SaveDataToPath(string filePath)
        {
            if (MenuItemPath.IndexOf("/") < 1)
            {
                EditorUtility.DisplayDialog("MenuPath Error", "MenuPath must be like 'xxx/xxx'!", "OK");
                return;
            }

            var data = ScriptableObject.CreateInstance<EUM_Object>();


            data.Stash = new EUM_Stash();
            data.MenuItemPath = MenuItemPath;
            var window = Window.Clone() as EUM_Window;

            foreach (var widget in window.Widgets)
            {
                data.Stash.Widgets.Add(widget);
            }

            AssetDatabase.DeleteAsset(filePath);
            AssetDatabase.CreateAsset(data, filePath);
            AssetDatabase.SaveAssets();

            SaveClassFile(filePath);
            SaveLogicFile(filePath);
            AssetDatabase.Refresh();
        }


        void SaveClassFile(string windowPath)
        {
            var page = @"
using System;
using EditorUIMaker;
using UnityEditor;
using UnityEngine;
using Sirenix.Serialization;
{{additionNamespace}}

public class {{className}} : EditorWindow,ISerializationCallbackReceiver
{
    [MenuItem(""{{menuItemPath}}"")]
    public static void ShowWindow()
    {
        var window = GetWindow<{{className}}>();
        window.titleContent = new GUIContent(""{{className}}"");
        window.Init();
        window.Show();
    }

    public {{className}}_Logic _Logic;

    void Init()
    {
        _Logic = new {{className}}_Logic(this);

        {{initCode}}

        _Logic.Init();
    }

    {{defineCode}}

    void OnGUI()
    {
        {{code}}

        Repaint();
    }

    void OnEnable()
    {
        AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
        AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
    }

    void OnDisable()
    {
        AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
        AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
    }
    
    void OnBeforeAssemblyReload()
    {
        _Logic.BeforeReloadDoMain();
    }
    
    void OnAfterAssemblyReload()
    {
        _Logic.AfterReloadDoMain();
    }

    [SerializeField] [HideInInspector] private SerializationData serializationData;

    public void OnBeforeSerialize()
    {
        UnitySerializationUtility.SerializeUnityObject((UnityEngine.Object) this, ref this.serializationData);
    }

    public void OnAfterDeserialize()
    {
        UnitySerializationUtility.DeserializeUnityObject((UnityEngine.Object) this, ref this.serializationData);
    }
}
";
            List<string> contents = new List<string>();
            contents.Clear();
            foreach (var nameSpace in AdditionNamespace)
            {
                if (string.IsNullOrEmpty(nameSpace))
                    continue;
                contents.Add(string.Format("using {0};", nameSpace));
            }

            var additionNamespace = string.Join("\n", contents);

            contents.Clear();
            for (int i = 0; i < Window.Widgets.Count; i++)
            {
                var widget = Window.Widgets[i];
                var widgetCode = widget.Code();
                if (string.IsNullOrEmpty(widgetCode))
                    continue;
                contents.Add(widgetCode);
            }

            var code = string.Join("\n\n", contents);

            contents.Clear();
            for (int i = 0; i < Window.Widgets.Count; i++)
            {
                var widget = Window.Widgets[i];
                var widgetDefineCode = widget.CodeForDefine();
                if (string.IsNullOrEmpty(widgetDefineCode))
                    continue;
                contents.Add(widgetDefineCode);
            }

            var defineCode = string.Join("\n\n", contents);

            contents.Clear();
            for (int i = 0; i < Window.Widgets.Count; i++)
            {
                var widget = Window.Widgets[i];
                var widgetInitCode = widget.CodeForInit();
                if (string.IsNullOrEmpty(widgetInitCode))
                    continue;
                contents.Add(widgetInitCode);
            }

            var initCode = string.Join("\n\n", contents);

            var sObj = new ScriptObject();
            sObj.Add("code", code);
            sObj.Add("className", WindowTitle);
            sObj.Add("menuItemPath", MenuItemPath);
            sObj.Add("defineCode", defineCode);
            sObj.Add("initCode", initCode);
            sObj.Add("additionNamespace", additionNamespace);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(page);
            var result = template.Render(context);


            var fileName = WindowTitle;
            var path = Path.GetDirectoryName(windowPath);
            var filePath = Path.Combine(Application.dataPath.Replace("Assets", string.Empty),
                path, fileName + ".cs");

            File.WriteAllText(filePath, result, new UTF8Encoding(false));
        }

        void SaveLogicFile(string windowPath)
        {
            var page = @"
using System;
using EditorUIMaker;
using UnityEngine;
{{additionNamespace}}

public partial class {{className}}_Logic : EUM_BaseWindowLogic
{
    public {{className}} Window;

    public {{className}}_Logic({{className}} window)
    {
        Window = window;
    }

    public void Init()
    {
        CallMethod(""DoInit"");
    }

    public void BeforeReloadDoMain()
    {
        CallMethod(""OnBeforeReloadDoMain"");
    }

    public void AfterReloadDoMain()
    {
        CallMethod(""OnAfterReloadDoMain"");
    }

    {{code}}
}
";
            var contents = new List<string>();
            contents.Clear();
            foreach (var nameSpace in AdditionNamespace)
            {
                if (string.IsNullOrEmpty(nameSpace))
                    continue;
                contents.Add(string.Format("using {0};", nameSpace));
            }

            var additionNamespace = string.Join("\n", contents);


            contents.Clear();
            for (int i = 0; i < Window.Widgets.Count; i++)
            {
                var widget = Window.Widgets[i];
                var widgetCode = widget.LogicCode();
                if (string.IsNullOrEmpty(widgetCode))
                    continue;
                contents.Add(widgetCode);
            }

            var code = string.Join("\n", contents);


            var sObj = new ScriptObject();
            sObj.Add("code", code);
            sObj.Add("className", WindowTitle);
            sObj.Add("additionNamespace", additionNamespace);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(page);
            var result = template.Render(context);

            var fileName = WindowTitle;
            var path = Path.GetDirectoryName(windowPath);
            var filePath = Path.Combine(Application.dataPath.Replace("Assets", string.Empty),
                path, fileName + "_Logic.cs");

            File.WriteAllText(filePath, result, new UTF8Encoding(false));
        }

        public void OpenFile()
        {
            var path = EditorUtility.OpenFilePanelWithFilters("Open File", "", new[] {"EUM_Object", "asset"});
            if (string.IsNullOrEmpty(path))
                return;
            var filePath = Utility.Utility.GetRelativePathInProject(path);
            var data = AssetDatabase.LoadAssetAtPath<EUM_Object>(filePath);
            LoadData(data, filePath);
        }

        public void NewFile()
        {
            var path = EditorUtility.SaveFilePanelInProject("Save File", "NewFile", "asset", "Save File");
            var data = ScriptableObject.CreateInstance<EUM_Object>();
            AssetDatabase.CreateAsset(data, path);
            AssetDatabase.SaveAssets();
            LoadData(data, path);
        }

        public void AddToContainer(EUM_BaseWidget widget, EUM_Container container, int index = -1)
        {
            if (container == null)
                return;
            if (container.Widgets.Contains(widget))
                return;
            if (index == -1)
                container.Widgets.Add(widget);
            else
            {
                container.Widgets.Insert(index, widget);
            }

            widget.OnAddToContainer(container);

            var type = widget.GetType();
            if (!WidgetCount.ContainsKey(type))
            {
                WidgetCount.Add(type, 0);
            }

            WidgetCount[type]++;
            if (string.IsNullOrEmpty(widget.Info.Name) || widget.Info.Name == widget.TypeName)
                widget.Info.Name = widget.TypeName + WidgetCount[type];
            else
            {
                foreach (var pair in Widgets)
                {
                    if (pair.Value.Info.Name == widget.Info.Name)
                    {
                        widget.Info.Name = widget.TypeName + WidgetCount[type];
                    }
                }
            }

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
            var percent = deltaTime / _FadeTime;
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