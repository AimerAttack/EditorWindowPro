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
            info.Value = EditorGUILayout.DoubleField(info.Label, info.Value);
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
            var code =@"
var tmp{{name}} = EditorGUILayout.DoubleField(""{{label}}"",_Logic.{{name}});
if(tmp{{name}} != _Logic.{{name}})
{
    _Logic.{{name}} = tmp{{name}};
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
            GUILayout.BeginArea(new Rect(position.x,position.y,200,20));
            EditorGUILayout.DoubleField(TypeName, 0);
            GUILayout.EndArea();
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Double();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}