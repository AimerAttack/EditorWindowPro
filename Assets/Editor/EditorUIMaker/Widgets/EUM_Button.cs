using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Button : EUM_BaseWidget
    {
        public string Name = "Button";

        public override string TypeName => "Button";

        public override void DrawDraging(float x, float y)
        {
            GUI.Button(new Rect(x,y,100,20), Name);
        }

        public override void DrawLayout()
        {
            GUILib.Button(Name);
        }
        
        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Button();
            widget.Name = Name;
            return widget;
        }
    }
}