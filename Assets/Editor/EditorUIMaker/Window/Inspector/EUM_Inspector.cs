using System;
using System.Reflection;
using Amazing.Editor.Library;
using EditorUIMaker.Widgets;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Inspector : I_EUM_Drawable
    {
        public EUM_Title Title;
        public Vector2 ScrollPosition;
        public const float s_PropertyNameWidth = 130;

        public EUM_Inspector()
        {
            Title = new EUM_Title(new GUIContent("Inspector"));
        }

        public void Draw(ref Rect rect)
        {
            GUILib.Rect(rect, GUILib.s_DefaultColor, 1f);

            Title.Draw(ref rect);

            DrawProperty(rect);
        }

        void DrawProperty(Rect rect)
        {
            if (EUM_Helper.Instance.SelectWidget == null)
                return;

            GUILayout.BeginArea(rect);
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);
            var info = EUM_Helper.Instance.SelectWidget.Info;
            var type = info.GetType();

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            Array.Sort(properties, (left, right) =>
            {
                var leftFromBase = left.DeclaringType == typeof(EUM_BaseInfo);
                var rightFromBase = right.DeclaringType == typeof(EUM_BaseInfo);
                if (leftFromBase && rightFromBase)
                    return 0;
                else if (leftFromBase)
                    return -1;
                else if (rightFromBase)
                    return 1;
                else
                    return 0;
            });

            for (int index = 0; index < properties.Length; index++)
            {
                var fieldInfo = properties[index];
                var fieldType = fieldInfo.GetType();
                var value = fieldInfo.GetValue(info);
                GUILayout.BeginHorizontal();
                GUILayout.Label(fieldInfo.Name);
                GUILayout.Label(value.ToString(),GUILayout.ExpandWidth(true)); 
                GUILayout.EndHorizontal();
            }


            FieldInfo[] allFieldInfo = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            Array.Sort(allFieldInfo, (left, right) =>
            {
                var leftFromBase = left.DeclaringType == typeof(EUM_BaseInfo);
                var rightFromBase = right.DeclaringType == typeof(EUM_BaseInfo);
                if (leftFromBase && rightFromBase)
                    return 0;
                else if (leftFromBase)
                    return -1;
                else if (rightFromBase)
                    return 1;
                else
                    return 0;
            });
            for (int index = 0; index < allFieldInfo.Length; index++)
            {
                var i = index;
                var fieldInfo = allFieldInfo[i];
                var fieldType = fieldInfo.FieldType;
                if (fieldType == typeof(string))
                {
                    var value = fieldInfo.GetValue(info) as string;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(fieldInfo.Name);
                    var newValue = GUILayout.TextField(value, GUILayout.ExpandWidth(true));
                    GUILayout.EndHorizontal();
                    if (newValue != value)
                    {
                        EUM_Helper.Instance.Modified = true;
                        fieldInfo.SetValue(info, newValue);
                        if (fieldInfo.Name == "Name")
                            EUM_Helper.Instance.OnItemRename?.Invoke(EUM_Helper.Instance.SelectWidget);
                    }
                }
                else if (fieldType == typeof(int))
                {
                    var value = (int) fieldInfo.GetValue(info);
                    if (GUILib.IntField(ref value, new GUIContent(fieldInfo.Name), GUILayout.ExpandWidth(true)))
                    {
                        EUM_Helper.Instance.Modified = true;
                        fieldInfo.SetValue(info, value);
                    }
                }
                else if (fieldType == typeof(float))
                {
                    var value = (float) fieldInfo.GetValue(info);
                    if (GUILib.FloatField(ref value, new GUIContent(fieldInfo.Name), GUILayout.ExpandWidth(true)))
                    {
                        EUM_Helper.Instance.Modified = true;
                        fieldInfo.SetValue(info, value);
                    }
                }
                else if (fieldType.BaseType == typeof(Enum))
                {
                    var currentValue = fieldInfo.GetValue(info);
                    var currentString = currentValue.ToString();
                    var values = Enum.GetValues(fieldType);
                    var selectValue = EditorGUILayout.EnumPopup(fieldInfo.Name, (Enum) currentValue,
                        GUILayout.ExpandWidth(true));
                    var selectString = selectValue.ToString();
                    if (!selectString.Equals(currentString))
                    {
                        EUM_Helper.Instance.Modified = true;
                        var obj = Enum.Parse(fieldType, selectString);
                        fieldInfo.SetValue(info, obj);
                    }
                }
                else if (fieldType.BaseType == typeof(bool))
                {
                    var value = (bool) fieldInfo.GetValue(info);
                    if(GUILib.Toggle(ref value,new GUIContent(fieldInfo.Name), null,GUILayout.ExpandWidth(true)))
                    {
                        EUM_Helper.Instance.Modified = true;
                        fieldInfo.SetValue(info, value);
                    }
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
    }
}