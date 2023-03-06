using Scriban;
using Scriban.Runtime;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Color : EUM_Widget
    {
        private EUM_Color_Info info => Info as EUM_Color_Info;
        public override string TypeName => "Color";
        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Color_Info(this);
            info.Label = TypeName;
            return info;
        }

        protected override void OnDrawLayout()
        {
            GUILib.HorizontalRect(() =>
            {
                GUILib.Color(info.Label,ref info.Value,LayoutOptions());
            });
        }

        public override string LogicCode()
        {
            var code = @"public Color {{name}};

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
                @"if(GUILib.Color(""{{label}}"",ref _Logic.{{name}},{{layout}}))
{
    _Logic.{{name}}ValueChange();
}
";
            
            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);
            sObj.Add("label",info.Label);
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
            GUILib.Area(new Rect(position.x + 20,position.y,100,20), () =>
            {
                var color = Color.white;
                GUILib.Color(TypeName,ref color);
            });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Color();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}