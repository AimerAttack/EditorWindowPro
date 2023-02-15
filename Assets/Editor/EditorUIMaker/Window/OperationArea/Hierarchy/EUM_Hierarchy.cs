using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Hierarchy : I_EUM_Drawable
    {
        private EUM_Title _Title;

        public EUM_Hierarchy()
        {
            _Title = new EUM_Title(new GUIContent("Hierarchy"));
        }

        public void Draw(ref Rect rect)
        {
            _Title.Draw(ref rect);
 
        }
    }
}