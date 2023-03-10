using System.Collections.Generic;
using OdinSerializer;
using UnityEngine;

namespace EditorUIMaker
{
    [CreateAssetMenu(fileName = "NewWindow", menuName = "EditorUIMaker/NewWindow", order = 1)]
    public class EUM_Object : SerializedScriptableObject
    {
        public string MenuItemPath;
        public EUM_Stash Stash;
    }
}
