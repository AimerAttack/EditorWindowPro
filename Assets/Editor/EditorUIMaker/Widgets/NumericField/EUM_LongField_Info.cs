using System;

namespace EditorUIMaker.Widgets
{
    public class EUM_LongField_Info : EUM_BaseInfo
    {
        public string Label;

        [NonSerialized] public long Value;
        
        public EUM_LongField_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_LongField_Info;
            CopyBaseInfo(info);
            
            info.Label = Label;
        }
    }
}