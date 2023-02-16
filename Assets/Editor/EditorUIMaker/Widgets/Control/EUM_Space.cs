using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Space : EUM_Widget
    {
        private float _Pixels = 5;
        public override string TypeName => "Space";
        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Space_Info();
            return info;
        }

        public override void DrawDraging(Vector2 position)
        {
            var rect = new Rect(position.x, position.y, 200, 20);
            GUILib.Frame(rect, Color.white, 1);
        }

        protected override void OnDrawLayout()
        {
            GUILayout.Space(_Pixels);
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Space();
            widget._Pixels = _Pixels;
            return widget;
        }
    }
}