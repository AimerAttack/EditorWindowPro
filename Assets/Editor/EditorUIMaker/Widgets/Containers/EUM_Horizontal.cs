using Amazing.Editor.Library;
using EditorUIMaker.Widgets;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Horizontal : EUM_Container
    {
        public override string TypeName => "Horizontal";
        protected override EUM_BaseInfo CreateInfo()
        {
            return new EUM_NormalInfo(this);
        }

        protected override void OnDrawLayout()
        {
            GUILayout.BeginHorizontal();
            DrawItems();
            GUILayout.EndHorizontal();
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
            var widget = new EUM_Horizontal();
            Info.CopyTo(widget.Info);
            
            foreach (var w in Widgets)
            {
                widget.Widgets.Add(w.Clone());
            }
            
            return widget;
        }

        public override EUM_BaseWidget SingleClone()
        {
            var widget = new EUM_Horizontal();
            Info.CopyTo(widget.Info);
            
            return widget; 
        }
    }
}