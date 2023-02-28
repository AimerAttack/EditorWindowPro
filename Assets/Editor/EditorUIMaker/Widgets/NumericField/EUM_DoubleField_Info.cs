using System;

namespace EditorUIMaker.Widgets
{
    public class EUM_DoubleField_Info : EUM_BaseInfo
    {
        public string Label;

        [NonSerialized] public double Value;
        
        public EUM_DoubleField_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_DoubleField_Info;
            CopyBaseInfo(info);

            info.Label = Label;   
        }
    }
}