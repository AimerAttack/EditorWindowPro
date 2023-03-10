using EditorUIMaker.Utility;
using Scriban;
using Scriban.Runtime;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Material : EUM_Widget
    {
        public override GUIIconLib.E_Icon IconType=> GUIIconLib.E_Icon.Material;
        private EUM_Material_Info info => Info as EUM_Material_Info;
        public override string TypeName => "Material";
        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Material_Info(this);
            info.Label = TypeName;
            return info;
        }

        protected override void OnDrawLayout()
        {
            GUILib.ObjectField(info.Label, ref info.Value,LayoutOptions());
        }

        public override string LogicCode()
        {
            var code = @"public Material {{name}};
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
            var code =
                @"if(GUILib.ObjectField(""{{label}}"",ref _Logic.{{name}},{{layout}}))
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
            GUILib.Area(new Rect(position.x, position.y, 300, 20), () =>
            {
                Material val = null;
                GUILib.ObjectField(TypeName, ref val);
            });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Material();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}