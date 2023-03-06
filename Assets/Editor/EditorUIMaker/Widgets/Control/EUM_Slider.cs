using Scriban;
using Scriban.Runtime;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Slider : EUM_Widget
    {
        private EUM_Slider_Info info => Info as EUM_Slider_Info;
        public override string TypeName => "Slider";

        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Slider_Info(this);
            info.Label = TypeName;
            return info;
        }

        protected override void OnDrawLayout()
        {
            GUILib.Slider(info.Label, ref info.Value, info.Min, info.Max,LayoutOptions());
        }

        public override string LogicCode()
        {
            var code = @"public float {{name}};
public float {{name}}Min={{min}};
public float {{name}}Max={{max}};

public void {{name}}ValueChange()
{
    CallMethod(""On{{name}}ValueChange"");
}
";
            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);
            sObj.Add("min", info.Min);
            sObj.Add("max", info.Max);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);

            return result;
        }

        public override string Code()
        {
            var code =
                @"if(GUILib.Slider(""{{label}}"",ref _Logic.{{name}}, _Logic.{{name}}Min, _Logic.{{name}}Max,{{layout}}))
{
    _Logic.{{name}}ValueChange();
}
";

            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);
            sObj.Add("label", info.Label);
            var layoutString = LayoutOptionsStr();
            sObj.Add("layout",layoutString);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);

            return result;
        }

        public override void DrawDraging(Vector2 position)
        {
            GUILib.Area(new Rect(position.x, position.y, 300, 20),
                () =>
                {
                    var val = 1f;
                    GUILib.Slider(TypeName, ref val, 0, 10);
                });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Slider();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}