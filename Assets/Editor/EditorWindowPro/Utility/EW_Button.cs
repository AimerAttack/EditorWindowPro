using UnityEditor;
using UnityEngine;

namespace EditorWindowPro.Utility
{
    public class EW_Button : EW_BaseWidget
    {
        private string _Text;
        
        public EW_Button(Rect rect, string text)
        {
            Rect = rect;
            _Text = text;
        }

        public override void Draw()
        {
            base.Draw();
            
            EditorGUI.BeginDisabledGroup(true);
            GUILayout.BeginArea(Rect);
            GUILayout.Button(_Text, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.EndArea();
            EditorGUI.EndDisabledGroup(); 
        }


        public override bool Contains(Vector2 position)
        {
            return Rect.Contains(position);
        }

        public override int ID => Rect.GetHashCode();
    }
}