using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Button : EUM_Widget
    {
        private string _Content
        {
            get
            {
                if (Info != null)
                {
                    var info = Info as EUM_Button_Info;
                    return string.IsNullOrEmpty(info.Text) ? TypeName : info.Text;
                }
                else
                {
                    return TypeName;
                }
            }
        }
        
        public override string TypeName => "Button";
        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Button_Info(this);
            return info;
        }

        public override void DrawDraging(Vector2 position)
        {
            GUI.Button(new Rect(position.x,position.y,100,20), TypeName);
        }

        protected override void OnDrawLayout()
        {
            GUILib.Button(_Content);
        }
        
        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Button();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}