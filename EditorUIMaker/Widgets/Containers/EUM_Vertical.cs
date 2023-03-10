using EditorUIMaker.Widgets;
using Scriban;
using Scriban.Runtime;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Vertical : EUM_Container
    {
        public override string IconName => "d_VerticalLayoutGroup Icon";
        public override string TypeName => "Vertical";
        protected override EUM_BaseInfo CreateInfo()
        {
            return new EUM_NormalInfo(this);
        }

        protected override void OnDrawLayout()
        {
            GUILib.VerticelRect(() =>
            {
                DrawItems();
            });
        }

        public override bool CanResize()
        {
            return false;
        }

        public override void DrawDraging(Vector2 position)
        {
            GUILib.Area(new Rect(position.x + 20, position.y, 200, 20), () =>
            {
                GUILib.Label(TypeName);
            });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Vertical();
            Info.CopyTo(widget.Info);
            foreach (var w in Widgets)
            {
                widget.Widgets.Add(w.Clone());
            }
            return widget;
        }

        public override EUM_BaseWidget SingleClone()
        {
            var widget = new EUM_Vertical();
            Info.CopyTo(widget.Info);
            return widget;
        }
        
        protected override string BeginCode()
        {
            var code = @"GUILib.VerticelRect((() =>
{";
            return code;
        }
        
        protected override string EndCode()
        {
            var code = @"}),null,{{layout}});";
            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);
            var layout = LayoutOptionsStr();
            sObj.Add("layout",layout);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);
            
            return result;
        }
    }
}