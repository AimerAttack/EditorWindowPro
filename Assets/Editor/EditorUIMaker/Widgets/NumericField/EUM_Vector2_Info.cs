using System;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Vector2_Info : EUM_BaseInfo
    {
        public string Label;

        [NonSerialized] public Vector2 Value;
        
        public EUM_Vector2_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_Vector2_Info;
            CopyBaseInfo(info);
            
            info.Label = Label;
        }
    }
}