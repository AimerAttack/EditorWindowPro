using System;
using UnityEngine;

namespace EditorUIMaker.Widgets
{
    public abstract class EUM_Widget : EUM_BaseWidget
    {
        public override EUM_BaseWidget SingleClone()
        {
            return Clone();
        }

    }
}