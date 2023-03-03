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

            GUILib.Area(rect, () =>
            {
                GUILib.ScrollView(ref ScrollPosition, () =>
                {
                    var info = EUM_Helper.Instance.SelectWidget.Info;
                    var type = info.GetType();

                    GUILib.Label("[" + EUM_Helper.Instance.SelectWidget.TypeName + "]");

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

                        GUILib.HorizontalRect(() =>
                        {
                            GUILib.Label(fieldInfo.Name);
                            GUILib.Label(value.ToString(), GUILayout.ExpandWidth(true));
                        });
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
                        var fieldName = fieldInfo.Name;
                        if (!EUM_Helper.Instance.SelectWidget.CanResize())
                        {
                            if(fieldName == "Height")
                                continue;
                        }
                        
                        if (fieldType == typeof(string))
                        {
                            var value = fieldInfo.GetValue(info) as string;

                            GUILib.HorizontalRect(() =>
                            {
                                GUILib.Label(fieldInfo.Name);

                                if (GUILib.TextField(ref value, GUILayout.ExpandWidth(true)))
                                {
                                    if (fieldInfo.Name == "Name")
                                    {
                                        //检查命名是否合法
                                        if (EUM_Helper.Instance.NameValid(EUM_Helper.Instance.SelectWidget, value))
                                        {
                                            EUM_Helper.Instance.Modified = true;
                                            fieldInfo.SetValue(info, value);
                                            EUM_Helper.Instance.OnItemRename?.Invoke(EUM_Helper.Instance.SelectWidget);
                                        }
                                    }
                                    else
                                    {
                                        EUM_Helper.Instance.Modified = true;
                                        fieldInfo.SetValue(info, value);
                                    }
                                }
                            });
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
                        else if (fieldType == typeof(long))
                        {
                            var value = (long) fieldInfo.GetValue(info);
                            if (GUILib.LongField(ref value, new GUIContent(fieldInfo.Name),
                                    GUILayout.ExpandWidth(true)))
                            {
                                EUM_Helper.Instance.Modified = true;
                                fieldInfo.SetValue(info, value);
                            }
                        }
                        else if (fieldType == typeof(float))
                        {
                            var value = (float) fieldInfo.GetValue(info);
                            if (GUILib.FloatField(ref value, new GUIContent(fieldInfo.Name),
                                    GUILayout.ExpandWidth(true)))
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
                            if (GUILib.EnumPopup(ref currentValue, new GUIContent(fieldInfo.Name),
                                    GUILayout.ExpandWidth(true)))
                            {
                                EUM_Helper.Instance.Modified = true;
                                fieldInfo.SetValue(info, currentValue); 
                            }
                        }
                        else if (fieldType.BaseType == typeof(bool))
                        {
                            var value = (bool) fieldInfo.GetValue(info);
                            if (GUILib.Toggle(ref value, new GUIContent(fieldInfo.Name), null,
                                    GUILayout.ExpandWidth(true)))
                            {
                                EUM_Helper.Instance.Modified = true;
                                fieldInfo.SetValue(info, value);
                            }
                        }
                        else if (fieldType.BaseType == typeof(Type))
                        {
                            var value = (Type) fieldInfo.GetValue(info);
                            
                        }
                    }
                });
            });
        }
    }
}