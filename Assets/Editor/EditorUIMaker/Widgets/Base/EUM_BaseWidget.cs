using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public abstract class EUM_BaseWidget : I_EUM_LayoutDrawable, I_EUM_Depth, I_EUM_Draggable
    {
        public int ID;
        public bool InViewport = false;
        public Rect Rect;
        public Rect AbsoluteRect;
        public EUM_Container Parent;
        public abstract string TypeName { get; }

        public EUM_BaseInfo Info;
        protected abstract EUM_BaseInfo CreateInfo();
        
        public EUM_BaseWidget()
        {
            ID = EUM_Helper.Instance.WidgetID++;
            Info = CreateInfo();
            Info.Name = TypeName;
        }

        protected abstract void OnDrawLayout();
        public void DrawLayout()
        {
            OnDrawLayout();
            if (Event.current.type == EventType.Repaint)
            {
                var selfRect = GUILib.GetLastRect();
                AbsoluteRect = new Rect(Parent.AbsoluteRect.x + selfRect.x,Parent.AbsoluteRect.y + selfRect.y,selfRect.width, selfRect.height);
                Rect = AbsoluteRect;
                FixAbsoluteRect();
            }
        }

        public abstract string LogicCode();

        public abstract string Code();

        public virtual string CodeForDefine()
        {
            return string.Empty;
        }

        public virtual string CodeForInit()
        {
            return string.Empty;
        }

        protected virtual void FixAbsoluteRect()
        {
        }
        
        public bool Contains(Vector2 point)
        {
            return AbsoluteRect.Contains(point);
        }

        [SerializeField]
        public int Depth { get; set; }

        public abstract void DrawDraging(Vector2 position);
        
        public abstract EUM_BaseWidget Clone();
        public abstract EUM_BaseWidget SingleClone();

        public void OnAddToContainer(EUM_Container container)
        {
            Parent = container;
            Depth = container.Depth + 1;
            InViewport = true;
        }

    }
}