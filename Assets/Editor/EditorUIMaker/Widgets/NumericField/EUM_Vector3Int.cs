using Scriban;
using Scriban.Runtime;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Vector3Int : EUM_Widget
    {
        private EUM_Vector3Int_Info info => Info as EUM_Vector3Int_Info;
        public override string TypeName => "Vector3Int";

        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Vector3Int_Info(this);
            info.Label = TypeName;
            return info;
        }

        protected override void OnDrawLayout()
        {
            GUILib.Vector3IntField(info.Label, ref info.Value);
        }

        public override string LogicCode()
        {
            var code = @"public Vector3Int {{name}};

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
if(GUILib.Vector3IntField(""{{label}}"",ref _Logic.{{name}}))
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
            GUILib.Area(new Rect(position.x, position.y, 200, 40), () =>
            {
                var val = Vector3Int.zero;
                GUILib.Vector3IntField(TypeName, ref val);
            });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Vector3Int();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}