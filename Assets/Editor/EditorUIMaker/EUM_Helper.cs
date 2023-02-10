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
        public EUM_Container HoverContainer;
        public List<EUM_Container> Containers = new List<EUM_Container>();
        public bool Preview;
        public string WindowTitle = "WindowTitle";
        public float Alpha = 0;

        private float _FadeTime = 0.2f;
        private float _StartFadeTime;

        public void ResetFade()
        {
            Alpha = 0;
            _StartFadeTime = Time.realtimeSinceStartup;
        }
        
        public void Fade()
        {
            var deltaTime = Time.realtimeSinceStartup - _StartFadeTime;
            var percent = deltaTime/ _FadeTime;
            percent = Mathf.Clamp01(percent);
            Alpha = Mathf.Lerp(0, 1, percent);
        }
    }
}