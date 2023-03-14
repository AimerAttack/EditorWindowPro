using System;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public abstract class EUM_Widget : EUM_BaseWidget
    {
        internal override EUM_BaseWidget CloneWithChildren()
        {
            return Clone();
        }
    }
}