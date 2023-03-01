using Scriban;
using Scriban.Runtime;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Vector2Int : EUM_Widget
    {
        private EUM_Vector2Int_Info info => Info as EUM_Vector2Int_Info;
        public override string TypeName => "Vector2Int";
        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Vector2Int_Info(this);
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
            GUILayout.BeginArea(new Rect(position.x,position.y,200,40));
            EditorGUILayout.Vector2IntField(TypeName,Vector2Int.zero);
            GUILayout.EndArea();
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Vector2Int();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}