namespace EditorUIMaker.Widgets
{
    public class EUM_TreeView_Info : EUM_BaseInfo
    {
        public string Label;
        public bool ExpendAll;
        public float MinHeight;
    
        public EUM_TreeView_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_TreeView_Info;
            CopyBaseInfo(info);
            
            info.Label = Label;
            info.ExpendAll = ExpendAll;
            info.MinHeight = MinHeight;
        }
    }
}