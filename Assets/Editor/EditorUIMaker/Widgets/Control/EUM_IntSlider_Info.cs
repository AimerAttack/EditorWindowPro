using System;

namespace EditorUIMaker.Widgets
{
    public class EUM_IntSlider_Info : EUM_BaseInfo
    {
        public string Label;
        public int Min = 0;
        public int Max = 10;

        [NonSerialized]
        public int Value;
        
        public EUM_IntSlider_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_IntSlider_Info;
            CopyBaseInfo(info);
            
            info.Label = Label;
        }
    }
}