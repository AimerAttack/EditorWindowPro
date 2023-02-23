using Amazing.Editor.Library;
using EditorUIMaker.Widgets;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Vertical : EUM_Container
    {
        public override string TypeName => "Vertical";
        protected override EUM_BaseInfo CreateInfo()
        {
            return new EUM_NormalInfo(this);
        }

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
            rect.x += 60;
            GUI.Label(rect,TypeName);
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Vertical();
            Info.CopyTo(widget.Info);
            foreach (var w in Widgets)
            {
                widget.Widgets.Add(w.Clone());
            }
            return widget;
        }
    }
}