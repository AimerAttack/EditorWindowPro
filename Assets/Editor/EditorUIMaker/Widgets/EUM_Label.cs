using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Label : EUM_BaseWidget
    {
        public override string TypeName => "Label";
        public GUIContent Content = new GUIContent("Label");
        
        public override void DrawDraging(float x, float y)
        {
            GUI.Label(new Rect(x + 10,y-10,100,40),Content);
        }

        public override void DrawLayout()
        {
            GUILib.Label(Content);
        }
        
        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Label();
            widget.Content = new GUIContent(Content);
            return widget;
        }
    }
}