using System;

namespace EditorUIMaker.Widgets
{
    public class EUM_Slider_Info : EUM_BaseInfo
    {
        public string Label;
        public float Min = 0;
        public float Max = 10;

        [NonSerialized]
        public float Value;
        
        public EUM_Slider_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_Slider_Info;
            CopyBaseInfo(info);

            info.Label = Label;
        }
    }
}