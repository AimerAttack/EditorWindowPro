using Scriban;
using Scriban.Runtime;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Button : EUM_Widget
    {
        private string _Content
        {
            get
            {
                if (Info != null)
                {
                    var info = Info as EUM_Button_Info;
                    return string.IsNullOrEmpty(info.Text) ? TypeName : info.Text;
                }
                else
                {
                    return TypeName;
                }
            }
        }
        
        private EUM_Button_Info info => Info as EUM_Button_Info;
        
        public override string TypeName => "Button";
        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Button_Info(this);
            return info;
        }

        public override void DrawDraging(Vector2 position)
        {
            GUILib.Area(new Rect(position.x,position.y,100,20), () =>
            {
                GUILib.Button(TypeName);
            });
        }

        protected override void OnDrawLayout()
        {
            var style = new GUIStyle(GUI.skin.button);
            style.alignment = info.TextAnchor;
            GUILib.Button(_Content,style,LayoutOptions());
        }
        
        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Button();
            Info.CopyTo(widget.Info);
            return widget;
        }

        public override string Code()
        {
            var code =
                @"var style{{name}} = new GUIStyle(GUI.skin.button);
style{{name}}.alignment = TextAnchor.{{textAnchor}};
if(GUILib.Button(""{{text}}"",style{{name}},{{layout}}))
{
    _Logic.Click{{name}}();
}
";
            
            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);
            sObj.Add("text",string.IsNullOrEmpty(info.Text) ? TypeName : info.Text);
            sObj.Add("textAnchor", info.TextAnchor.ToString());
            var layoutString = LayoutOptionsStr();
            sObj.Add("layout",layoutString);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);
            
            return result;
        }

        public override string LogicCode()
        {
            var code = @"public void Click{{name}}()
{
    CallMethod(""OnClick{{name}}"");
}";
            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);
            
            return result;
        }
    }
}