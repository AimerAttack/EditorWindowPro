using UnityEditor;
using UnityEngine;

namespace EditorWindowPro.Utility
{
    public abstract class EW_BaseWidget : I_EW_Drawable,I_EW_ID,I_EW_Contains
    {
        public abstract int ID { get; }
        public Rect Rect;

        public virtual void Draw()
        {
            EditorGUIUtility.AddCursorRect(Rect,MouseCursor.MoveArrow);
        }
        public abstract bool Contains(Vector2 position);

        public void SetPosition(float x, float y)
        {
            Rect.Set(x,y,Rect.width,Rect.height);
        }
        
        public void SetPosition(Vector2 position)
        {
            SetPosition(position.x,position.y);
        }
    }
}