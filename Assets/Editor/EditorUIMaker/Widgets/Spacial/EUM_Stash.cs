using EditorUIMaker.Widgets;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Stash : EUM_Container
    {
        public override string TypeName { get; }
        protected override EUM_BaseInfo CreateInfo()
        {
            return new EUM_NormalInfo(this);
        }

        protected override void OnDrawLayout()
        {
            
        }

        public override void DrawDraging(Vector2 position)
        {
            
        }

        public override EUM_BaseWidget Clone()
        {
            var widget = new EUM_Stash();
            Info.CopyTo(widget.Info);
            return widget;
        }

    }
}