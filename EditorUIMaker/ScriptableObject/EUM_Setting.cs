using System;
using System.Collections.Generic;
using EditorUIMaker.Widgets;
using OdinSerializer;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    [CreateAssetMenu(menuName = "EditorUIMaker/Setting")]
    public class EUM_Setting : SerializedScriptableObject
    {
        public List<string> AdditionNamespaces;
        public List<EUM_Widget> Widgets;
        public List<EUM_Container> Containers;
    }

}