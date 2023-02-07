using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Amazing.Editor.Library
{
    public class ASplitView
    {
        private const float s_SplitSize = 2f;
        private const float s_TitleHeight = 16;

        private IEditorWindow window;
        private bool dirty;

        public ASplitView(IEditorWindow w)
        {
            this.window = w;
        }

        [Serializable]
        public class Info
        {
            public GUIContent title;


            public Rect rect;
            public float normWeight;
            public int stIndex;

            public bool visible = true;
            public float weight = 1f;
            public Action<Rect> draw;

            public void DoDraw()
            {
                var drawRect = rect;
                if (title != null)
                {
                    var titleRect = new Rect(rect.x, rect.y, rect.width, s_TitleHeight);
                    GUILib.Rect(titleRect, Color.black, 0.2f);

                    titleRect.xMin += 4f;
                    GUI.Label(titleRect, title, EditorStyles.boldLabel);
                    drawRect.yMin += s_TitleHeight;
                }


                GUILayout.BeginArea(drawRect);
                draw(drawRect);
                GUILayout.EndArea();
            }
        }

        public bool isHorz;
        public List<Info> splits = new List<Info>();

        public bool isVisible
        {
            get { return _visibleCount > 0; }
        }

        private int _visibleCount;
        private Rect _rect;

        public void CalculateWeight()
        {
            _visibleCount = 0;
            var _totalWeight = 0f;

            for (var i = 0; i < splits.Count; i++)
            {
                var info = splits[i];
                if (!info.visible) continue;

                info.stIndex = _visibleCount;
                _totalWeight += info.weight;

                _visibleCount++;
            }

            if (_visibleCount == 0 || _totalWeight == 0)
            {
                //Debug.LogWarning("Nothing visible!");
                return;
            }

            var cWeight = 0f;
            for (var i = 0; i < splits.Count; i++)
            {
                var info = splits[i];
                if (!info.visible) continue;

                cWeight += info.weight;
                info.normWeight = info.weight / _totalWeight;
            }
        }

        public void DoDraw(Rect rect)
        {
            if (rect.width > 0 || rect.height > 0)
            {
                _rect = rect;
            }

            if (dirty)
            {
                dirty = false;
                CalculateWeight();
            }

            var sz = (_visibleCount - 1) * s_SplitSize;
            var dx = _rect.x;
            var dy = _rect.y;

            for (var i = 0; i < splits.Count; i++)
            {
                var info = splits[i];
                if (!info.visible) continue;

                var rr = new Rect
                (
                    dx, dy,
                    isHorz ? (_rect.width - sz) * info.normWeight : _rect.width,
                    isHorz ? _rect.height : (_rect.height - sz) * info.normWeight
                );

                if (rr.width > 0 && rr.height > 0)
                {
                    info.rect = rr;
                }

                if (info.draw != null) info.DoDraw();

                if (info.stIndex < _visibleCount - 1)
                {
                    DrawSpliter(i, isHorz ? info.rect.xMax : info.rect.yMax);
                }

                if (isHorz)
                {
                    dx += info.rect.width + s_SplitSize;
                }
                else
                {
                    dy += info.rect.height + s_SplitSize;
                }
            }
        }

        public void Draw()
        {
            var rect = StartLayout(isHorz);
            {
                DoDraw(rect);
            }
            EndLayout(isHorz);
        }

        private int resizeIndex = -1;


        void RefreshSpliterPos(int index, float px)
        {
            var sp1 = splits[index];
            var sp2 = splits[index + 1];

            var r1 = sp1.rect;
            var r2 = sp2.rect;

            var w1 = sp1.weight;
            var w2 = sp2.weight;
            var tt = w1 + w2;

            var dd = isHorz ? (r2.xMax - r1.xMin) : (r2.yMax - r1.yMin) - s_SplitSize;
            var m = isHorz
                ? (Event.current.mousePosition.x - r1.x)
                : Mathf.Max(s_TitleHeight, Event.current.mousePosition.y - r1.y);
            var pct = Mathf.Min(0.9f, Mathf.Max(0.1f, m / dd));

            sp1.weight = tt * pct;
            sp2.weight = tt * (1 - pct);

            dirty = true;
            if (window != null) window.NeedRepaint = true;
        }


        void DrawSpliter(int index, float px)
        {
            var dRect = _rect;

            if (isHorz)
            {
                dRect.x = px;
                dRect.width = s_SplitSize;
            }
            else
            {
                dRect.y = px;
                dRect.height = s_SplitSize;
            }

            // if (Event.current.type == EventType.Repaint || Event.current.type == EventType.MouseMove)
            // {
                GUILib.Rect(dRect, Color.black, 0.4f);
            // }

            var dRect2 = GUILib.Padding(dRect, -2f, -2f);

            EditorGUIUtility.AddCursorRect(dRect2, isHorz ? MouseCursor.ResizeHorizontal : MouseCursor.ResizeVertical);
            if (Event.current.type == EventType.MouseDown && dRect2.Contains(Event.current.mousePosition))
            {
                resizeIndex = index;
                RefreshSpliterPos(index, px);
            }

            if (resizeIndex == index)
            {
                RefreshSpliterPos(index, px);
            }

            if (Event.current.type == EventType.MouseUp)
            {
                resizeIndex = -1;
            }
        }


        Rect StartLayout(bool horz)
        {
            Rect rect;
            if (horz)
            {
                rect = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            }
            else
            {
                rect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            }

            return rect;
        }

        void EndLayout(bool horz)
        {
            if (horz)
            {
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.EndVertical();
            }
        }
    }
}