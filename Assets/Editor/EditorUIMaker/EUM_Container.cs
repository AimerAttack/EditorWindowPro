using System.Collections.Generic;
using EditorUIMaker.Widgets;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Container
    {
        public int Depth = 0;
        public Rect Rect;
        public List<EUM_BaseWidget> Widgets = new List<EUM_BaseWidget>();

        private EUM_ExaminationArea _ExaminationArea;

        public EUM_Container()
        {
            _ExaminationArea = new EUM_ExaminationArea();
        }

        public void DrawItems()
        {
            GUILayout.BeginArea(Rect);
            
            foreach (var widget in Widgets)
            {
                widget.DrawLayout();
            }
            
            GUILayout.EndArea();
        }
        
        public void DrawArea()
        {
            _ExaminationArea.Rect = Rect;
            _ExaminationArea.Draw();
        }
        
        public bool Contains(Vector2 point)
        {
            return Rect.Contains(point);
        }
    }
}