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
        
        public void Draw(ref Rect rect)
        {
            var paperRect = new Rect(rect.x, rect.y, rect.width, rect.height);
            paperRect.yMin += EUM_Title.s_TitleHeight;
            paperRect.yMin += EUM_Toolbar.s_Height;
            
            EUM_Helper.Instance.ViewportRect = paperRect;
            _Paper.Draw(ref paperRect);
            
            _Title.Draw(ref rect);
            _Toolbar.Draw(ref rect);
        }
        
        public void DrawRect(Rect rect)
        {
            
        }
    }
}