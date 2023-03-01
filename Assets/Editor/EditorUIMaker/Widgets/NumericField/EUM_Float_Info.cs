using System;

namespace EditorUIMaker.Widgets
{
    public class EUM_Float_Info : EUM_BaseInfo
    {
        public string Label;

        [NonSerialized] public float Value;
        
        public EUM_Float_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_Float_Info;
            CopyBaseInfo(info);
            
            info.Label = Label;
        }
    }
}