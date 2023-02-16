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
                    widget.Parent.Widgets.Remove(widget);
                    
                    EUM_Helper.Instance.SelectWidget = null;
                    EUM_Helper.Instance.OnRemoveItemFromWindow?.Invoke();
                    EUM_Helper.Instance.OnSelectWidgetChange?.Invoke();
                }
            }
        }
    }
}