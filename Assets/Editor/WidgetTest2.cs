using System;
using System.Collections.Generic;
using System.Linq;
using Amazing.Editor.Library;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Editor
{
    /*
     * flow layout
     * 如果只有一行，则不用计算宽度，直接都走layout布局
     * 如果不只一行，则需要计算宽度，每行分配特定个数的子元素
     */

    public class WidgetTest2 : EditorWindow
    {
        [MenuItem("Tools/WidgetTest2")]
        static void OpenWindow()
        {
            var window = GetWindow<WidgetTest2>();
            window.Show();
        }

        private bool Expanded = false;

        private bool b_A;

        private List<string> items = new List<string>()
        {
            "a", "b", "c", "d",
            "a", "b", "c", "d"
        };

        private List<bool> itemVals = new List<bool>()
        {
            false,
            false,
            false,
            false,
        };

        private string lastItem;
        private Rect buttonRect;

        private const float MinDuration = 0.2f;
        private double lastClickTime = 0;

        void OnClickItem(string item)
        {
            Debug.Log("click " + item);
        }

        private bool changed = false;
        private int lineCount;
        private int perLineItemCount;

        void OnGUI()
        {
            if (Event.current.type == EventType.Layout)
            {
                lineCount = Random.Range(1, 4);
                perLineItemCount = Random.Range(3, 9);
            }

            for (int i = 0; i < lineCount; i++)
            {
                for (int j = 0; j < perLineItemCount; j++)
                {
                    GUILayout.Label(i + "_" + j);
                }
            }
            
            Repaint();
            
            // if (changed && Event.current.type == EventType.Repaint)
            // {
            //     Repaint();
            //     changed = false;
            //
            //     for (int i = 0; i < lineCount; i++)
            //     {
            //         for (int j = 0; j < perLineItemCount; j++)
            //         {
            //             GUILayout.Label(i + "_" + j);
            //         }
            //     }
            // }
            // else
            // {
            //     lineCount = Random.Range(1, 4);
            //     perLineItemCount = Random.Range(3, 9);
            //
            //     for (int i = 0; i < lineCount; i++)
            //     {
            //         for (int j = 0; j < perLineItemCount; j++)
            //         {
            //             GUILayout.Label(i + "_" + j);
            //         }
            //     }
            //
            //     changed = true;
            // }
        }

        private void OnGUI2()
        {
            var style = new GUIStyle(GUI.skin.button);
            style.wordWrap = true;
            style.alignment = TextAnchor.LowerCenter;
            style.fontSize = 11;


            var width = position.width;
            var itemWidth = 80;
            var itemCount = items.Count;
            var perLineItemCount = Mathf.FloorToInt(width / itemWidth);
            perLineItemCount = Mathf.Max(perLineItemCount, 1);
            var lineCount = Mathf.CeilToInt(itemCount * 1f / perLineItemCount);
            if (lineCount == 1)
            {
                GUILayout.BeginHorizontal();
                for (int i = 0; i < items.Count; i++)
                {
                    GUILayout.Button(items[i], GUILayout.Width(itemWidth), GUILayout.Height(80));
                }

                GUILayout.EndHorizontal();
            }
            else if (lineCount > 1)
            {
                var space = (width - perLineItemCount * itemWidth) / (perLineItemCount + 1);
                for (int lineIndex = 0; lineIndex < lineCount; lineIndex++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(space);
                    for (int i = 0; i < perLineItemCount; i++)
                    {
                        var itemIndex = lineIndex * perLineItemCount + i;
                        if (itemIndex >= items.Count)
                            continue;
                        var item = items[itemIndex];
                        GUILayout.Button(item, GUILayout.Width(itemWidth), GUILayout.Height(80));
                        GUILayout.Space(space);
                    }

                    GUILayout.EndHorizontal();
                }
            }

            Repaint();
        }
    }
}