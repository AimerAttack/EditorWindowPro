using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public abstract class EUM_BaseWidget : I_EUM_Drawable,I_EUM_LayoutDrawable,I_EUM_Draggable
    {
        public abstract string TypeName { get; }
        public abstract void DrawDraging(float x,float y);
        public abstract void DrawLayout();
        public abstract EUM_BaseWidget Clone();
        public abstract void Draw(ref Rect rect);
    }
}