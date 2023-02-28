using System;

namespace EditorUIMaker.Widgets
{
    public class EUM_Toggle_Info : EUM_BaseInfo
    {
        public string Label;

        [NonSerialized] public bool Value;
        
        public EUM_Toggle_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_Toggle_Info;
            CopyBaseInfo(info);
            
            info.Label = Label;
        }
    }
}