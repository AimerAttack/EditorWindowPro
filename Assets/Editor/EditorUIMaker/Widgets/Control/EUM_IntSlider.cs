using Scriban;
using Scriban.Runtime;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_IntSlider : EUM_Widget
    {
        private EUM_IntSlider_Info info => Info as EUM_IntSlider_Info;
        public override string TypeName => "IntSlider";
        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_IntSlider_Info(this);
            info.Label = TypeName;
            return info;
        }

        protected override void OnDrawLayout()
        {
            info.Value = EditorGUILayout.IntSlider(info.Label,info.Value, info.Min, info.Max);
        }

        public override string LogicCode()
        {
            var code = @"public int {{name}};
public int {{name}}Min={{min}};
public int {{name}}Max={{max}};

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
var tmp{{name}} = EditorGUILayout.IntSlider(""{{label}}"",_Logic.{{name}}, _Logic.{{name}}Min, _Logic.{{name}}Max);
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
            GUI.BeginGroup(new Rect(position.x,position.y,1000,20));
            GUI.Label(new Rect(0,0,100,20), TypeName);
            GUI.HorizontalSlider(new Rect(100,0,100,20),0,0,1);
            GUI.EndGroup(); 
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_IntSlider();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}