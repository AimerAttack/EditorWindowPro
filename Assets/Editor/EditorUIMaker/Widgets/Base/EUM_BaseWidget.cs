using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public abstract class EUM_BaseWidget : I_EUM_LayoutDrawable, I_EUM_Depth, I_EUM_Draggable
    {
        public int ID;
        public bool InViewport = false;
        public Rect Rect;
        public Rect AbsoluteRect;
        public EUM_Container Parent;
        public abstract string TypeName { get; }
        public virtual string IconName => string.Empty;

        public EUM_BaseInfo Info;
        protected abstract EUM_BaseInfo CreateInfo();
        
        public EUM_BaseWidget()
        {
            ID = EUM_Helper.Instance.WidgetID++;
            Info = CreateInfo();
            Info.Name = TypeName;
        }

        public virtual bool CanResize()
        {
            return true;
        }

        protected abstract void OnDrawLayout();
        public void DrawLayout()
        {
            OnDrawLayout();
            if (Event.current.type == EventType.Repaint)
            {
                var selfRect = GUILib.GetLastRect();
                if (selfRect.width == 0)
                    selfRect.width = Parent.AbsoluteRect.width;
                var topWindowRect = EUM_Helper.Instance.VitualWindowRect;
                if(ParentHasScrollView())
                    AbsoluteRect = new Rect(Parent.AbsoluteRect.x + selfRect.x,Parent.AbsoluteRect.y + selfRect.y,selfRect.width, selfRect.height);
                else
                {
                    AbsoluteRect = new Rect(topWindowRect.x + selfRect.x, topWindowRect.y + selfRect.y, selfRect.width,
                        selfRect.height);
                }

                Rect = AbsoluteRect;
                FixAbsoluteRect();
            }
        }

        bool ParentHasScrollView()
        {
            var node = Parent;
            while (node != null)
            {
                if (node is EUM_ScrollView)
                    return true;
                node = node.Parent;
            }

            return false;
        }

        public abstract string LogicCode();

        public abstract string Code();

        public virtual string CodeForDefine()
        {
            return string.Empty;
        }

        public virtual string CodeForInit()
        {
            return string.Empty;
        }

        protected virtual void FixAbsoluteRect()
        {
        }
        
        public bool Contains(Vector2 point)
        {
            return AbsoluteRect.Contains(point);
        }

        [SerializeField]
        public int Depth { get; set; }

        public abstract void DrawDraging(Vector2 position);
        
        public abstract EUM_BaseWidget Clone();
        public abstract EUM_BaseWidget SingleClone();

        public void OnAddToContainer(EUM_Container container)
        {
            Parent = container;
            Depth = container.Depth + 1;
            InViewport = true;
        }

        protected GUILayoutOption[] LayoutOptions()
        {
            if (Info.Height > 0)
            {
                var options = new GUILayoutOption[] { GUILayout.MinHeight(Info.Height)};
                return options;
            }
                
            return null;
        }

        protected string LayoutOptionsStr()
        {
            var options = LayoutOptions();
            if (options != null)
            {
                var content = new List<string>();
                for (int i = 0; i < options.Length; i++)
                {
                    var option = options[i];
                    var optionString = OptionToString(option);
                    content.Add(optionString);
                }

                var str = string.Join(",", content);
                return str;
            }
            return "null";
        }

        private string OptionToString(GUILayoutOption option)
        {
            var type = option.GetType();
            var typeField = type.GetField("type",BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var typeVal = typeField.GetValue(option);
            var valuefield = type.GetField("value",BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var optionValue = valuefield.GetValue(option);
            var nestedTypes = type.GetNestedTypes(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            Type enumType = null;
            for (int i = 0; i < nestedTypes.Length; i++)
            {
                if (nestedTypes[i].Name == "Type")
                {
                    enumType = nestedTypes[i];
                    break;
                }
            }

            var values = enumType.GetEnumValues();

            for (int i = 0; i < values.Length; i++)
            {
                var item = values.GetValue(i);
                var str = item.ToString();
                var regionType = str;
                if (typeVal.Equals(item))
                {
                    if (str == "fixedWidth")
                        str = "width";
                    else if (str == "stretchWidth")
                        str = "ExpandWidth";
                    else if (str == "stretchHeight")
                        str = "ExpandHeight";
                    
                    str = char.ToUpper(str[0]) + str.Substring(1);
                    var valStr = OptionValueToString(regionType, optionValue);
                    var result = string.Format("GUILayout.{0}({1})",str,valStr);
                    return result;
                }
            }

            return string.Empty;
        }

        private string OptionValueToString(string type,object obj)
        {
            if (type == "stretchWidth" || type == "stretchHeight")
            {
                var val = (int) obj;
                return val == 1 ? "true" : "false";
            }
            else
            {
                var val = (float) obj;
                return val.ToString();
            }
        }
    }
    
}