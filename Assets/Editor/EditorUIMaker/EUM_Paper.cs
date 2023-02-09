using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Paper : I_EUM_Drawable
    {
        private EUM_VitualWindow _VitualWindow;

        public EUM_Paper()
        {
            _VitualWindow = new EUM_VitualWindow();
        }
        
        public void Draw(ref Rect rect)
        {
            GUILib.Rect(rect, Color.black, 0.8f);
            
            _VitualWindow.Draw(ref rect);
        }
    }
}