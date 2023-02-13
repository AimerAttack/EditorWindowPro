using System.Collections.Generic;
using EditorUIMaker.Widgets;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Container : I_EUM_Depth
    {
        public bool Selected = false;
        public Rect Rect;
        public List<EUM_BaseWidget> Widgets = new List<EUM_BaseWidget>();

        private EUM_ExaminationArea _ExaminationArea;
        public EUM_ExaminationArea ExaminationArea => _ExaminationArea;

        public EUM_Container()
        {
            _ExaminationArea = new EUM_ExaminationArea(this);
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
            _ExaminationArea.Draw(Selected);
        }
        
        public bool Contains(Vector2 point)
        {
            return Rect.Contains(point);
        }

        public int Depth { get; set; }
    }
}