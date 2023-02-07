using System;
using System.Collections.Generic;
using EditorWindowPro.Utility;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace EditorWindowPro
{
    public class EditorWindowTool : EditorWindow
    {
        [MenuItem("Tools/EditorWindowTool")]
        static EditorWindowTool OpenWindow()
        {
            var window = GetWindow<EditorWindowTool>();
            window.Focus();
            window.Repaint();
            window.titleContent = new GUIContent("EditorWindowTool");
            window.wantsMouseMove = true;
            return window;
        }

        public EditorWindowTool()
        {
            Init();
        }
      
        private List<EW_CheckRect> _CheckRects = new List<EW_CheckRect>();
        private List<EW_BaseWidget> _Widget = new List<EW_BaseWidget>();
        
        private EW_BaseWidget _SelectedWidget;
        private EW_CheckRect _SelectedCheckRect;
        
        private float _DeltaX = 0;
        private float _DeltaY = 0;
        private Vector2 _RegionPosition = Vector2.zero;

        void Init()
        {
            var testButton2 = new EW_Button(new Rect(200, 0, 100, 100), "Test2");
            
            _Widget.Add(testButton2);

            var check1 = new EW_CheckRect(new Rect(200, 200, 60, 80));
            _CheckRects.Add(check1);
        }
        
        private void OnGUI()
        {
            foreach (var rect in _CheckRects)
            {
                rect.Draw();
            }

            foreach (var button in _Widget)
            {
                button.Draw();
            }
            
            ProcessMouseDown();
            ProcessMouseDrag();
            ProcessMouseUp();
            
            Repaint();
        }

        void ProcessMouseDown()
        {
            if (Event.current.type == EventType.MouseDown)
            {
                // 遍历所有的组件，判断是否有组件被点击
                foreach (var widget in _Widget)
                {
                    if (widget.Contains(Event.current.mousePosition))
                    {
                        _SelectedWidget = widget;
                        _DeltaX = Event.current.mousePosition.x - widget.Rect.x;
                        _DeltaY = Event.current.mousePosition.y - widget.Rect.y;
                        _RegionPosition.x = widget.Rect.x;
                        _RegionPosition.y = widget.Rect.y;
                        break;
                    }
                }
            }
        }

        void ProcessMouseDrag()
        {
            if (Event.current.type == EventType.MouseDrag)
            {
                if (_SelectedCheckRect != null)
                    _SelectedCheckRect.Selected = false;
                _SelectedCheckRect = null;
                
                if (_SelectedWidget != null)
                {
                    foreach (var rect in _CheckRects)
                    {
                        if(rect.Contains(Event.current.mousePosition))
                        {
                            _SelectedCheckRect = rect;
                            _SelectedCheckRect.Selected = true;
                            break;
                        }
                    }
                    
                    _SelectedWidget.SetPosition(Event.current.mousePosition.x - _DeltaX,
                         Event.current.mousePosition.y - _DeltaY);
                }
            }
        }

        void ProcessMouseUp()
        {
            if (Event.current.type == EventType.MouseUp)
            {
                if (_SelectedWidget != null)
                {
                    _SelectedWidget.SetPosition(_RegionPosition);
                }

                if (_SelectedCheckRect != null)
                {
                    _SelectedCheckRect.Selected = false;
                }

                _SelectedCheckRect = null;
                _SelectedWidget = null;
            }
        }
    }
}