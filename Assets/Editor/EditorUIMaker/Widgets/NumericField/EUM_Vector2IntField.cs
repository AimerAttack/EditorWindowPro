using Scriban;
using Scriban.Runtime;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Vector2IntField : EUM_Widget
    {
        private EUM_Vector2IntField_Info info => Info as EUM_Vector2IntField_Info;
        public override string TypeName => "Vector2IntField";
        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Vector2IntField_Info(this);
            info.Label = TypeName;
            return info;
        }

        protected override void OnDrawLayout()
        {
            info.Value = EditorGUILayout.Vector2IntField(info.Label,info.Value);
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
            var code =@"
var tmp{{name}} = EditorGUILayout.Vector2IntField(""{{label}}"",_Logic.{{name}});
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
            GUI.BeginGroup(new Rect(position.x,position.y,1000,20));
            GUI.Label(new Rect(0,0,100,20), TypeName);
            GUI.TextField(new Rect(100, 0, 50, 20), "");
            GUI.TextField(new Rect(160, 0, 50, 20), "");
            GUI.EndGroup(); 
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Vector2IntField();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}