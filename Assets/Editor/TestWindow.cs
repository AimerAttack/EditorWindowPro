using System;
using Amazing.Editor.Library;
using EditorUIMaker;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class TestWindow : EditorWindow
    {
        // [MenuItem("Tools/鼠标拖拽测试")]
        static TestWindow OpenWindow()
        {
            var window = GetWindow<TestWindow>();
            window.Focus();
            window.Repaint();
            window.wantsMouseMove = true;
            return window;
        }


        private Rect r = new Rect(0, 0, 100, 100);
        private float deltaX = 0;
        private float deltaY = 0;
        private Vector2 regionPos = Vector2.zero;

        private Rect checkRect = new Rect(300, 300, 100, 100);
        private bool inCheck = false;

        private void OnGUI()
        {
            GUILib.Rect(checkRect, Color.red, 0.5f);

            EditorGUI.BeginDisabledGroup(true);
            GUILayout.BeginArea(r);
            GUILayout.Button("123", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.EndArea();
            EditorGUI.EndDisabledGroup();

            int controlID = GUIUtility.GetControlID(r.GetHashCode(), FocusType.Passive);
            int controlID_check = GUIUtility.GetControlID(checkRect.GetHashCode(), FocusType.Passive);

            EditorGUIUtility.AddCursorRect(r, MouseCursor.MoveArrow);
            if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
            {
                deltaX = Event.current.mousePosition.x - r.x;
                deltaY = Event.current.mousePosition.y - r.y;
                regionPos.x = r.x;
                regionPos.y = r.y;
            }

            if (Event.current.type == EventType.MouseDrag && r.Contains(Event.current.mousePosition))
            {
                r.x = Event.current.mousePosition.x - deltaX;
                r.y = Event.current.mousePosition.y - deltaY;
            }

            if (Event.current.type == EventType.MouseDrag)
            {
                if (!inCheck && checkRect.Contains(Event.current.mousePosition))
                {
                    inCheck = true;
                }
                else if (inCheck && !checkRect.Contains(Event.current.mousePosition))
                {
                    inCheck = false;
                }
            }

            if (Event.current.type == EventType.MouseUp)
            {
                if (inCheck)
                {
                    // Debug.LogError("Do");
                }
                else
                {
                    // Debug.LogWarning("Cancel");
                }

                inCheck = false;
                r.x = regionPos.x;
                r.y = regionPos.y;
            }

            Repaint();
        }
    }
}