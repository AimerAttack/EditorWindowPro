using Scriban;
using Scriban.Runtime;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_ProgressBar : EUM_Widget
    {
        private EUM_ProgressBar_Info info => Info as EUM_ProgressBar_Info;
        public override string TypeName => "ProgressBar";
        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_ProgressBar_Info(this);
            info.Label = TypeName;
            return info;
        }

        protected override void OnDrawLayout()
        {
            GUILib.ProgressBar(0.5f, info.Label);
        }

        public override string LogicCode()
        {
            var code = @"public float {{name}};";

            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);
            sObj.Add("label", info.Label);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);

            return result; 
        }

        public override string Code()
        {
            var code =
                @"GUILib.ProgressBar(_Logic.{{name}},""{{label}}"");";

            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);
            sObj.Add("label", info.Label);

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
                GUILib.ProgressBar(0.5f,TypeName);
            });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_ProgressBar();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}