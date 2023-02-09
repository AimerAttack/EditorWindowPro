using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_VitualWindow : I_EUM_Drawable
    {
        private Rect _ContentRect;
        
        public void Draw(ref Rect rect)
        {
            _ContentRect = rect;
            DrawContent();
        }

        void DrawContent()
        {
            var rect = new Rect(_ContentRect.x - 100, _ContentRect.y + 100, 1300, 300);
            // GUILib.Rect(rect, Color.grey, 0.5f);
            GUILib.Rect(rect, Color.white, 1f);
        }
    }
}