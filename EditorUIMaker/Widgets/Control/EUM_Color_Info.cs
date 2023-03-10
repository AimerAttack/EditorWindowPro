using System;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Color_Info : EUM_BaseInfo
    {
        public string Label;

        [NonSerialized] public Color Value = Color.white;
        
        public EUM_Color_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_Color_Info;
            CopyBaseInfo(info);
            
            info.Label = Label;
        }
    }
}