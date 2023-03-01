using System;
using System.Collections.Generic;
using EditorUIMaker;
using UnityEditor;
using UnityEngine;

namespace Amazing.Editor.Library
{
    public class ATabList
    {
        public Action OnTabChange;
        public int Current;
        
        private IEditorWindow _Window;
        private bool _CanDeselectAll;
        private GUIContent[] contents;

        public ATabList(IEditorWindow window,bool canDeselectAll)
        {
            Current = 0;
            _Window = window;
            _CanDeselectAll = canDeselectAll;
        }
        
        public void Draw()
        {
            for (int i = 0; i < contents.Length; i++)
            {
                var isActive = (i == Current);

                var item = contents[i];
                var clicked = item.image != null
                    ? GUILib.ToolbarToggle(ref isActive, item.image, Vector2.zero, item.tooltip)
                    : GUILib.Toggle(ref isActive, item, EditorStyles.toolbarButton);

                if (!clicked)
                    continue;

                Current = (!isActive && _CanDeselectAll) ? -1 : i;

                OnTabChange?.Invoke();
                if (_Window == null)
                    continue;
                _Window.NeedRepaint = true;
            }
            
           
        }
        
        public static ATabList CreateFromEnum<T>(IEditorWindow window,bool canDeselectAll = false)
        {
            var widget = new ATabList(window,canDeselectAll);

            var values = Enum.GetValues(typeof(T));
            var contents = new List<GUIContent>(values.Length);

            foreach (var val in values)
            {
                contents.Add(new GUIContent(val.ToString()));
            }

            widget.contents = contents.ToArray();

            return widget;
        }

        static GUIContent GetGUIContent(object key)
        {
            if(key is GUIContent)
                return (GUIContent) key;
            if (key is Texture)
                return new GUIContent((Texture)key);
            if (key is string)
                return new GUIContent((string) key);
            return GUIContent.none;
        }
        
        public static ATabList Create(IEditorWindow window,List<object> tabs,bool canDeselectAll = false)
        {
            var widget = new ATabList(window,canDeselectAll);

            var contents = new List<GUIContent>(tabs.Count);
            foreach (var tab in tabs)
            {
                contents.Add(GetGUIContent(tab));
            }
            widget.contents = contents.ToArray();

            return widget;
        }
    }
}