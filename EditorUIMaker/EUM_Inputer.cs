using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Inputer
    {
        public void CheckInput()
        {
            if (Event.current.isKey && Event.current.keyCode == KeyCode.S && Event.current.type == EventType.KeyDown &&
                Event.current.control)
            {
                EUM_Helper.Instance.SaveFile();
            }

            if (Event.current.isKey && Event.current.keyCode == KeyCode.Delete &&
                Event.current.type == EventType.KeyDown)
            {
                if (EUM_Helper.Instance.SelectWidget != null)
                {
                    var widget = EUM_Helper.Instance.SelectWidget;
                    if (widget.Depth != 0)
                    {
                        widget.Parent.Widgets.Remove(widget);

                        EUM_Helper.Instance.SelectWidget = null;
                        EUM_Helper.Instance.OnRemoveItemFromWindow?.Invoke(widget);
                        EUM_Helper.Instance.OnSelectWidgetChange?.Invoke();
                    }
                }
            }

            if (Event.current.isKey && Event.current.keyCode == KeyCode.C && Event.current.type == EventType.KeyDown &&
                Event.current.control)
            {
                if (EUM_Helper.Instance.SelectWidget != null)
                {
                    EUM_Helper.Instance.ClipboardWidget = EUM_Helper.Instance.SelectWidget.Clone();
                }
            }

            if (Event.current.isKey && Event.current.keyCode == KeyCode.V && Event.current.type == EventType.KeyDown &&
                Event.current.control)
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

            ProcessScrollWheel();
            ProcessMouseMove();
        }

        void ProcessMouseMove()
        {
            if (Event.current.isMouse)
            {
                if (EUM_Helper.Instance.CheckCanvasDrag)
                {
                    if (Event.current.rawType == EventType.MouseUp)
                    {
                        EUM_Helper.Instance.CheckCanvasDrag = false;
                        Event.current.Use();
                    }
                    else
                    {
                        var distance = Vector2.Distance(Event.current.mousePosition,
                            EUM_Helper.Instance.StartDragCanvasPosition);
                        if (distance > EUM_Helper.MinimumDragToSnapToMoveRotateScaleResize)
                        {
                            var deltaX = Event.current.mousePosition.x - EUM_Helper.Instance.StartDragCanvasPosition.x;
                            var deltaY = Event.current.mousePosition.y - EUM_Helper.Instance.StartDragCanvasPosition.y;

                            var scale = EUM_Helper.GetZoomScaleFactor();
                            deltaX /= scale;
                            deltaY /= scale;

                            EUM_Helper.Instance.StartDragCanvasPosition.x = Event.current.mousePosition.x;
                            EUM_Helper.Instance.StartDragCanvasPosition.y = Event.current.mousePosition.y;
                            EUM_Helper.Instance.WindowRect.x += deltaX;
                            EUM_Helper.Instance.WindowRect.y += deltaY;
                        }
                    }
                }
                else
                {
                    if (Event.current.button == 1)
                    {
                        if (Event.current.type == EventType.MouseDown)
                        {
                            EUM_Helper.Instance.StartDragCanvasPosition.x = Event.current.mousePosition.x;
                            EUM_Helper.Instance.StartDragCanvasPosition.y = Event.current.mousePosition.y;
                            EUM_Helper.Instance.CheckCanvasDrag = true;
                            Event.current.Use();
                        }
                    }
                }
            }
        }


        void ProcessScrollWheel()
        {
            return;
            if (Event.current.type == EventType.ScrollWheel)
            {
                var zoomDelta = EUM_Helper.Instance.ZoomIndex;
                if (Event.current.delta.y < 0)
                    zoomDelta++;
                else
                {
                    zoomDelta--;
                }

                if (zoomDelta < 0)
                    zoomDelta = 0;
                if (zoomDelta >= EUM_Helper.ZoomScales.Length)
                    zoomDelta = EUM_Helper.ZoomScales.Length - 1;

                EUM_Helper.Instance.ZoomIndex = zoomDelta;
                EUM_Helper.Instance.MousePosition = Event.current.mousePosition;
            }
        }
    }
}