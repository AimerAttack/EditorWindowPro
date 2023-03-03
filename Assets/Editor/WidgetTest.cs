using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class WidgetTest : OdinEditorWindow
    {
        [MenuItem("Tools/WidgetTest")]
        static void OpenWindow()
        {
            var window = GetWindow<WidgetTest>();
            window.Show();
        }

        public string DisplayTooltipWhenElidedDisplayTooltipWhenElidedDisplayTooltipWhenElidedDisplayTooltipWhenElided;
        public bool EnableRichText;
        public bool DisplayTooltipWhenElidedDisplayTooltipWhenElidedDisplayTooltipWhenElidedDisplayTooltipWhenElided1;
        public string bbbbbbb;

        public int a;
    }
}