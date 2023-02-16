using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Label : EUM_Widget
    {
        public override string TypeName => "Label";
        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Label_Info(this);
            return info;
        }

        private string _Content
        {
            get
            {
                if (Info != null)
                {
                    var info = Info as EUM_Label_Info;
                    return string.IsNullOrEmpty(info.Text) ? TypeName : info.Text;
                }
                else
                {
                    return TypeName;
                }
            }
        }

        public override void DrawDraging(Vector2 position)
        {
            GUI.Label(new Rect(position.x + 10, position.y - 10, 100, 40), TypeName);
        }

        protected override void OnDrawLayout()
        {
            GUILib.Label(new GUIContent(_Content));
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Label();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}