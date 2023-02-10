using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Space : EUM_BaseWidget
    {
        private float _Pixels = 5;
        public override string TypeName=> "Space";
        public override void DrawDraging(float x, float y)
        {
            var rect = new Rect(x, y, 200, 20);
            GUILib.Frame(rect,Color.white,1);
        }

        public override void DrawLayout()
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