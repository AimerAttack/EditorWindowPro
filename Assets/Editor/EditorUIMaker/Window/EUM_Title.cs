using Amazing.Editor.Library;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Title : I_EUM_Drawable
    {
        public const float s_TitleHeight = 16;

        public GUIContent Title;

        public EUM_Title(GUIContent title)
        {
            Title = title;
        }
        
        public void Draw(ref Rect rect)
        {
            var titleRect = new Rect(rect.x, rect.y, rect.width, s_TitleHeight);
            GUILib.Rect(titleRect, Color.black, 1f);

            titleRect.xMin += 4f;
            GUILib.Label(titleRect, Title);

            rect.yMin += s_TitleHeight;
        }
    }
}