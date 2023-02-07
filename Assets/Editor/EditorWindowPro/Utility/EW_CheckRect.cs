using Amazing.Editor.Library;
using UnityEngine;

namespace EditorWindowPro.Utility
{
    public class EW_CheckRect : I_EW_ID,I_EW_Contains
    {
        private Rect Rect;

        public bool Selected = false;
        
        public EW_CheckRect(Rect rect)
        {
            Rect = rect;
        }

        public void Draw()
        {
            GUILib.Rect(Rect, Selected ? Color.green : Color.black, Selected ? 0.5f : 0.1f);
        }

        public int ID
        {
            get
            {
                return Rect.GetHashCode();
            }
        }

        public bool Contains(Vector2 position)
        {
            return Rect.Contains(position);
        }
    }
}