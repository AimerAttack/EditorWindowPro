using Amazing.Editor.Library;
using EditorUIMaker.Widgets;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Horizontal : EUM_Container
    {
        public override string TypeName => "Horizontal";
        
        protected override void OnDrawLayout()
        {
            GUILayout.BeginHorizontal(GUILayout.MinHeight(100));
            DrawItems();
            GUILayout.EndHorizontal();
        }

        public override void DrawDraging(Vector2 position)
        {
            var rect = new Rect(position.x, position.y, 200, 20);
            GUILib.Frame(rect, Color.white, 1);
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Horizontal();
            return widget;
        }
    }
}