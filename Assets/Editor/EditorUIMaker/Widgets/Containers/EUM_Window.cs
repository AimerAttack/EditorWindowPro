using EditorUIMaker.Widgets;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Window : EUM_Container,I_EUM_Drawable
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

        public override void DrawDraging(Vector2 position)
        {
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Window();
            Info.CopyTo(widget.Info);
            return widget;
        }
        
        public void Draw(ref Rect rect)
        {
            Rect = rect;
            OnDraw(rect);
        }


        void OnDraw(Rect rect)
        {
            AbsoluteRect = new Rect(EUM_Helper.Instance.VitualWindowRect.x, EUM_Helper.Instance.VitualWindowRect.y, rect.width, rect.height);
            DrawItems();
        }
    }
}