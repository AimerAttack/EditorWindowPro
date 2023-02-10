using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_SelectArea : I_EUM_Drawable
    {
        public void Draw(ref Rect rect)
        {
            GUILib.Frame(rect,Color.green,2);
        }
    }
}