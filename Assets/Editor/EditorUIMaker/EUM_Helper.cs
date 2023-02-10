using System.Collections.Generic;
using EditorUIMaker.Widgets;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Helper
    {
        public static EUM_Helper Instance;
        
        public Rect VitualWindowRect;
        public Rect ViewportRect;
        public EUM_BaseWidget DraggingWidget;
        public EUM_Container DraggingOverContainer;
        public List<EUM_Container> Containers = new List<EUM_Container>();
        public bool Preview;
    }
}