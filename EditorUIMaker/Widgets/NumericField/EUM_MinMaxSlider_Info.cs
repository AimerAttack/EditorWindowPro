using System;

namespace EditorUIMaker.Widgets
{
    public class EUM_MinMaxSlider_Info : EUM_BaseInfo
    {
        public string Label;
        public float Min = 0;
        public float Max = 10;

        [NonSerialized] public float MinValue;
        [NonSerialized] public float MaxValue;
        
        public EUM_MinMaxSlider_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_MinMaxSlider_Info;
            CopyBaseInfo(info);
            
            info.Label = Label;
            info.Min = Min;
            info.Max = Max;
        }
    }
}