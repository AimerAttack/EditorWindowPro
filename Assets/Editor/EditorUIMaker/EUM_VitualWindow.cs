using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_VitualWindow : I_EUM_Drawable
    {
        private Rect _ContentRect;
        private EUM_Container _Container;
        private const float s_TitleHeight = 15;
        
        public EUM_VitualWindow()
        {
            _Container = new EUM_Container();
            EUM_Helper.Instance.Containers.Add(_Container);
        }
        
        public void Draw(ref Rect rect)
        {
            _ContentRect = rect;
            DrawContent();
        }

        void DrawContent()
        {
            var rect = new Rect(_ContentRect.x + 200, _ContentRect.y + 100, 400, 300);
            GUILib.Rect(rect, Color.grey, 0.5f);
            EUM_Helper.Instance.VitualWindowRect = new Rect(rect.x,rect.y + s_TitleHeight,rect.width,rect.height - s_TitleHeight);
            
            //title
            var titleBgRect = new Rect(rect.x, rect.y, rect.width, s_TitleHeight);
            GUILib.Rect(titleBgRect, Color.black, 0.3f);

            var titleRect = new Rect(rect.x, rect.y, rect.width, s_TitleHeight);
            GUI.Label(titleRect,EUM_Helper.Instance.WindowTitle);
            
            _Container.Rect = EUM_Helper.Instance.VitualWindowRect;

            //resize bar

        }
    }
}