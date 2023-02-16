namespace EditorUIMaker.Widgets
{
    public class EUM_Space_Info : EUM_BaseInfo
    {
        public float Height = 5;


        public EUM_Space_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_Space_Info;
            CopyBaseInfo(info);
            info.Height = Height;
        }
    }
}