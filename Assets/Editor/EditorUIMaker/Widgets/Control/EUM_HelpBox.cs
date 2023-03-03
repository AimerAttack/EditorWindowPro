using Scriban;
using Scriban.Runtime;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_HelpBox : EUM_Widget
    {
        private EUM_HelpBox_Info info => Info as EUM_HelpBox_Info;
        public override string TypeName => "HelpBox";
        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_HelpBox_Info(this);
            info.Label = TypeName;
            return info;
        }

        public override bool CanResize()
        {
            return false;
        }

        protected override void OnDrawLayout()
        {
            GUILib.HelpBox(info.Label, info.MessageType);
        }

        public override string LogicCode()
        {
            return string.Empty;
        }

        public override string Code()
        {
            var code =
                @"GUILib.HelpBox(""{{label}}"",MessageType.{{type}});";

            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);
            sObj.Add("label", info.Label);
            sObj.Add("type",info.MessageType);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);

            return result; 
        }

        public override void DrawDraging(Vector2 position)
        {
            GUILib.Area(new Rect(position.x, position.y, 300, 20), () =>
            {
                GUILib.HelpBox(TypeName, MessageType.Info);
            });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_HelpBox();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}