using EditorUIMaker.Utility;
using Scriban;
using Scriban.Runtime;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Dropdown : EUM_Widget
    {
        public override GUIIconLib.E_Icon IconType=> GUIIconLib.E_Icon.Dropdown;
        private EUM_Dropdown_Info info => Info as EUM_Dropdown_Info;
        public override string TypeName => "Dropdown";

        protected override EUM_BaseInfo CreateInfo()
        {
            var info = new EUM_Dropdown_Info(this);
            return info;
        }

        protected override void OnDrawLayout()
        {
            GUILib.Popup(ref info.Value, info.Options,LayoutOptions());
        }

        public override string LogicCode()
        {
            var code = @"public string {{name}}Str =""1"";
public int {{name}}Index;
public string[] {{name}}Options = {""1"",""2"",""3""};

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
            var code = @"if(GUILib.Popup(ref _Logic.{{name}}Str,_Logic.{{name}}Options,{{layout}}))
{
    _Logic.{{name}}Index = Array.IndexOf(_Logic.{{name}}Options,_Logic.{{name}}Str);
    _Logic.{{name}}ValueChange();
}
";

            var sObj = new ScriptObject();
            sObj.Add("name", Info.Name);
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
            GUILib.Area(new Rect(position.x + 20, position.y, 100, 30), () =>
            {
                string a = "1";
                GUILib.Popup(ref a, new[] {"1", "2", "3"});
            });
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Dropdown();
            Info.CopyTo(widget.Info);
            return widget;
        }
    }
}