using Scriban;
using Scriban.Runtime;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Float : EUM_Widget
    {
        private EUM_Float_Info info => Info as EUM_Float_Info;
        public override string TypeName => "Float";

        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Float_Info(this);
            info.Label = TypeName;
            return info;
        }

        protected override void OnDrawLayout()
        {
            GUILib.FloatField(ref info.Value, info.Label);
        }

        public override string LogicCode()
        {
            var code = @"public float {{name}};

public void {{name}}ValueChange()
{
    CallMethod(""On{{name}}ValueChange"");
}
";
            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);

            return result;
        }

        public override string Code()
        {
            var code = @"
if(GUILib.FloatField(ref _Logic.{{name}},""{{label}}""))
{
    _Logic.{{name}}ValueChange();
}
";

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
            GUILib.Area(new Rect(position.x, position.y, 200, 20), () =>
            {
                var val = 1f;
                GUILib.FloatField(ref val, TypeName);
            });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Float();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}