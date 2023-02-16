namespace EditorUIMaker.Widgets
{
    public class EUM_Button_Info : EUM_BaseInfo
    {
        public string Text;

        public EUM_Button_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_Button_Info;
            CopyBaseInfo(info);
            info.Text = Text;
        }
    }
}