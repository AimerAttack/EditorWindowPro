using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public abstract class EUM_BaseWidget : I_EUM_LayoutDrawable, I_EUM_Depth, I_EUM_Draggable
    {
        public bool InViewport = false;
        public Rect Rect;
        public Rect CheckRect;
        public EUM_Container Parent;
        public abstract string TypeName { get; }

        protected abstract void OnDrawLayout();
        public void DrawLayout()
        {
            OnDrawLayout();
            if (Event.current.type == EventType.Repaint)
            {
                var selfRect = GUILayoutUtility.GetLastRect();
                CheckRect = new Rect(Parent.CheckRect.x + selfRect.x,Parent.CheckRect.y + selfRect.y,selfRect.width, selfRect.height);
                Rect = CheckRect;
            }
        }
        public bool Contains(Vector2 point)
        {
            return CheckRect.Contains(point);
        }

        public int Depth { get; set; }

        public abstract void DrawDraging(Vector2 position);
        
        public abstract EUM_BaseWidget Clone();

        public void OnAddToContainer(EUM_Container container)
        {
            Parent = container;
            Depth = container.Depth + 1;
            InViewport = true;
        }

    }
}