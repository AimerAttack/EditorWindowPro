using Amazing.Editor.Library;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_OperationArea : I_EUM_Drawable
    {
        public EUM_Hierarchy Hierarchy;
        public EUM_Library Library;

        public float _SplitRatio = 0.5f;
        public bool _Spliting = false;
        public const float s_MinRatio = 0.1f;
        public const float s_MaxRatio = 0.8f;
        
        public Rect _HierarchyRect;
        public Rect _WindowRect;
        
        public EUM_OperationArea()
        {
            Hierarchy = new EUM_Hierarchy();
            Library = new EUM_Library();
        }
        
        public void Draw(ref Rect rect)
        {
            _WindowRect = rect;
            GUILib.Rect(rect, GUILib.s_DefaultColor, 1f); 
            
            _HierarchyRect = new Rect(rect.x, 0, rect.width, rect.height * _SplitRatio);
            var toolboxRect = new Rect(rect.x, _HierarchyRect.yMax, rect.width , rect.height - _HierarchyRect.height);
            
            Hierarchy.Draw(ref _HierarchyRect);
            Library.Draw(ref toolboxRect);
            
            DrawSpliter();
        }

        void DrawSpliter()
        {
            var inspectorSplitRect = new Rect(_HierarchyRect.x, _HierarchyRect.yMax, _HierarchyRect.width, 2f);
            GUILib.Rect(inspectorSplitRect, Color.black, 0.4f);
            var splitCursorRect = GUILib.Padding(inspectorSplitRect, -2f, -2f);
            EditorGUIUtility.AddCursorRect(splitCursorRect, MouseCursor.ResizeVertical);
            EUM_Helper.Instance.MouseRects.Add(inspectorSplitRect);

            if (Event.current.type == EventType.MouseDown &&
                splitCursorRect.Contains(Event.current.mousePosition))
            {
                _Spliting = true;
                RefreshSplitor();
            }

            if (_Spliting)
            {
                RefreshSplitor();
            }

            if (Event.current.rawType == EventType.MouseUp)
            {
                if(_Spliting)
                    _Spliting = false;
            }
        }

        void RefreshSplitor()
        {
            var delta = Event.current.mousePosition.y;
            var ratio = delta / _WindowRect.height;
            ratio = Mathf.Clamp(ratio, s_MinRatio, s_MaxRatio);
            _SplitRatio = ratio;
        }
    }
}