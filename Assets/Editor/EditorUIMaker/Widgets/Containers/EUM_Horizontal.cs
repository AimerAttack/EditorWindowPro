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
            GUILib.HorizontalRect((() =>
            {
                DrawItems();
            }),null,LayoutOptions());
        }

        public override void DrawDraging(Vector2 position)
        {
            GUILib.Area(new Rect(position.x + 20, position.y, 200, 20), () =>
            {
                GUILib.Label(TypeName);
            });
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

        protected override string BeginCode()
        {
            return "GUILayout.BeginHorizontal();";
        }
        
        protected override string EndCode()
        {
            return "GUILayout.EndHorizontal();";
        }
    }
}