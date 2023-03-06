using Scriban;
using Scriban.Runtime;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_GameObject : EUM_Widget
    {
        private EUM_GameObject_Info info => Info as EUM_GameObject_Info;
        public override string TypeName => "GameObject";
        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_GameObject_Info(this);
            info.Label = TypeName;
            return info;
        }


        protected override void OnDrawLayout()
        {
            GUILib.ObjectField(info.Label, ref info.Value,LayoutOptions());
        }

        public override string LogicCode()
        {
            var code = @"public GameObject {{name}};
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
                GameObject val = null;
                GUILib.ObjectField(TypeName, ref val);
            });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_GameObject();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}