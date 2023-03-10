using Scriban;
using Scriban.Runtime;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Vector2Int : EUM_Widget
    {
        private EUM_Vector2Int_Info info => Info as EUM_Vector2Int_Info;
        public override string TypeName => "Vector2 Int";

        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Vector2Int_Info(this);
            info.Label = TypeName;
            return info;
        }

        protected override void OnDrawLayout()
        {
            GUILib.Vector2IntField(info.Label, ref info.Value,LayoutOptions());
        }

        public override string LogicCode()
        {
            var code = @"public Vector2Int {{name}};

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
if(GUILib.Vector2IntField(""{{label}}"",ref _Logic.{{name}},{{layout}}))
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
            GUILib.Area(new Rect(position.x, position.y, 200, 40), () =>
            {
                var val = Vector2Int.zero;
                GUILib.Vector2IntField(TypeName, ref val);
            });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Vector2Int();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}