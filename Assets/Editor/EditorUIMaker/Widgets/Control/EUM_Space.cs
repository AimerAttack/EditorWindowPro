using Amazing.Editor.Library;
using Scriban;
using Scriban.Runtime;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Space : EUM_Widget
    {
        EUM_Space_Info info => Info as EUM_Space_Info;
        public override string TypeName => "Space";

        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Space_Info(this);
            return info;
        }

        public override void DrawDraging(Vector2 position)
        {
            var rect = new Rect(position.x, position.y, 200, 20);
            GUILib.Frame(rect, Color.white, 1);
            rect.x += 60;
            GUI.Label(rect,TypeName);
        }

        protected override void OnDrawLayout()
        {
            GUILayout.Space(info.Height);
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Space();
            Info.CopyTo(widget.Info);
            return widget;
        }

        public override string LogicCode()
        {
            return string.Empty;
        }

        public override string Code()
        {
            var code =
                @"GUILayout.Space({{pixel}});";
            
            var sObj = new ScriptObject();
            sObj.Add("pixel", info.Height);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);
            
            return result;
        }
    }
}