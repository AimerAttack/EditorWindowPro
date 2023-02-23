using System.Collections.Generic;
using UnityEngine;

namespace EditorUIMaker
{
    [CreateAssetMenu(fileName = "NewWindow", menuName = "EditorUIMaker/NewWindow", order = 1)]
    public class EUM_Object : ScriptableObject
    {
        public EUM_Stash Stash;
    }
}
