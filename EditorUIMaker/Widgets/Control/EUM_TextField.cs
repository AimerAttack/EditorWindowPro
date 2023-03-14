using EditorUIMaker.Utility;
using Scriban;
using Scriban.Runtime;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_TextField : EUM_Widget
    {
        public override GUIIconLib.E_Icon IconType => GUIIconLib.E_Icon.TextField;
        private EUM_TextField_Info info => Info as EUM_TextField_Info;
        public override string TypeName => "TextField";

        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_TextField_Info(this);
            info.Label = TypeName;
            return info;
        }

        protected override void OnDrawLayout()
        {
            GUILib.HorizontalRect(() =>
            {
                GUILib.Label(info.Label, LayoutOptions());
                GUILib.TextField(ref info.Value, LayoutOptions());
            });
        }

        public override string Code()
        {
            var code =
                @"GUILib.HorizontalRect(()=>
{
    GUILib.Label(""{{label}}"",{{layout}});
    if(GUILib.TextField(ref _Logic.{{name}},{{layout}}))
    {
        _Logic.{{name}}ValueChange();
    }
});
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

        public override string LogicCode()
        {
            var code = @"public string {{name}};
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

        public override void DrawDragging(Vector2 position)
        {
            GUILib.Area(new Rect(position.x + 20, position.y, 200, 20), () =>
            {
                GUILib.HorizontalRect(() =>
                {
                    GUILib.Label(TypeName);
                    var val = "";
                    GUILib.TextField(ref val);
                });
            });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_TextField();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}