using EditorUIMaker.Utility;
using Scriban;
using Scriban.Runtime;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Toggle : EUM_Widget
    {
        public override GUIIconLib.E_Icon IconType => GUIIconLib.E_Icon.Toggle;
        private EUM_Toggle_Info info => Info as EUM_Toggle_Info;
        public override string TypeName => "Toggle";

        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Toggle_Info(this);
            info.Label = TypeName;
            return info;
        }

        protected override void OnDrawLayout()
        {
            GUILib.Toggle(ref info.Value, info.Label, null, LayoutOptions());
        }

        public override string LogicCode()
        {
            var code = @"public bool {{name}};

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
if(GUILib.Toggle(ref _Logic.{{name}},""{{label}}"",{{layout}}))
{
    _Logic.{{name}}ValueChange();
}
";

            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);
            sObj.Add("label", info.Label);
            var layoutString = LayoutOptionsStr();
            sObj.Add("layout", layoutString);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);

            return result;
        }

        public override void DrawDraging(Vector2 position)
        {
            GUILib.Area(new Rect(position.x + 20, position.y, 200, 20), () =>
            {
                var val = true;
                GUILib.Toggle(ref val, TypeName);
            });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Toggle();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}