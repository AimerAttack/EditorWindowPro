using System;
using System.Collections;
using System.Collections.Generic;
using Amazing.Editor.Library;
using EditorUIMaker.Widgets;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EditorUIMaker : EditorWindow
    {
        [MenuItem("Tools/EditorUIMaker")]
        static void OpenWindow()
        {
            var window = GetWindow<EditorUIMaker>();
            window.titleContent = new GUIContent(EUM_Helper.Instance.WindowName);
            window.Show();
            window.Focus();
        }

        private const float s_SplitSize = 2f;
        private const float s_MinInspectorWidth = 300;
        private const float s_MinViewportWidth = 200;
        private const float s_MinOperationWidth = 200;

        private EUM_OperationArea _OperationArea;
        private EUM_Viewport _Viewport;
        private EUM_Inspector _Inspector;
        private EUM_Inputer _Input;

        private float _RatioInspector = 0.2f;
        private float _RatioOperationArea = 0.1f;
        private bool _ResizeInspector = false;
        private bool _ResizeOperationArea = false;
        private bool _CheckCanvasDrag = false;

        public EditorUIMaker()
        {
            Init();
        }

        void Init()
        {
            EUM_Helper.Instance = new EUM_Helper();

            _OperationArea = new EUM_OperationArea();
            _Viewport = new EUM_Viewport();
            _Inspector = new EUM_Inspector();
            _Input = new EUM_Inputer();

            EUM_Helper.Instance.OnRemoveItemFromWindow += OnRemoveItemFromWindow;
            EUM_Helper.Instance.OnAddItemToWindow += OnAddItemToWindow;
            EUM_Helper.Instance.OnItemIndexChange += OnItemIndexChange;
        }

        void OnItemIndexChange()
        {
            OnModified();
        }

        void OnAddItemToWindow(EUM_BaseWidget widget)
        {
            OnModified();
        }

        void OnRemoveItemFromWindow()
        {
            OnModified();
        }

        void OnModified()
        {
            EUM_Helper.Instance.Modified = true;
            titleContent = new GUIContent(EUM_Helper.Instance.WindowName + " *");
        }
        

        private void OnGUI()
        {
            ProcessMouseMove();
            
            _Input.CheckInput();
            _OperationArea.Library.HandleDrag();

            EUM_Helper.Instance.Fade();

            var inspectorWidth = position.width * _RatioInspector;
            inspectorWidth = Mathf.Max(s_MinInspectorWidth, inspectorWidth);
            var operationWidth = position.width * Mathf.Min(_RatioOperationArea,0.5f);
            operationWidth = Mathf.Max(s_MinOperationWidth, operationWidth);
            var viewportWidth = position.width - inspectorWidth - operationWidth;
            viewportWidth = Mathf.Max(s_MinViewportWidth, viewportWidth);

            var viewportRect = new Rect(operationWidth, 0, viewportWidth - s_SplitSize, position.height);
            _Viewport.Draw(ref viewportRect);

            var inspectorRect = new Rect(viewportRect.x + viewportRect.width + s_SplitSize, 0, inspectorWidth,
                position.height);
            _Inspector.Draw(ref inspectorRect);

            var operationRect = new Rect(0, 0, operationWidth, position.height);
            _OperationArea.Draw(ref operationRect);

            var operationSplitRect = new Rect(operationRect.x + operationRect.width, 0, 2, position.height);
            GUILib.Rect(operationSplitRect, Color.black, 0.4f);
            var operationSplitCursorRect = GUILib.Padding(operationSplitRect, -2f, -2f);
            EditorGUIUtility.AddCursorRect(operationSplitCursorRect, MouseCursor.ResizeHorizontal);
            
            var inspectorSplitRect = new Rect(viewportRect.x + viewportRect.width, 0, 2, position.height);
            GUILib.Rect(inspectorSplitRect, Color.black, 0.4f);
            var inspectorSplitCursorRect = GUILib.Padding(inspectorSplitRect, -2f, -2f);
            EditorGUIUtility.AddCursorRect(inspectorSplitCursorRect, MouseCursor.ResizeHorizontal);

            if (!EUM_Helper.Instance.Preview)
            {
                DrawDraging();
                DrawHoverRect();
                CheckSelectRect();
                DrawSelectRect();
            }
            else
            {
                DrawPreviewFrame();
            }

            if (Event.current.type == EventType.MouseDown && inspectorSplitCursorRect.Contains(Event.current.mousePosition))
            {
                _ResizeInspector = true;
                RefreshInspectorSplitPosition();
            }
            if (_ResizeInspector)
            {
                RefreshInspectorSplitPosition();
            }

            if(Event.current.type == EventType.MouseDown && operationSplitCursorRect.Contains(Event.current.mousePosition))
            {
                _ResizeOperationArea = true;
                RefreshOperationSplitPosition();
            }
            if (_ResizeOperationArea)
            {
                RefreshOperationSplitPosition();
            }

            if (Event.current.rawType == EventType.MouseUp)
            {
                if (_ResizeInspector)
                    _ResizeInspector = false;
                if (_ResizeOperationArea)
                    _ResizeOperationArea = false;
            }

            Repaint();
        }

        void DrawPreviewFrame()
        {
            GUILib.Frame(EUM_Helper.Instance.ViewportRect,new Color(255f/255,139f/255,40f/255),2);
        }

        void DrawDraging()
        {
            if (EUM_Helper.Instance.DraggingWidget != null)
            {
                EUM_Helper.Instance.DraggingWidget.DrawDraging(Event.current.mousePosition);

                if (!EUM_Helper.Instance.ViewportRect.Contains(Event.current.mousePosition))
                {
                    EUM_Helper.Instance.DraggingOverContainer = null;
                    return;
                }

                if (!EUM_Helper.Instance.VitualWindowRect.Contains(Event.current.mousePosition))
                {
                    EUM_Helper.Instance.DraggingOverContainer = null;
                    return;
                }

                EUM_Container container = null;

                foreach (var item in EUM_Helper.Instance.Containers)
                {
                    if (!item.InViewport)
                        continue;
                    if (!item.Contains(Event.current.mousePosition))
                        continue;
                    if (container == null)
                    {
                        container = item;
                    }
                    else
                    {
                        if (item.Depth > container.Depth)
                            container = item;
                    }
                }

                if (container != null)
                {
                    DrawDraggingOverRect(container);
                    if (EUM_Helper.Instance.DraggingOverContainer != container)
                        EUM_Helper.Instance.ResetFade();
                    EUM_Helper.Instance.DraggingOverContainer = container;
                }
                else
                {
                    EUM_Helper.Instance.DraggingOverContainer = null;
                }
            }
            else
            {
                EUM_Helper.Instance.DraggingOverContainer = null;
            }
        }

        void DrawDraggingOverRect(EUM_Container container)
        {
            GUILib.Frame(container.Rect, Color.blue,EUM_Helper.Instance.ViewportRect);
        }

        void CheckSelectRect()
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                var oldSelect = EUM_Helper.Instance.SelectWidget;
                if (EUM_Helper.Instance.HoverWidget == null)
                {
                    EUM_Helper.Instance.SelectWidget = null;   
                }
                else
                {
                    var widget = EUM_Helper.Instance.HoverWidget;
                    if (widget.Contains(Event.current.mousePosition))
                    {
                        EUM_Helper.Instance.SelectWidget = widget;
                    }
                }
                if(EUM_Helper.Instance.SelectWidget != oldSelect)
                    EUM_Helper.Instance.OnSelectWidgetChange?.Invoke();
            }
        }

        void DrawSelectRect()
        {
            if (EUM_Helper.Instance.SelectWidget == null)
                return;
            var widget = EUM_Helper.Instance.SelectWidget;
            GUILib.Frame(widget.Rect, Color.green,EUM_Helper.Instance.ViewportRect);
        }

        void DrawHoverRect()
        {
            if (EUM_Helper.Instance.DraggingWidget != null)
            {
                EUM_Helper.Instance.HoverWidget = null;
                return;
            }

            EUM_BaseWidget widget = null;

            Queue<EUM_BaseWidget> checkList = new Queue<EUM_BaseWidget>();

            foreach (var item in EUM_Helper.Instance.Containers)
            {
                if (!item.InViewport)
                    continue;
                if (!item.Contains(Event.current.mousePosition))
                    continue;
                //在区域内，进而判断是否在子控件内
                checkList.Enqueue(item);
                while (checkList.Count > 0)
                {
                    var checkItem = checkList.Dequeue();
                    //if depth == 0 , is root window,dont check hover
                    if (checkItem.Depth != 0 && checkItem.Contains(Event.current.mousePosition))
                    {
                        if (widget == null)
                        {
                            widget = checkItem;
                        }
                        else
                        {
                            if (checkItem.Depth > widget.Depth)
                                widget = checkItem;
                        }
                    }

                    if (checkItem is EUM_Container)
                    {
                        var container = checkItem as EUM_Container;
                        foreach (var child in container.Widgets)
                        {
                            checkList.Enqueue(child);
                        }
                    }
                }
            }

            if (widget == null)
            {
               
            }

            if (widget != null)
            {
                GUILib.Frame(widget.Rect, Color.blue, EUM_Helper.Instance.ViewportRect,1.5f);
                if (EUM_Helper.Instance.HoverWidget != widget)
                    EUM_Helper.Instance.ResetFade();
                EUM_Helper.Instance.HoverWidget = widget;
            }
            else
            {
                var treeHoverItem = _OperationArea.Hierarchy.TreeView.HoverItem;
                if (treeHoverItem != null && EUM_Helper.Instance.Widgets.ContainsKey(treeHoverItem.id))
                {
                    widget = EUM_Helper.Instance.Widgets[treeHoverItem.id];
                    GUILib.Frame(widget.Rect, Color.blue, EUM_Helper.Instance.ViewportRect,1.5f);
                }

                EUM_Helper.Instance.HoverWidget = null;
            }
        }
        
        void RefreshOperationSplitPosition()
        {
            if (position.width <= s_MinInspectorWidth + s_MinViewportWidth + s_MinOperationWidth)
                return;
            var delta = Event.current.mousePosition.x;
            if (delta < s_MinOperationWidth)
                return;
            _RatioOperationArea = delta / position.width;
        }

        void RefreshInspectorSplitPosition()
        {
            if (position.width <= s_MinInspectorWidth + s_MinViewportWidth + s_MinOperationWidth)
                return;
            var delta = position.width - Event.current.mousePosition.x;
            if (delta < s_MinInspectorWidth)
                return;
            _RatioInspector = delta / position.width;
        }

        void ProcessScrollWheel()
        {
            if (Event.current.type == EventType.ScrollWheel)
            {
                var zoomDelta = UIEditorVariables.ZoomIndex;
                if (Event.current.delta.y < 0)
                    zoomDelta++;
                else
                {
                    zoomDelta--;
                }

                if (zoomDelta < 0)
                    zoomDelta = 0;
                if (zoomDelta >= UIEditorHelpers.ZoomScales.Length)
                    zoomDelta = UIEditorHelpers.ZoomScales.Length - 1;

                UIEditorVariables.ZoomIndex = zoomDelta;
            }
        }

        void ProcessMouseMove()
        {
            if (Event.current.isMouse)
            {
                if (_CheckCanvasDrag)
                {
                    if (Event.current.rawType == EventType.MouseUp)
                    {
                        _CheckCanvasDrag = false;
                        Event.current.Use();
                    }
                    else
                    {
                        var distance = Vector2.Distance(Event.current.mousePosition,
                            EUM_Helper.Instance.StartDragCanvasPosition);
                        if (distance > EUM_Helper.MinimumDragToSnapToMoveRotateScaleResize)
                        {
                            var deltaX = Event.current.mousePosition.x - EUM_Helper.Instance.StartDragCanvasPosition.x;
                            var deltaY = Event.current.mousePosition.y - EUM_Helper.Instance.StartDragCanvasPosition.y;
                            EUM_Helper.Instance.StartDragCanvasPosition.x = Event.current.mousePosition.x;
                            EUM_Helper.Instance.StartDragCanvasPosition.y = Event.current.mousePosition.y;
                            EUM_Helper.Instance.WindowRect.x += deltaX;
                            EUM_Helper.Instance.WindowRect.y += deltaY;
                        }
                    }
                }
                else
                {
                    if (Event.current.button == 1)
                    {
                        if (Event.current.type == EventType.MouseDown)
                        {
                            EUM_Helper.Instance.StartDragCanvasPosition.x = Event.current.mousePosition.x;
                            EUM_Helper.Instance.StartDragCanvasPosition.y = Event.current.mousePosition.y;
                            _CheckCanvasDrag = true;
                            Event.current.Use();
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (!EUM_Helper.Instance.Modified)
                return;
            if (EditorUtility.DisplayDialog("提示", "当前UI编辑器有未保存的修改，是否保存？", "保存", "不保存"))
            {
                //wtodo
            }
        }
    }
}