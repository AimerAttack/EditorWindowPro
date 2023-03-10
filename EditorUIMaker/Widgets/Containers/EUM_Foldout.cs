using EditorUIMaker.Utility;
using EditorUIMaker.Widgets;
using Scriban;
using Scriban.Runtime;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Foldout : EUM_Container
    {
        public override GUIIconLib.E_Icon IconType => GUIIconLib.E_Icon.Foldout;
        private EUM_Foldout_Info info => Info as EUM_Foldout_Info;
        public override string TypeName => "Foldout";

        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Foldout_Info(this);
            info.Label = TypeName;
            return info;
        }

        public override bool CanResize()
        {
            return false;
        }

        protected override void OnDrawLayout()
        {
            GUILib.Foldout(info.Label, ref info.IsOpen);
            if (info.IsOpen)
                DrawItems();
        }

        public override void DrawDraging(Vector2 position)
        {
            GUILib.Area(new Rect(position.x, position.y, 300, 30), () =>
            {
                var foldOut = false;
                GUILib.Foldout(TypeName, ref foldOut);
            });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Foldout();
            Info.CopyTo(widget.Info);

            foreach (var w in Widgets)
            {
                widget.Widgets.Add(w.Clone());
            }

            return widget;
        }

        public override EUM_BaseWidget SingleClone()
        {
            var widget = new EUM_Foldout();
            Info.CopyTo(widget.Info);

            return widget;
        }

        public override string LogicCode()
        {
            var code = @"public bool {{name}} = false;";

            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);

            return result + "\n" + base.LogicCode();
        }

        protected override string BeginCode()
        {
            var code = @"GUILib.Foldout(""{{label}}"",ref _Logic.{{name}});
if(_Logic.{{name}})
{
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

        protected override string EndCode()
        {
            return "}";
        }
    }
}