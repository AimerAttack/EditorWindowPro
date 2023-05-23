using System;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.TestCase
{
    public class TestForText : EditorWindow
    {
        private Vector2 scrollPosition;
        private void OnGUI()
        {
            GUILib.ScrollView(ref scrollPosition, () =>
            {
                GUILib.Label("test for scrollview");
                if (GUILib.Button("a simple button"))
                {
                    ClickButton();
                }
            });
        }

        void ClickButton()
        {
            
        }
    }
}