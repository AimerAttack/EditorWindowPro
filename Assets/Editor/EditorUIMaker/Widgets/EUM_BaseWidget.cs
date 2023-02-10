using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public abstract class EUM_BaseWidget : I_EUM_Drawable,I_EUM_LayoutDrawable
    {
        public abstract string TypeName { get; }
        public abstract void DrawWithRect(ref Rect rect);
        public abstract void DrawLayout();
        public abstract EUM_BaseWidget Clone();
    }
}