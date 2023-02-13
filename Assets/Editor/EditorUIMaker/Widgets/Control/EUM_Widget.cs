using Amazing.Editor.Library;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public abstract class EUM_Widget : I_EUM_LayoutDrawable,I_EUM_Draggable,I_EUM_Depth
    {
        private EUM_ExaminationArea _ExaminationArea;
        
        public EUM_Widget()
        {
            _ExaminationArea = new EUM_ExaminationArea(this);
        }
        
        public abstract string TypeName { get; }
        public abstract void DrawLayout();
        public abstract EUM_Widget Clone();
        public abstract void DrawDraging(Vector2 position);

        public void OnAddToContainer(EUM_Container container)
        {
            Depth = container.Depth + 1;
            _ExaminationArea.Enable = true;
        }

        public int Depth { get; set; }
    }
}