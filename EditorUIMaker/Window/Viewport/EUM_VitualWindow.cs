using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_VitualWindow : I_EUM_Drawable
    {
        public EUM_Window Window;
        public const float s_TitleHeight = 15;

        enum E_ResizeType
        {
            Null,
            Left,
            Top,
            Right,
            Bottom,
        }

        private E_ResizeType _ResizeType = E_ResizeType.Null;
        public Vector2 LastDownPosition;

        public EUM_VitualWindow()
        {
            Window = new EUM_Window();
            EUM_Helper.Instance.Widgets.Add(Window.ID, Window);
            EUM_Helper.Instance.Window = Window;
            EUM_Helper.Instance.OnClearData += ClearData;
        }

        void ClearData()
        {
            Window.Widgets.Clear();
        }

        public void Draw(ref Rect rect)
        {
            DrawContent(rect);
        }

        void DrawContent(Rect contentRect)
        {
            if (EUM_Helper.Instance.WindowRect.width == 0)
            {
                EUM_Helper.Instance.WindowRect = new Rect(
                    contentRect.x + (contentRect.width - EUM_Helper.InitWindowWidth) / 2,
                    contentRect.y + (contentRect.height - EUM_Helper.InitWindowHeight) / 2,
                    EUM_Helper.InitWindowWidth, EUM_Helper.InitWindowHeight
                );
            }

            var rect = EUM_Helper.Instance.WindowRect;

            var scale = EUM_Helper.GetZoomScaleFactor();
            var scaleVec = new Vector2(scale,scale);
            GUIUtility.ScaleAroundPivot(scaleVec, EUM_Helper.Instance.MousePosition);
            
            GUILib.Rect(rect, Color.grey, 0.5f);
            EUM_Helper.Instance.VitualWindowRect =
                new Rect(rect.x, rect.y + s_TitleHeight, rect.width, rect.height - s_TitleHeight);

            //title
            var titleBgRect = new Rect(rect.x, rect.y, rect.width, s_TitleHeight);
            GUILib.Rect(titleBgRect, Color.black, 0.3f);

            var titleRect = new Rect(rect.x, rect.y, rect.width, s_TitleHeight);
            GUILib.Label(titleRect, EUM_Helper.Instance.WindowTitle + (EUM_Helper.Instance.Modified ? "*" : ""));

            Window.Draw(ref EUM_Helper.Instance.VitualWindowRect);

            DrawResizeBar(rect);
            GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(1, 1, 1));
        }

        void DrawResizeBar(Rect rect)
        {
            var rightResizeRect = new Rect(rect.xMax, rect.yMin, GUILib.s_SplitSize, rect.height);
            rightResizeRect = GUILib.Padding(rightResizeRect, -2, -2);
            EditorGUIUtility.AddCursorRect(rightResizeRect, MouseCursor.ResizeHorizontal);

            var leftResizeRect = new Rect(rect.xMin, rect.yMin, GUILib.s_SplitSize, rect.height);
            leftResizeRect = GUILib.Padding(leftResizeRect, -2, -2);
            EditorGUIUtility.AddCursorRect(leftResizeRect, MouseCursor.ResizeHorizontal);

            var topResizeRect = new Rect(rect.xMin, rect.yMin, rect.width, GUILib.s_SplitSize);
            topResizeRect = GUILib.Padding(topResizeRect, -2, -2);
            EditorGUIUtility.AddCursorRect(topResizeRect, MouseCursor.ResizeVertical);

            var bottomResizeRect = new Rect(rect.xMin, rect.yMax, rect.width, GUILib.s_SplitSize);
            bottomResizeRect = GUILib.Padding(bottomResizeRect, -2, -2);
            EditorGUIUtility.AddCursorRect(bottomResizeRect, MouseCursor.ResizeVertical);

            if (Event.current.type == EventType.MouseDown)
            {
                LastDownPosition = Event.current.mousePosition;
                if (rightResizeRect.Contains(Event.current.mousePosition))
                {
                    _ResizeType = E_ResizeType.Right;
                }
                else if (leftResizeRect.Contains(Event.current.mousePosition))
                {
                    _ResizeType = E_ResizeType.Left;
                }
                else if (topResizeRect.Contains(Event.current.mousePosition))
                {
                    _ResizeType = E_ResizeType.Top;
                }
                else if (bottomResizeRect.Contains(Event.current.mousePosition))
                {
                    _ResizeType = E_ResizeType.Bottom;
                }

                if (_ResizeType != E_ResizeType.Null)
                    ResizeContent();
            }

            if (_ResizeType != E_ResizeType.Null)
                ResizeContent();

            if (Event.current.rawType == EventType.MouseUp)
                _ResizeType = E_ResizeType.Null;
        }

        void ResizeContent()
        {
            var currentPosition = Event.current.mousePosition;
            var deltaX = currentPosition.x - LastDownPosition.x;
            var deltaY = currentPosition.y - LastDownPosition.y;
            var rect = EUM_Helper.Instance.WindowRect;

            if (_ResizeType == E_ResizeType.Top)
            {
                var canResize = true;
                if (deltaY > 0)
                {
                    //缩小
                    if(rect.height <= EUM_Helper.MinWindowHeight)
                        canResize = false;
                }
                else if (deltaY < 0)
                {
                    //放大
                    if(rect.height <= EUM_Helper.MinWindowHeight &&
                       currentPosition.y >= EUM_Helper.Instance.WindowRect.yMin)
                        canResize = false;
                }

                if (canResize)
                {
                    EUM_Helper.Instance.WindowRect.yMin += deltaY;
                }
            }
            else if (_ResizeType == E_ResizeType.Bottom)
            {
                var canResize = true;
                if (deltaY > 0)
                {
                    //放大
                    if(rect.height <= EUM_Helper.MinWindowHeight &&
                       currentPosition.y <= EUM_Helper.Instance.WindowRect.yMax)
                        canResize = false;
                }
                else if (deltaY < 0)
                {
                    //缩小
                    if(rect.height <= EUM_Helper.MinWindowHeight)
                        canResize = false;
                }

                if (canResize)
                {
                    EUM_Helper.Instance.WindowRect.yMax += deltaY;
                }
            }
            else if (_ResizeType == E_ResizeType.Left)
            {
                var canResize = true;
                if (deltaX > 0)
                {
                    //缩小
                    if(rect.width <= EUM_Helper.MinWindowWidth)
                        canResize = false;
                }
                else if (deltaX < 0)
                {
                    //放大
                    if(rect.width <= EUM_Helper.MinWindowWidth &&
                       currentPosition.x >= EUM_Helper.Instance.WindowRect.xMin)
                        canResize = false;
                }

                if (canResize)
                {
                    EUM_Helper.Instance.WindowRect.xMin += deltaX;
                }
            }
            else if (_ResizeType == E_ResizeType.Right)
            {
                var canResize = true;
                if (deltaX > 0)
                {
                    //放大
                    if(rect.width <= EUM_Helper.MinWindowWidth &&
                       currentPosition.x <= EUM_Helper.Instance.WindowRect.xMax)
                        canResize = false;
                }
                else if (deltaX < 0)
                {
                    //缩小
                    if(rect.width <= EUM_Helper.MinWindowWidth)
                        canResize = false;
                }

                if (canResize)
                    EUM_Helper.Instance.WindowRect.xMax += deltaX;
            }

            LastDownPosition = Event.current.mousePosition;
        }
    }
}