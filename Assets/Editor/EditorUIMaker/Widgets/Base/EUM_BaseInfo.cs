
namespace EditorUIMaker.Widgets
{
    public abstract class EUM_BaseInfo
    {
        public string Name;
        private EUM_BaseWidget _Widget;

        public string DisplayName
        {
            get
            {
                return string.IsNullOrEmpty(Name) ? _Widget.TypeName : Name;
            }
        }
        
        public EUM_BaseInfo(EUM_BaseWidget widget)
        {
            _Widget = widget;
        }
        
        public abstract void CopyTo<T>(T target) where T : EUM_BaseInfo;
        
        protected void CopyBaseInfo(EUM_BaseInfo info)
        {
            info.Name = Name;
        }
    }
}