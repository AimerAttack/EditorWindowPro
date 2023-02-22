using System;
using System.Collections.Generic;
using EditorUIMaker.Widgets;
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
        public EUM_BaseWidget ClipboardWidget;
        public string WindowName = "EditorUIMaker";
        public bool Modified = false;
        public int WidgetID = 1;
        public Action<EUM_BaseWidget> OnAddItemToWindow;
        public Action OnRemoveItemFromWindow;
        public Action OnItemIndexChange;
        public Action OnSelectWidgetChange;
        public Action<EUM_BaseWidget> OnItemRename;
        public Action OnAfterReloadDomain;
        public List<Rect> MouseRects = new List<Rect>(10);
        public EUM_Window Window;
        public int ZoomIndex;
        public Rect WindowRect;
        public Rect VitualWindowRect;
        public Rect ViewportRect;
        public EUM_BaseWidget DraggingWidget;
        public EUM_BaseWidget SelectWidget;
        public EUM_Container DraggingOverContainer;
        public EUM_BaseWidget HoverWidget;
        public List<EUM_Container> Containers = new List<EUM_Container>();
        public Dictionary<int, EUM_BaseWidget> Widgets = new Dictionary<int, EUM_BaseWidget>();
        public Vector2 StartDragCanvasPosition;
        public bool Preview;
        public string WindowTitle = "WindowTitle";
        public float Alpha = 0;

        public float _FadeTime = 0.2f;
        public float _StartFadeTime;

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