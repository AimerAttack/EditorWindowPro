using Scriban;
using Scriban.Runtime;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Slider : EUM_Widget
    {
        private EUM_Slider_Info info => Info as EUM_Slider_Info;
        public override string TypeName => "Slider";
        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Slider_Info(this);
            info.Label = TypeName;
            return info;
        }

        protected override void OnDrawLayout()
        {
            info.Value = EditorGUILayout.Slider(info.Label,info.Value, info.Min, info.Max);
        }

        public override string LogicCode()
        {
            var code = @"public float {{name}};
public float {{name}}Min={{min}};
public float {{name}}Max={{max}};

public void {{name}}ValueChange()
{
    CallMethod(""On{{name}}ValueChange"");
}
";
            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);
            sObj.Add("min",info.Min);
            sObj.Add("max",info.Max);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);
            
            return result;
        }

        public override string Code()
        {
            var code =
                @"GUILayout.BeginHorizontal();
var tmp{{name}} = EditorGUILayout.Slider(""{{label}}"",_Logic.{{name}}, _Logic.{{name}}Min, _Logic.{{name}}Max);
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

        public override void DrawDraging(Vector2 position)
        {
            GUILayout.BeginArea(new Rect(position.x,position.y,300,20));
            EditorGUILayout.Slider(TypeName,5,0,10);
            GUILayout.EndArea();
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Slider();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}