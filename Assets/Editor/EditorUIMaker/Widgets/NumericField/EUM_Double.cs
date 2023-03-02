using Scriban;
using Scriban.Runtime;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Double : EUM_Widget
    {
        private EUM_Double_Info info => Info as EUM_Double_Info;
        public override string TypeName => "Double";

        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Double_Info(this);
            info.Label = TypeName;
            return info;
        }

        protected override void OnDrawLayout()
        {
            GUILib.DoubleField(ref info.Value, info.Label);
        }

        public override string LogicCode()
        {
            var code = @"public double {{name}};

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
if(GUILib.DoubleField(ref _Logic.{{name}},""{{label}}""))
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
                var val = 1d;
                GUILib.DoubleField(ref val, TypeName);
            });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Double();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}