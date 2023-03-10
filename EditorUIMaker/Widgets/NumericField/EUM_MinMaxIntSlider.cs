using EditorUIMaker.Utility;
using Scriban;
using Scriban.Runtime;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_MinMaxIntSlider : EUM_Widget
    {
        public override GUIIconLib.E_Icon IconType => GUIIconLib.E_Icon.MinMaxIntSlider;
        public EUM_MinMaxIntSlider_Info info => Info as EUM_MinMaxIntSlider_Info;
        public override string TypeName => "Min Max Int Slider";

        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_MinMaxIntSlider_Info(this);
            info.Label = TypeName;
            return info;
        }

        protected override void OnDrawLayout()
        {
            GUILib.IntMinMaxSlider(TypeName, ref info.MinValue, ref info.MaxValue, info.Min, info.Max,false,LayoutOptions());
        }

        public override string LogicCode()
        {
            var code = @"public int min{{name}};
public int max{{name}};

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
            var code =
                @"if(GUILib.IntMinMaxSlider(""{{label}}"",ref _Logic.min{{name}},ref _Logic.max{{name}},{{min}},{{max}},{{layout}}))
{
    _Logic.{{name}}ValueChange();
}
";

            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);
            sObj.Add("label", info.Label);
            sObj.Add("min", info.Min);
            sObj.Add("max", info.Max);
            var layoutString = LayoutOptionsStr();
            sObj.Add("layout",layoutString);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(code);
            var result = template.Render(context);

            return result;
        }

        public override void DrawDraging(Vector2 position)
        {
            GUILib.Area(new Rect(position.x, position.y, 300, 30),
                () => { GUILib.IntMinMaxSlider(TypeName, ref info.MinValue, ref info.MaxValue, info.Min, info.Max); });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_MinMaxIntSlider();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}