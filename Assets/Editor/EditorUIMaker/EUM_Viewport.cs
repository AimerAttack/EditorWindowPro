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
            _Title.Draw(ref rect);
            _Toolbar.Draw(ref rect);
            _Paper.Draw(ref rect);
        }
    }
}