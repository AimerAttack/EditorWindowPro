using System;
using System.Collections.Generic;
using Amazing.Editor.Library;
using EditorWindowPro.Utility;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace EditorWindowPro
{
    public class EditorWindowTool : EditorWindow, IEditorWindow
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
        
        private const float s_SplitSize = 2f;

        private List<EW_CheckRect> _CheckRects = new List<EW_CheckRect>();
        private List<EW_BaseWidget> _Widget = new List<EW_BaseWidget>();

        private EW_BaseWidget _SelectedWidget;
        private EW_CheckRect _SelectedCheckRect;

        private float _DeltaX = 0;
        private float _DeltaY = 0;

        private float _SplitRatio = 0.5f;
        private Rect _SplitRect = new Rect(0, 0, 0, 0);
        private bool _MoveSplit = false;


        void Init()
        {
            var testButton2 = new EW_Button(new Rect(200, 0, 100, 100), "Test2");
            _Widget.Add(testButton2);

            var check1 = new EW_CheckRect(new Rect(200, 200, 60, 80));
            _CheckRects.Add(check1);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            
            GUILayout.BeginArea(new Rect(0,0,position.width * _SplitRatio - s_SplitSize,position.height));
            GUILayout.Button("1");
            DrawLeft();
            GUILayout.EndArea();
            
            DrawSplit();
            
            DrawRight();

            Repaint();

            EditorGUILayout.EndHorizontal();
        }

        private Vector2 pp;
        void DrawLeft()
        {
            foreach (var rect in _CheckRects)
            {
                rect.Draw();
            }
        }

        void DrawSplit()
        {
            _SplitRect.Set(_SplitRatio * position.width-s_SplitSize,0,s_SplitSize,position.height);
            GUILib.Rect(_SplitRect, Color.black, 0.4f);

            var dRect2 = GUILib.Padding(_SplitRect, -10f, -2f);
            EditorGUIUtility.AddCursorRect(dRect2, MouseCursor.ResizeHorizontal);
            if (Event.current.type == EventType.MouseDown && dRect2.Contains(Event.current.mousePosition))
            {
                _MoveSplit = true;
                RefreshSplitPosition();
            }

            if (_MoveSplit)
            {
               RefreshSplitPosition();
            }
            
            if (Event.current.type == EventType.MouseUp)
            {
                _MoveSplit = false;
            }
        }

        void RefreshSplitPosition()
        {
            var dd = position.width;
            var m = Event.current.mousePosition.x;
            var pct = Mathf.Min(0.9f, Mathf.Max(0.1f, m / dd));
            _SplitRatio = pct;  
        }

        void DrawRight()
        {
            var rect = new Rect(_SplitRatio * position.width,0, position.width * (1 - _SplitRatio), position.height);
            foreach (var button in _Widget)
            {
                button.Draw(rect);
            }

            ProcessMouseDown();
            ProcessMouseDrag();
            ProcessMouseUp();
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
                        _DeltaX = Event.current.mousePosition.x - widget.Rect.x;
                        _DeltaY = Event.current.mousePosition.y - widget.Rect.y;
                        CreateTmpWidget(widget);
                        break;
                    }
                }
            }
        }

        void CreateTmpWidget(EW_BaseWidget widget)
        {
            var btn = new EW_Button(widget.Rect, "tmp");
            _Widget.Add(btn);
            _SelectedWidget = btn;
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
                        if (rect.Contains(Event.current.mousePosition))
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
                    _Widget.Remove(_SelectedWidget);
                }

                if (_SelectedCheckRect != null)
                {
                    _SelectedCheckRect.Selected = false;
                }

                _SelectedCheckRect = null;
                _SelectedWidget = null;
            }
        }

        public bool NeedRepaint { get; set; }
    }
}