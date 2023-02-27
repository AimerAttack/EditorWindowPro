using Amazing.Editor.Library;
using Scriban;
using Scriban.Runtime;
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
            GUI.Button(new Rect(position.x,position.y,100,20), TypeName);
        }

        protected override void OnDrawLayout()
        {
            var style = new GUIStyle(GUI.skin.button);
            style.alignment = info.TextAnchor;
            GUILayout.Button(_Content,style);
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
                @"if(GUILayout.Button(""{{name}}""))
{
    _Logic.OnClick{{name}}();
}
";
            
            var sObj = new ScriptObject();
            sObj.Add("name", Name);

            var context = new TemplateContext();
            context.AutoIndent = true;
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);
            
            return result;
        }
    }
}