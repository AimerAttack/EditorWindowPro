using System;

namespace EditorUIMaker.Widgets
{
    public class EUM_Dropdown_Info : EUM_BaseInfo
    {
        public string[] Options = {"1","2","3" };
        
        [NonSerialized]
        public string Value = "1";
        
        public EUM_Dropdown_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_Dropdown_Info;
            CopyBaseInfo(info);
            
            info.Options = new string[Options.Length];
            Options.CopyTo(info.Options, 0);
        }
    }
}