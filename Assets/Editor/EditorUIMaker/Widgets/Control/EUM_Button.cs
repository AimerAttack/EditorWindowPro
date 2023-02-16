using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Button : EUM_Widget
    {
        public override string TypeName => "Button";
        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Button_Info();
            return info;
        }

        public override void DrawDraging(Vector2 position)
        {
            GUI.Button(new Rect(position.x,position.y,100,20), Name);
        }

        protected override void OnDrawLayout()
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