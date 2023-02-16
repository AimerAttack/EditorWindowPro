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

        protected virtual void DrawItems()
        {
            if (!EUM_Helper.Instance.Preview && InViewport)
            {
                GUI.enabled = false;
                GUI.color = new Color(1, 1, 1, 2);
            }
            
            foreach (var widget in Widgets)
            {
                widget.DrawLayout();
            }
            
            if (!EUM_Helper.Instance.Preview && InViewport)
            {
                GUI.color = Color.white;
                GUI.enabled = true;
            }
        }
    }
}