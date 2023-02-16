using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Inputer
    {
        public void CheckInput()
        {
            if(Event.current.isKey && Event.current.keyCode == KeyCode.Delete && Event.current.type == EventType.KeyDown)
            {
                if (EUM_Helper.Instance.SelectWidget != null)
                {
                    var widget = EUM_Helper.Instance.SelectWidget;
                    if (widget.Depth != 0)
                    {
                        widget.Parent.Widgets.Remove(widget);

                        EUM_Helper.Instance.SelectWidget = null;
                        EUM_Helper.Instance.OnRemoveItemFromWindow?.Invoke();
                        EUM_Helper.Instance.OnSelectWidgetChange?.Invoke();
                    }
                }
            }
            
            if(Event.current.isKey && Event.current.keyCode == KeyCode.C && Event.current.type == EventType.KeyDown && Event.current.control)
            {
                if(EUM_Helper.Instance.SelectWidget != null)
                {
                    EUM_Helper.Instance.ClipboardWidget = EUM_Helper.Instance.SelectWidget.Clone();
                }
            }
            
            if(Event.current.isKey && Event.current.keyCode == KeyCode.V && Event.current.type == EventType.KeyDown && Event.current.control)
            {
                if (EUM_Helper.Instance.ClipboardWidget != null)
                {
                    var widget = EUM_Helper.Instance.ClipboardWidget.Clone();

                    if (EUM_Helper.Instance.SelectWidget != null)
                    {
                        if (EUM_Helper.Instance.SelectWidget is EUM_Container)
                        {
                            EUM_Helper.Instance.AddToContainer(widget,
                                EUM_Helper.Instance.SelectWidget as EUM_Container);
                        }
                        else
                        {
                            EUM_Helper.Instance.AddToContainer(widget, EUM_Helper.Instance.SelectWidget.Parent);
                        }
                    }
                    else
                    {
                        EUM_Helper.Instance.AddToContainer(widget, EUM_Helper.Instance.Window);
                    }

                    EUM_Helper.Instance.SelectWidget = widget;
                    EUM_Helper.Instance.OnSelectWidgetChange?.Invoke();
                }
            }
        }
    }
}