using UnityEditor;
using UnityEngine;

namespace EditorWindowPro.Utility
{
    public abstract class EW_BaseWidget : I_EW_Drawable, I_EW_ID, I_EW_Contains
    {
        public abstract int ID { get; }
        public Rect Rect;
        protected float _X, _Y;
        private float _RectX, _RectY;
        private float _OffsetX = 0, _OffsetY = 0;

        public EW_BaseWidget(Rect rect)
        {
            Rect = rect;
            _X = rect.x;
            _Y = rect.y;
        }

        public virtual void Draw(Rect rect)
        {
            _RectX = rect.x;
            _RectY = rect.y;
            Rect.Set(_X + _RectX + _OffsetX, _Y + _RectY + _OffsetY, Rect.width, Rect.height);
            EditorGUIUtility.AddCursorRect(Rect, MouseCursor.MoveArrow);
        }

        public bool Contains(Vector2 position)
        {
            return Rect.Contains(position);
        }

        public void SetPosition(float x, float y)
        {
            _OffsetX = x - _X - _RectX;
            _OffsetY = y - _Y - _RectY;
        }

        public void SetPosition(Vector2 position)
        {
            SetPosition(position.x, position.y);
        }
    }
}