using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
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

        private bool Expanded = false;
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

            EditorGUI.BeginChangeCheck();
            string currentValueName = "1";
            IEnumerable<object> query;
            if (true)
            {
                // if (GUILayout.Button("button"))
                // {
                    // ShowSelector(GUILayoutUtility.GetLastRect());
                // }
                query = OdinSelector<object>.DrawSelectorDropdown(new GUIContent("code"), currentValueName, new Func<Rect, OdinSelector<object>>(this.ShowSelector), (GUIStyle) null);
            }
          
            
            Repaint();
        }
        
        private OdinSelector<object> ShowSelector(Rect rect)
        {
            GenericSelector<object> selector = this.CreateSelector();
            rect.x = (float) (int) rect.x;
            rect.y = (float) (int) rect.y;
            rect.width = (float) (int) rect.width;
            rect.height = (float) (int) rect.height;
            selector.ShowInPopup(rect, new Vector2(0.0f, 0.0f));
            selector.SelectionConfirmed += delegate(IEnumerable<object> objects) {
                foreach (var o in objects)
                {
                    Debug.Log(o);
                }
            };
            return (OdinSelector<object>) selector;
        }
        
        private GenericSelector<object> CreateSelector()
        {
            IEnumerable<ValueDropdownItem> source1 = Enumerable.Empty<ValueDropdownItem>();
            source1 = new List<ValueDropdownItem>(){new ValueDropdownItem("111",111)};
            bool flag = source1.Take<ValueDropdownItem>(10).Count<ValueDropdownItem>() == 10;
            GenericSelector<object> selector = new GenericSelector<object>("", false, source1.Select<ValueDropdownItem, GenericSelectorItem<object>>((Func<ValueDropdownItem, GenericSelectorItem<object>>) (x => new GenericSelectorItem<object>(x.Text, x.Value))));
            selector.CheckboxToggle = false;
            selector.EnableSingleClickToSelect();
            selector.SelectionTree.Config.DrawSearchToolbar = flag;
            IEnumerable<object> source2 = Enumerable.Empty<object>();
            IEnumerable<object> selection = source2.Select<object, object>((Func<object, object>) (x => x != null ? (object) x.GetType() : (object) null));
            selector.SetSelection(selection);
            selector.SelectionTree.EnumerateTree().AddThumbnailIcons(true);
            return selector;
        }

        void Testt()
        {
        }
    }
}