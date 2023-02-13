using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Label : EUM_Widget
    {
        public override string TypeName => "Label";
        public GUIContent Content = new GUIContent("Label");

        public override void DrawDraging(Vector2 position)
        {
            GUI.Label(new Rect(position.x + 10, position.y - 10, 100, 40), Content);
        }

        public override void DrawLayout()
        {
            GUILib.Label(Content);
        }

        public override EUM_Widget Clone()
        {
            var widget = new EUM_Label();
            widget.Content = new GUIContent(Content);
            return widget;
        }
    }
}