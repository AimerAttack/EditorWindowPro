using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Amazing.Editor.Library
{
    public static class GUILib
    {
        public static readonly Color s_DefaultColor = new Color(56f/255, 56f/255, 56f/255, 1f);
        public const float s_DefaultLineHeight = 1f;

        internal static Dictionary<string, GUIContent> tooltipCache = new Dictionary<string, GUIContent>();

        public static void Frame(Rect rect, Color color,float size=1, float alpha = 1)
        {
            var oldColor = GUI.color;
            color.a = alpha;

            GUI.color = color;
            
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, size), (Texture) EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(rect.x, rect.yMax - size, rect.width, size), (Texture) EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(rect.x, rect.y + 1f, size, rect.height - 2f * size), (Texture) EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(rect.xMax - size, rect.y + 1f, size, rect.height - 2f * size), (Texture) EditorGUIUtility.whiteTexture);

            GUI.color = oldColor;
        }
        
        public static void Rect(Rect r, Color c, float alpha = 1)
        {
            var cColor = GUI.color;
            c.a = alpha;

            GUI.color = c;
            GUI.DrawTexture(r, Texture2D.whiteTexture);
            GUI.color = cColor;
        }

        internal static GUIContent GetTooltip(string tooltip)
        {
            if (string.IsNullOrEmpty(tooltip)) return GUIContent.none;

            GUIContent result;
            if (tooltipCache.TryGetValue(tooltip, out result)) return result;
            result = new GUIContent(string.Empty, tooltip);
            tooltipCache.Add(tooltip, result);
            return result;
        }

        public static Rect Padding(Rect r, float x, float y)
        {
            return new Rect(r.x + x, r.y + y, r.width - 2 * x, r.height - 2 * y);
        }

        internal static bool ToolbarToggle(ref bool value, Texture icon, Vector2 padding, string tooltip = null)
        {
            var vv = GUILayout.Toggle(value, GetTooltip(tooltip), EditorStyles.toolbarButton, GUILayout.Width(22f));

            if (icon != null)
            {
                var rect = GUILayoutUtility.GetLastRect();
                rect = Padding(rect, padding.x, padding.y);
                GUI.DrawTexture(rect, icon, ScaleMode.ScaleToFit);
            }

            if (vv == value) return false;
            value = vv;
            return true;
        }

        public static bool Toggle(ref bool value, GUIContent tex, GUIStyle style = null,
            params GUILayoutOption[] options)
        {
            bool vv = false;
            if (style == null)
                vv = GUILayout.Toggle(value, tex, options);
            else
                vv = GUILayout.Toggle(value, tex, style, options);
            if (vv == value) return false;
            value = vv;
            return true;
        }

        public static void HelpBox(string text, MessageType type)
        {
            EditorGUILayout.HelpBox(text, type);
        }

        public static void Line(float margin = 0)
        {
            GUILayout.Space(margin);
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, s_DefaultLineHeight), s_DefaultColor);
            GUILayout.Space(margin);
        }

        public static void Icon(GUIContent icon, float width, float height)
        {
            GUILayout.Label(icon, GUILayout.Width(width), GUILayout.Height(height));
        }

        public static void HorizontalRect(Action drawContent)
        {
            GUILayout.BeginHorizontal();
            drawContent?.Invoke();
            GUILayout.EndHorizontal();
        }

        public static void VerticelRect(Action drawContent)
        {
            GUILayout.BeginVertical();
            drawContent?.Invoke();
            GUILayout.EndVertical();
        }

        public static void FlexableHorizontalRect(Action drawContent)
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            drawContent?.Invoke();
            GUILayout.EndHorizontal();
        }

        public static void FlexableVerticelRect(Action drawContent)
        {
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            drawContent?.Invoke();
            GUILayout.EndVertical();
        }

        public static bool Popup(ref string val, string[] contents, GUIContent icon,
            params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            var cRect = GUILayoutUtility.GetRect(16f, 16f);
            cRect.xMin -= 2f;
            cRect.yMin += 2f;
            GUI.Label(cRect, icon);

            var select = Array.IndexOf(contents, val);
            if (select == -1) select = 0;
            var vv = EditorGUILayout.Popup(select, contents, EditorStyles.toolbarPopup, options);
            if (Equals(vv, select))
            {
                GUILayout.EndHorizontal();
                return false;
            }

            val = contents[vv];
            GUILayout.EndHorizontal();
            return true;
        }

        public static bool EnumPopup<T>(ref T mode, GUIContent icon, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            var obj = (Enum) (object) mode;
            var cRect = GUILayoutUtility.GetRect(16f, 16f);
            cRect.xMin -= 2f;
            cRect.yMin += 2f;
            GUI.Label(cRect, icon);

            var vv = EditorGUILayout.EnumPopup(obj, EditorStyles.toolbarPopup, options);
            if (Equals(vv, obj))
            {
                GUILayout.EndHorizontal();
                return false;
            }

            mode = (T) (object) vv;
            GUILayout.EndHorizontal();
            return true;
        }

        public static bool IntSlider(string label, ref int val, int min, int max, params GUILayoutOption[] options)
        {
            var tmp = EditorGUILayout.IntSlider(label, val, min, max, options);
            if (Equals(tmp, val))
                return false;
            val = tmp;
            return true;
        }

        public static bool Slider(string label, ref float val, float min, float max, params GUILayoutOption[] options)
        {
            var tmp = EditorGUILayout.Slider(label, val, min, max, options);
            if (Equals(tmp, val))
                return false;
            val = tmp;
            return true;
        }

        public static bool MinMaxSlider(string label, ref float minVal, ref float maxVal, float min, float max,
            bool readOnly = false,
            params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            var tmpMin = minVal;
            var tmpMax = maxVal;
            if (readOnly)
            {
                GUILayout.Label(minVal.ToString(), GUILayout.Width(50));
                GUILayout.Label(maxVal.ToString(), GUILayout.Width(50));
            }
            else
            {
                var tmpMinStr = GUILayout.TextField(minVal.ToString(), GUILayout.Width(50));
                if (float.TryParse(tmpMinStr, out var tmpMinVal))
                {
                    tmpMin = tmpMinVal;
                }
                else
                {
                    tmpMin = min;
                }

                tmpMin = Mathf.Clamp(tmpMin, min, max);

                var tmpMaxStr = GUILayout.TextField(maxVal.ToString(), GUILayout.Width(50));
                if (float.TryParse(tmpMaxStr, out var tmpMaxVal))
                {
                    tmpMax = tmpMaxVal;
                }
                else
                {
                    tmpMax = max;
                }

                tmpMax = Mathf.Clamp(tmpMax, tmpMin, max);
            }

            EditorGUILayout.MinMaxSlider(ref tmpMin, ref tmpMax, min, max, options);

            GUILayout.EndHorizontal();

            if (Equals(tmpMin, minVal) && Equals(tmpMax, maxVal))
                return false;

            minVal = tmpMin;
            maxVal = tmpMax;
            return true;
        }

        public static bool Button(string text)
        {
            return GUILayout.Button(text);
        }

        public static bool Color(string label, ref Color color)
        {
            var tmpColor = EditorGUILayout.ColorField(label, color);
            if (Equals(tmpColor, color))
                return false;
            color = tmpColor;
            return true;
        }

        public static void Label(GUIContent label, params GUILayoutOption[] options)
        {
            GUILayout.Label(label, options);
        }

        public static bool SearchBar(ref string _SearchString, params GUILayoutOption[] options)
        {
            var tmpStr = GUILayout.TextField(_SearchString, GUI.skin.FindStyle("ToolbarSeachTextField"), options);
            if (Equals(tmpStr, _SearchString))
                return false;
            _SearchString = tmpStr;
            return true;
        }

        public static bool ObjectField<T>(string label, ref T obj, bool allowSceneObjects = false) where T : Object
        {
            var tmpObj = EditorGUILayout.ObjectField(label, obj, typeof(T), allowSceneObjects);
            if (Equals(tmpObj, obj))
                return false;
            obj = tmpObj as T;
            return true;
        }

        public static bool IntMinMaxSlider(string label, ref int minVal, ref int maxVal, int min, int max,
            bool readOnly = false,
            params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            float tmpMin = minVal;
            float tmpMax = maxVal;
            if (readOnly)
            {
                GUILayout.Label(minVal.ToString(), GUILayout.Width(50));
                GUILayout.Label(maxVal.ToString(), GUILayout.Width(50));
            }
            else
            {
                var tmpMinStr = GUILayout.TextField(minVal.ToString(), GUILayout.Width(50));
                if (float.TryParse(tmpMinStr, out var tmpMinVal))
                {
                    tmpMin = tmpMinVal;
                }
                else
                {
                    tmpMin = min;
                }

                tmpMin = Mathf.Clamp(tmpMin, min, max);

                var tmpMaxStr = GUILayout.TextField(maxVal.ToString(), GUILayout.Width(50));
                if (float.TryParse(tmpMaxStr, out var tmpMaxVal))
                {
                    tmpMax = tmpMaxVal;
                }
                else
                {
                    tmpMax = max;
                }

                tmpMax = Mathf.Clamp(tmpMax, tmpMin, max);
            }

            EditorGUILayout.MinMaxSlider(ref tmpMin, ref tmpMax, min, max, options);

            GUILayout.EndHorizontal();

            if (Equals(Mathf.FloorToInt(tmpMin), minVal) && Equals(Mathf.FloorToInt(tmpMax), maxVal))
                return false;

            minVal = Mathf.FloorToInt(tmpMin);
            maxVal = Mathf.FloorToInt(tmpMax);
            return true;
        }

        public static void ScrollView(ref Vector2 scrollPosition, Action drawContent)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            drawContent?.Invoke();
            GUILayout.EndScrollView();
        }

        public static void Foldout(string label, ref bool foldout,GUIStyle style=null)
        {
            if(style == null)
                foldout = EditorGUILayout.Foldout(foldout, label);
            else
            {
                foldout = EditorGUILayout.Foldout(foldout, label, style);
            }
        }
    }
}