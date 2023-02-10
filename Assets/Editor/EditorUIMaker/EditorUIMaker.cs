using System;
using System.Collections;
using System.Collections.Generic;
using Amazing.Editor.Library;
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
            window.titleContent = new GUIContent("EditorUIMaker");
            window.Show();
            window.Focus();
        }

        private const float s_SplitSize = 2f;
        private const float s_MinInspectorWidth = 300;
        private const float s_MinViewportWidth = 200;
        private const float s_ToolbarWidth = 200;

        private EUM_Toolbox _Toolbox;
        private EUM_Viewport _Viewport;
        private EUM_Inspector _Inspector;

        private float _Ratio = 0.3f;
        private bool _ResizeView = false;

        public EditorUIMaker()
        {
            Init();
        }

        void Init()
        {
            EUM_Helper.Instance = new EUM_Helper();

            _Toolbox = new EUM_Toolbox();
            _Viewport = new EUM_Viewport();
            _Inspector = new EUM_Inspector();
        }

        private void OnLostFocus()
        {
        }

        private void OnFocus()
        {
        }

        private void OnGUI()
        {
            _Toolbox.HandleDrag();

            EUM_Helper.Instance.Fade();

            var inspectorWidth = position.width * _Ratio;
            inspectorWidth = Mathf.Max(s_MinInspectorWidth, inspectorWidth);
            var toolboxWidth = s_ToolbarWidth;
            var viewportWidth = position.width - inspectorWidth - toolboxWidth;
            viewportWidth = Mathf.Max(s_MinViewportWidth, viewportWidth);

            var viewportRect = new Rect(toolboxWidth, 0, viewportWidth - s_SplitSize, position.height);
            _Viewport.Draw(ref viewportRect);
            DrawItems();

            var inspectorRect = new Rect(viewportRect.x + viewportRect.width + s_SplitSize, 0, inspectorWidth,
                position.height);
            _Inspector.Draw(ref inspectorRect);

            var toolboxRect = new Rect(0, 0, toolboxWidth, position.height);
            _Toolbox.Draw(ref toolboxRect);

            var cursorRect = new Rect(viewportRect.x + viewportRect.width, 0, 2, position.height);
            GUILib.Rect(cursorRect, Color.black, 0.4f);
            var dRect2 = GUILib.Padding(cursorRect, -2f, -2f);
            EditorGUIUtility.AddCursorRect(dRect2, MouseCursor.ResizeHorizontal);

            DrawDraging();
            DrawHoverRect();
            DrawSelectRect();

            if (Event.current.type == EventType.MouseDown && dRect2.Contains(Event.current.mousePosition))
            {
                _ResizeView = true;
                RefreshSplitPosition();
            }

            if (_ResizeView)
            {
                RefreshSplitPosition();
            }

            if (Event.current.rawType == EventType.MouseUp)
            {
                if (_ResizeView)
                    _ResizeView = false;
            }

            Repaint();
        }

        void DrawItems()
        {
            foreach (var container in EUM_Helper.Instance.Containers)
            {
                container.DrawItems();
            }
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
                    container.DrawArea();
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

        void DrawSelectRect()
        {
        }

        void DrawHoverRect()
        {
            if (EUM_Helper.Instance.DraggingWidget != null)
            {
                EUM_Helper.Instance.HoverContainer = null;
                return;
            }

            if (!EUM_Helper.Instance.ViewportRect.Contains(Event.current.mousePosition))
            {
                EUM_Helper.Instance.HoverContainer = null;
                return;
            }

            if (!EUM_Helper.Instance.VitualWindowRect.Contains(Event.current.mousePosition))
            {
                EUM_Helper.Instance.HoverContainer = null;
                return;
            }

            EUM_Container container = null;

            foreach (var item in EUM_Helper.Instance.Containers)
            {
                //is root container,dont show
                if(item.Depth == 0)
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
                container.DrawArea();
                if (EUM_Helper.Instance.HoverContainer != container)
                    EUM_Helper.Instance.ResetFade();
                EUM_Helper.Instance.HoverContainer = container;
            }
            else
            {
                EUM_Helper.Instance.HoverContainer = null;
            }
        }

        void RefreshSplitPosition()
        {
            if (position.width <= s_MinInspectorWidth + s_MinViewportWidth + s_ToolbarWidth)
                return;
            if (Event.current.mousePosition.x <= s_ToolbarWidth + s_MinInspectorWidth)
                return;
            var delta = position.width - Event.current.mousePosition.x;
            if (delta < s_MinInspectorWidth)
                return;
            _Ratio = delta / position.width;
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
                if (Event.current.button == 2)
                {
                    if (Event.current.type == EventType.MouseDown)
                    {
                    }
                }
            }
        }

        private void OnDestroy()
        {
            
        }
    }
}