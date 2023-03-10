using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Viewport : I_EUM_Drawable
    { 
        public EUM_Title Title;
        public EUM_Toolbar Toolbar;
        public EUM_Paper Paper;
        
        public EUM_Viewport()
        {
            Title = new EUM_Title(new GUIContent("Viewport"));
            Toolbar = new EUM_Toolbar();
            Paper = new EUM_Paper();
        }
        
        public void Draw(ref Rect rect)
        {
            var paperRect = new Rect(rect.x, rect.y, rect.width, rect.height);
            paperRect.yMin += EUM_Title.s_TitleHeight;
            paperRect.yMin += EUM_Toolbar.s_Height;
            
            EUM_Helper.Instance.ViewportRect = paperRect;
            Paper.Draw(ref paperRect);
            
            Title.Draw(ref rect);
            Toolbar.Draw(ref rect);
        }
        
        public void DrawRect(Rect rect)
        {
            
        }
    }
}