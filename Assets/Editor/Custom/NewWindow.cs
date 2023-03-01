
using System;
using EditorUIMaker;
using UnityEditor;
using UnityEngine;
public class NewWindow : EditorWindow
{
    [MenuItem("Tools/NewWindow")]
    public static void ShowWindow()
    {
        var window = GetWindow<NewWindow>();
        window.titleContent = new GUIContent("NewWindow");
        window.Show();
    }

    private NewWindow_Logic _Logic;

    public NewWindow()
    {
        _Logic = new NewWindow_Logic();
    }

    void OnGUI()
    {
        
        GUILayout.BeginHorizontal();
        
        GUILayout.EndHorizontal();
        
        if(GUILib.Popup(ref _Logic.bStr,_Logic.bOptions))
        {
            _Logic.bIndex = Array.IndexOf(_Logic.bOptions,_Logic.bStr);
            _Logic.bValueChange();
        }
        
        GUILib.Foldout("Foldout1",ref _Logic.c);
        if(_Logic.c)
        {
        
        
        var styleButton1 = new GUIStyle(GUI.skin.button);
        styleButton1.alignment = TextAnchor.MiddleCenter;
        if(GUILayout.Button("Button",styleButton1))
        {
            _Logic.ClickButton1();
        }
        
        }

    }
}
