using System;

namespace EditorUIMaker.Widgets
{
    public class EUM_TextField_Info : EUM_BaseInfo
    {
        public string Label;
        
        [NonSerialized]
        public string Value;
        
        public EUM_TextField_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_TextField_Info;
            CopyBaseInfo(info);

            info.Label = Label;
        }
    }
}