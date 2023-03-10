using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Paper : I_EUM_Drawable
    {
        public EUM_VitualWindow VitualWindow;

        public EUM_Paper()
        {
            VitualWindow = new EUM_VitualWindow();
        }
        
        public void Draw(ref Rect rect)
        {
            GUILib.Rect(rect, Color.black, 0.8f);
            
            VitualWindow.Draw(ref rect);
        }
    }
}