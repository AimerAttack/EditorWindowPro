using UnityEditor;

namespace EditorUIMaker.Widgets
{
    public class EUM_HelpBox_Info : EUM_BaseInfo
    {
        public string Label;
        public MessageType MessageType;
        
        public EUM_HelpBox_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_HelpBox_Info;
            CopyBaseInfo(info);
            
            info.Label = Label;
            info.MessageType = MessageType;
        }
    }
}