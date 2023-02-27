using EditorUIMaker.Widgets;
using Scriban;
using Scriban.Runtime;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class TestScriban
    {
        [MenuItem("Tools/Do")]
        public static void Do()
        {
            Logic2();
        }

        static void Logic2()
        {
            var str = @"void OnGUI()
{
    {{strBtn}}
}
";

            var btn = new EUM_Button();
            btn.Name = "Button1";
            var sObj = new ScriptObject();
            sObj.Add("strBtn", btn.Code());

            var context = new TemplateContext();
            context.AutoIndent = true;
            context.PushGlobal(sObj);

            var template = Template.Parse(str);
            var result = template.Render(context);

            Debug.Log(result); 
        }

        static void Logic()
        {
            var str =
                @"if(GUILayout.Button({{btnName}}))
{
    _Logic.{{btnFuncName}}();
}
";

            var sObj = new ScriptObject();
            sObj.Add("btnName", "Button1");
            sObj.Add("btnFuncName", "ClickButton1");

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var template = Template.Parse(str);
            var result = template.Render(context);

            Debug.Log(result);
        }
    }
}