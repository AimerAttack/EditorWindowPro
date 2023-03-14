using EditorUIMaker.Widgets;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Window : EUM_Container, I_EUM_Drawable
    {
        public override string TypeName => "Window";

        protected override EUM_BaseInfo CreateInfo()
        {
            return new EUM_NormalInfo(this);
        }

        public EUM_Window()
        {
            InViewport = true;
        }

        protected override void OnDrawLayout()
        {
            DrawItems();
        }

        public override void DrawDragging(Vector2 position)
        {
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Window();
            Info.CopyTo(widget.Info);
            foreach (var w in Widgets)
            {
                widget.Widgets.Add(w.Clone());
            }

            return widget;
        }

        public override EUM_BaseWidget SingleClone()
        {
            return Clone();
        }

        public void Draw(ref Rect rect)
        {
            Rect = rect;
            OnDraw(rect);
        }


        void OnDraw(Rect rect)
        {
            AbsoluteRect = new Rect(EUM_Helper.Instance.VitualWindowRect.x, EUM_Helper.Instance.VitualWindowRect.y,
                rect.width, rect.height);
            DrawItems();
        }

        protected override void DrawItems()
        {
            if (!EUM_Helper.Instance.Preview && InViewport)
            {
                GUI.enabled = false;
                GUI.color = new Color(1, 1, 1, 2);
            }

            GUILib.Area(Rect, () =>
            {
                foreach (var widget in Widgets)
                {
                    widget.DrawLayout();
                }
            });

            if (!EUM_Helper.Instance.Preview && InViewport)
            {
                GUI.color = Color.white;
                GUI.enabled = true;
            }
        }

        protected override string BeginCode()
        {
            return string.Empty;
        }

        protected override string EndCode()
        {
            return string.Empty;
        }
    }
}