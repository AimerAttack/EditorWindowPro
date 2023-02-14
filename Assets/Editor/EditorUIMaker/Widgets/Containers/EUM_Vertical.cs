using Amazing.Editor.Library;
using EditorUIMaker.Widgets;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Vertical : EUM_Container
    {
        public override string TypeName => "Vertical";
        
        protected override void OnDrawLayout()
        {
            GUILayout.BeginVertical();
            DrawItems();
            GUILayout.EndVertical();
        }

        public override void DrawDraging(Vector2 position)
        {
            var rect = new Rect(position.x, position.y, 200, 20);
            GUILib.Frame(rect, Color.white, 1);
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Vertical();
            return widget;
        }
    }
}