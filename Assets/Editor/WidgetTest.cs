using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

//LayoutGroup只适用内部元素使用统一Style的情况，所以目前只放在Libaray里用
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
        
        [ResponsiveButtonGroup()]
        public void a()
        {
            
        }
    }
}