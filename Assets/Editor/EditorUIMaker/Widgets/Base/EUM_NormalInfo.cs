namespace EditorUIMaker.Widgets
{
    public class EUM_NormalInfo : EUM_BaseInfo
    {
        public EUM_NormalInfo(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            CopyBaseInfo(target);
        }
    }
}