using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Inspector : I_EUM_Drawable
    {
        private EUM_Title _Title;
        
        public EUM_Inspector()
        {
            _Title = new EUM_Title(new GUIContent("Inspector"));
        }
        
        public void DrawWithRect(ref Rect rect)
        {
            GUILib.Rect(rect,GUILib.s_DefaultColor , 1f);
            
            _Title.DrawWithRect(ref rect);
        }
    }
}