using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Button : EUM_BaseWidget
    {
        public string Name = "Button";

        public override string TypeName => "Button";

        public override void DrawWithRect(ref Rect rect)
        {
            GUI.Button(rect, Name);
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