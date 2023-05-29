using System;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.TestCase
{
    public class TestEditorWindow : EditorWindow
    {
        private string popString;
        private void OnGUI()
        {
            GUILib.Area(new Rect(20,20,100,1000),DrawArea);
        }

        void DrawArea()
        {
            GUILib.Label("test for scrollview");
            GUILib.Frame(new Rect(30,30,1,1),Color.cyan,1f);
            if (GUILib.Popup(ref popString, new string[] {"a", "b", "c"}))
            {
                Debug.Log("pop value change");
            }
        }
    }
}