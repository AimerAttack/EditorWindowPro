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
            _ScrollPosition = GUILayout.BeginScrollView(_ScrollPosition);
            DrawItems();
            GUILayout.EndScrollView();
        }

        protected override void FixAbsoluteRect()
        {
            AbsoluteRect.xMin -= _ScrollPosition.x;
            AbsoluteRect.yMin -= _ScrollPosition.y;
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
            var widget = new EUM_ScrollView();
            Info.CopyTo(widget.Info);
            
            foreach (var w in Widgets)
            {
                widget.Widgets.Add(w.Clone());
            }
            return widget;
        }
    }
}