using System;
using EditorUIMaker.Widgets;

namespace EditorUIMaker
{
    public class EUM_Foldout_Info : EUM_BaseInfo
    {
        public string Label;

        [NonSerialized] public bool IsOpen;
        
        public EUM_Foldout_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_Foldout_Info;
            CopyBaseInfo(info);
            
            info.Label = Label;
        }
    }
}