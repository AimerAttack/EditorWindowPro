using System.Collections.Generic;
using System.Text;
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
            var builder = new StringBuilder();
            foreach (var widget in Widgets)
            {
                var widgetCode = widget.Code();
                if(string.IsNullOrEmpty(widgetCode))
                    continue;
                builder.AppendJoin("\n",widgetCode);
            }

            var code = builder.ToString();
            return code;
        }

        public override string LogicCode()
        {
            var builder = new StringBuilder();
            foreach (var widget in Widgets)
            {
                var logicStr = widget.LogicCode();
                if(string.IsNullOrEmpty(logicStr))
                    continue;
                builder.AppendJoin("\n",logicStr);
            }

            var code = builder.ToString();
            return code;
        }
        
        public override string CodeForDefine()
        {
            var builder = new StringBuilder();
            foreach (var widget in Widgets)
            {
                var defineStr = widget.CodeForDefine();
                if(string.IsNullOrEmpty(defineStr))
                    continue;
                builder.AppendJoin("\n",defineStr);
            }

            var code = builder.ToString();
            return code;
        }
        
        public override string CodeForInit()
        {
            var builder = new StringBuilder();
            foreach (var widget in Widgets)
            {
                var initStr = widget.CodeForInit();
                if(string.IsNullOrEmpty(initStr))
                    continue;
                builder.AppendJoin("\n",initStr);
            }

            var code = builder.ToString();
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
{{endCode}}";
            var template = Template.Parse(code);
            var result = template.Render(context);
            
            return result;
        }
    }
}