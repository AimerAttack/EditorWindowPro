using Amazing.Editor.Library;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Title : I_EUM_Drawable
    {
        private const float s_TitleHeight = 16;

        private GUIContent _Title;

        public EUM_Title(GUIContent title)
        {
            _Title = title;
        }
        
        public void Draw(ref Rect rect)
        {
            var titleRect = new Rect(rect.x, rect.y, rect.width, s_TitleHeight);
            GUILib.Rect(titleRect, Color.black, 0.2f);

            titleRect.xMin += 4f;
            GUI.Label(titleRect, _Title);

            rect.yMin += s_TitleHeight;
        }
    }
}