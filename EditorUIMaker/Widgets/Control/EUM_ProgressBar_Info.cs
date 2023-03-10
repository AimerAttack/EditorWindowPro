namespace EditorUIMaker.Widgets
{
    public class EUM_ProgressBar_Info : EUM_BaseInfo
    {
        public string Label;
        
        public EUM_ProgressBar_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_ProgressBar_Info;
            CopyBaseInfo(info);
            
            info.Label = Label;
        }
    }
}