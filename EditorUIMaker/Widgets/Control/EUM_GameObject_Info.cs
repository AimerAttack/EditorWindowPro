using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EditorUIMaker.Widgets
{
    public class EUM_GameObject_Info : EUM_BaseInfo
    {
        public string Label;
        
        [NonSerialized] public GameObject Value;
        
        public EUM_GameObject_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_GameObject_Info;
            CopyBaseInfo(info);

            info.Label = Label;
        }
    }
}