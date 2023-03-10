using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_OperationArea : I_EUM_Drawable
    {
        public EUM_Hierarchy Hierarchy;
        public EUM_Library Library;

        public float SplitRatio = 0.5f;
        public bool Spliting = false;
        public const float s_MinRatio = 0.1f;
        public const float s_MaxRatio = 0.8f;
        
        public Rect HierarchyRect;
        public Rect WindowRect;
        
        public EUM_OperationArea()
        {
            Hierarchy = new EUM_Hierarchy();
            Library = new EUM_Library();
        }
        
        public void Draw(ref Rect rect)
        {
            WindowRect = rect;
            GUILib.Rect(rect, GUILib.s_DefaultColor, 1f); 
            
            HierarchyRect = new Rect(rect.x, 0, rect.width, rect.height * SplitRatio);
            var toolboxRect = new Rect(rect.x, HierarchyRect.yMax, rect.width , rect.height - HierarchyRect.height);
            
            Hierarchy.Draw(ref HierarchyRect);
            Library.Draw(ref toolboxRect);
            
            DrawSpliter();
        }

        void DrawSpliter()
        {
            var inspectorSplitRect = new Rect(HierarchyRect.x, HierarchyRect.yMax, HierarchyRect.width, 2f);
            GUILib.Rect(inspectorSplitRect, Color.black, 0.4f);
            var splitCursorRect = GUILib.Padding(inspectorSplitRect, -2f, -2f);
            EditorGUIUtility.AddCursorRect(splitCursorRect, MouseCursor.ResizeVertical);
            EUM_Helper.Instance.MouseRects.Add(inspectorSplitRect);

            if (Event.current.type == EventType.MouseDown &&
                splitCursorRect.Contains(Event.current.mousePosition))
            {
                Spliting = true;
                RefreshSplitor();
            }

            if (Spliting)
            {
                RefreshSplitor();
            }

            if (Event.current.rawType == EventType.MouseUp)
            {
                if(Spliting)
                    Spliting = false;
            }
        }

        void RefreshSplitor()
        {
            var delta = Event.current.mousePosition.y;
            var ratio = delta / WindowRect.height;
            ratio = Mathf.Clamp(ratio, s_MinRatio, s_MaxRatio);
            SplitRatio = ratio;
        }
    }
}