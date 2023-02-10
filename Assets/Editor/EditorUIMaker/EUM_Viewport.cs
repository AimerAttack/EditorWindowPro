using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Viewport : I_EUM_Drawable
    {
        private EUM_Title _Title;
        private EUM_Toolbar _Toolbar;
        private EUM_Paper _Paper;
        
        public EUM_Viewport()
        {
            _Title = new EUM_Title(new GUIContent("Viewport"));
            _Toolbar = new EUM_Toolbar();
            _Paper = new EUM_Paper();
        }
        
        public void DrawWithRect(ref Rect rect)
        {
            _Title.DrawWithRect(ref rect);
            _Toolbar.DrawWithRect(ref rect);

            EUM_Helper.ViewportRect = rect;
            _Paper.DrawWithRect(ref rect);
        }
    }
}