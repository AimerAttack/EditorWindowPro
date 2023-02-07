using UnityEditor;
using UnityEngine;

namespace EditorWindowPro.Utility
{
    public class EW_Button : EW_BaseWidget
    {
        private string _Text;
        
        public EW_Button(Rect rect, string text) : base(rect)
        {
            Rect = rect;
            _Text = text;
        }

        public override void Draw(Rect rect)
        {
            base.Draw(rect);
            
            EditorGUI.BeginDisabledGroup(true);
            GUILayout.BeginArea(Rect);
            GUILayout.Button(_Text, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.EndArea();
            EditorGUI.EndDisabledGroup(); 
        }

        public override int ID => Rect.GetHashCode();
    }
}