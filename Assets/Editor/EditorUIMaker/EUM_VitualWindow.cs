using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_VitualWindow : I_EUM_Drawable
    {
        private Rect _ContentRect;
        private EUM_Container _Container;
        
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
            EUM_Helper.Instance.VitualWindowRect = rect;
            GUILib.Rect(rect, Color.grey, 0.5f);
            _Container.Rect = rect;

            //resize bar

        }
    }
}