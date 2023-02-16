using AEditor;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public class EUM_Label_Info : EUM_BaseInfo
    {
        public string Text;
        public TextAnchor TextAnchor = TextAnchor.MiddleCenter;


        public EUM_Label_Info(EUM_BaseWidget widget) : base(widget)
        {
        }

        public override void CopyTo<T>(T target)
        {
            var info = target as EUM_Label_Info;
            CopyBaseInfo(info);
            
            info.Text = Text;
            info.TextAnchor = TextAnchor;
        }
    }
}