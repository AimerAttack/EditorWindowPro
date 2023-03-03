using Amazing.Editor.Library;
using EditorUIMaker.Widgets;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_ScrollView : EUM_Container
    {
        private Vector2 _ScrollPosition;
        public override string TypeName => "ScrollView";
        protected override EUM_BaseInfo CreateInfo()
        {
            return new EUM_NormalInfo(this);
        }

        protected override void OnDrawLayout()
        {
            GUILib.ScrollView(ref _ScrollPosition, () =>
            {
                DrawItems();
            },LayoutOptions());
        }

        protected override void FixAbsoluteRect()
        {
            AbsoluteRect.xMin -= _ScrollPosition.x;
            AbsoluteRect.yMin -= _ScrollPosition.y;
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
            var widget = new EUM_ScrollView();
            Info.CopyTo(widget.Info);
            
            foreach (var w in Widgets)
            {
                widget.Widgets.Add(w.Clone());
            }
            return widget;
        }

        public override EUM_BaseWidget SingleClone()
        {
            var widget = new EUM_ScrollView();
            Info.CopyTo(widget.Info);
            return widget;
        }
        
        protected override string BeginCode()
        {
            return "GUILayout.BeginScrollView();";
        }
        
        protected override string EndCode()
        {
            return "GUILayout.EndScrollView();";
        }
    }
}