using System;

namespace EditorUIMaker.Widgets
{
    public class EUM_MinMaxIntSlider_Info : EUM_BaseInfo
    {
        public string Label;
        public int Min = 0;
        public int Max = 10;

        [NonSerialized] public int MinValue;
        [NonSerialized] public int MaxValue;
        
        
        public EUM_MinMaxIntSlider_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_MinMaxIntSlider_Info;
            CopyBaseInfo(info);
            
            info.Label = Label;
            info.Min = Min;
            info.Max = Max;
        }
    }
}