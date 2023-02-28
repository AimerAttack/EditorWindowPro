using Scriban;
using Scriban.Runtime;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_TextField : EUM_Widget
    {
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
            GUILayout.BeginHorizontal();
            GUILayout.Label(info.Label);
            info.Value = GUILayout.TextField(info.Value);
            GUILayout.EndHorizontal();
        }

        public override string Code()
        {
            var code =
                @"GUILayout.BeginHorizontal();
GUILayout.Label(""{{label}}"");
var tmp{{name}} = GUILayout.TextField(_Logic.{{name}});
if(tmp{{name}} != _Logic.{{name}})
{
    _Logic.{{name}} = tmp{{name}};
    _Logic.{{name}}ValueChange();
}
GUILayout.EndHorizontal();
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

        public override void DrawDraging(Vector2 position)
        {
            GUI.BeginGroup(new Rect(position.x,position.y,0,0));
            GUI.Label(new Rect(0,0,100,30), TypeName);
            GUI.TextField(new Rect(100,0,100,30), TypeName);
            GUI.EndGroup();
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_TextField();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}