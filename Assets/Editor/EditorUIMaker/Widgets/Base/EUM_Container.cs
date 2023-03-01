using System.Collections.Generic;
using EditorUIMaker.Widgets;
using Scriban;
using Scriban.Runtime;
using UnityEngine;

namespace EditorUIMaker
{
    public abstract class EUM_Container : EUM_BaseWidget
    {
        public bool Selected = false;
        public List<EUM_BaseWidget> Widgets = new List<EUM_BaseWidget>();

        public EUM_Container()
        {
            EUM_Helper.Instance.Containers.Add(this);
        }

        protected virtual void DrawItems()
        {
            foreach (var widget in Widgets)
            {
                widget.DrawLayout();
            }
        }


        protected abstract string BeginCode();

        protected abstract string EndCode();
        
        string WidgetsCode()
        {
            var code = "";
            foreach (var widget in Widgets)
            {
                code += "\n" + widget.Code();
            }
            return code;
        }

        public override string LogicCode()
        {
            var code = "";
            foreach (var widget in Widgets)
            {
                code += "\n" + widget.LogicCode();
            }
            return code;
        }

        public sealed override string Code()
        {
            var beginCode = BeginCode();
            var endCode = EndCode();
            var widgetsCode = WidgetsCode();
            
            var sObj = new ScriptObject();
            sObj.Add("beginCode", beginCode);
            sObj.Add("endCode",endCode);
            sObj.Add("widgetsCode",widgetsCode);

            var context = new TemplateContext();
            context.PushGlobal(sObj);

            var code = @"{{beginCode}}
{{widgetsCode}}
{{endCode}}
";
            var template = Template.Parse(code);
            var result = template.Render(context);
            
            return result;
        }
    }
}