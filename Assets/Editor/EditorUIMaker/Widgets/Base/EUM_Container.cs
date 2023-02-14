using System.Collections.Generic;
using EditorUIMaker.Widgets;
using UnityEngine;

namespace EditorUIMaker
{
    public abstract class EUM_Container : EUM_BaseWidget
    {
        public bool Selected = false;
        public List<EUM_BaseWidget> Widgets = new List<EUM_BaseWidget>();

        public EUM_Container()
        {
            EUM_Helper.Instance.Containers.Add(this);
        }

        protected void DrawItems()
        {
            GUILayout.BeginArea(Rect);
            
            foreach (var widget in Widgets)
            {
                widget.DrawLayout();
            }
            
            GUILayout.EndArea();
        }
    }
}