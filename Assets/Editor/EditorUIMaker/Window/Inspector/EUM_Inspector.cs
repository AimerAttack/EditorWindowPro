using System;
using System.Reflection;
using Amazing.Editor.Library;
using EditorUIMaker.Widgets;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Inspector : I_EUM_Drawable
    {
        private EUM_Title _Title;
        private Vector2 _ScrollPosition;
        
        public EUM_Inspector()
        {
            _Title = new EUM_Title(new GUIContent("Inspector"));
        }
        
        public void Draw(ref Rect rect)
        {
            GUILib.Rect(rect,GUILib.s_DefaultColor , 1f);
            
            _Title.Draw(ref rect);
            
            DrawProperty(rect);
        }

        void DrawProperty(Rect rect)
        {
            if(EUM_Helper.Instance.SelectWidget == null)
                return;
            
            GUILayout.BeginArea(rect);
            _ScrollPosition = GUILayout.BeginScrollView(_ScrollPosition);
            var info = EUM_Helper.Instance.SelectWidget.Info;
            var type = info.GetType();
            FieldInfo[] allFieldInfo = type.GetFields( BindingFlags.Instance | BindingFlags.Public);
            Array.Sort(allFieldInfo, (left, right) =>
            {
                var leftFromBase = left.DeclaringType == typeof(EUM_BaseInfo);
                var rightFromBase = right.DeclaringType == typeof(EUM_BaseInfo);
                if(leftFromBase && rightFromBase)
                    return 0;
                else if(leftFromBase)
                    return -1;
                else if(rightFromBase)
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
                    GUILayout.Label(fieldInfo.Name,GUILayout.Width(130));
                    var newValue = GUILayout.TextField(value,GUILayout.ExpandWidth(true));
                    GUILayout.EndHorizontal();
                    if (newValue != value)
                    {
                        fieldInfo.SetValue(info, newValue);
                        EUM_Helper.Instance.OnItemRename?.Invoke(EUM_Helper.Instance.SelectWidget);
                    }
                }
                else if (fieldType == typeof(int))
                {
                    var value = (int) fieldInfo.GetValue(info);
                    if(GUILib.IntField(ref value,new GUIContent(fieldInfo.Name),GUILayout.ExpandWidth(true)))
                    {
                        fieldInfo.SetValue(info, value);
                    }
                }
                //wtodo
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
    }
}