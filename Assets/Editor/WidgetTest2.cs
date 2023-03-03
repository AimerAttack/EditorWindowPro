using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class WidgetTest2 : EditorWindow
    {
        [MenuItem("Tools/WidgetTest2")]
        static void OpenWindow()
        {
            var window = GetWindow<WidgetTest2>();
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.TextField("a", "");
            EditorGUILayout.Toggle("EnableRichText", false);
            EditorGUILayout.Toggle("DisplayTooltipWhenElidedDisplayTooltipWhenElidedDisplayTooltipWhenElidedDisplayTooltipWhenElided", false);
            EditorGUILayout.TextField("bbbbbbb", "");

            GUILayout.BeginHorizontal();
            GUILayout.Label("a");
            GUILayout.TextField("");
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("DisplayTooltipWhenElidedDisplayTooltipWhenElidedDisplayTooltipWhenElidedDisplayTooltipWhenElided");
            GUILayout.TextField("");
            GUILayout.EndHorizontal();
        }
    }
}