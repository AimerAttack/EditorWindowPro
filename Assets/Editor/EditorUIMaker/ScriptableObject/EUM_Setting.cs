using System.Collections.Generic;
using EditorUIMaker.Widgets;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Setting : SerializedScriptableObject
    {
        public List<string> AdditionNamespaces;
        public List<EUM_Widget> Widgets;
        public List<EUM_Container> Containers;
    }
}