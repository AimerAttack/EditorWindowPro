using EditorUIMaker.Utility;
using EditorUIMaker.Widgets;
using Scriban;
using Scriban.Runtime;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_ScrollView : EUM_Container
    {
        public override GUIIconLib.E_Icon IconType=> GUIIconLib.E_Icon.ScrollView;
        private Vector2 _ScrollPosition;
        public override string TypeName => "ScrollView";
        protected override EUM_BaseInfo CreateInfo()
        {
            return new EUM_NormalInfo(this);
        }

        protected override void OnDrawLayout()
        {
            GUILib.ScrollView(ref _ScrollPosition, () =>
            {
                DrawItems();
            },LayoutOptions());
        }

        protected override void FixAbsoluteRect()
        {
            AbsoluteRect.xMin -= _ScrollPosition.x;
            AbsoluteRect.yMin -= _ScrollPosition.y;
        }

        public override void DrawDragging(Vector2 position)
        {
            GUILib.Area(new Rect(position.x + 20, position.y, 200, 20), () =>
            {
                GUILib.Label(TypeName);
            });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_ScrollView();
            Info.CopyTo(widget.Info);
            
            foreach (var w in Widgets)
            {
                widget.Widgets.Add(w.Clone());
            }
            return widget;
        }

        public override EUM_BaseWidget SingleClone()
        {
            var widget = new EUM_ScrollView();
            Info.CopyTo(widget.Info);
            return widget;
        }

        public override string LogicCode()
        {
            var code = @"public Vector2 {{name}};";
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
            var code = @"GUILib.ScrollView(ref _Logic.{{name}},() =>
{";
            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);
            
            return result;
        }
        
        protected override string EndCode()
        {
            var code = @"},{{layout}});";
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