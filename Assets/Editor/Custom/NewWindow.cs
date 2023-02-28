
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
        
        var styleButton5 = new GUIStyle(GUI.skin.button);
        styleButton5.alignment = TextAnchor.MiddleCenter;
        if(GUILayout.Button("Button",styleButton5))
        {
            _Logic.ClickButton5();
        }
        
        var styledwa = new GUIStyle(GUI.skin.button);
        styledwa.alignment = TextAnchor.MiddleCenter;
        if(GUILayout.Button("test",styledwa))
        {
            _Logic.Clickdwa();
        }
        
        var styleButton2 = new GUIStyle(GUI.skin.button);
        styleButton2.alignment = TextAnchor.MiddleCenter;
        if(GUILayout.Button("Button",styleButton2))
        {
            _Logic.ClickButton2();
        }
        
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        
        var styleButton3 = new GUIStyle(GUI.skin.button);
        styleButton3.alignment = TextAnchor.MiddleCenter;
        if(GUILayout.Button("Button",styleButton3))
        {
            _Logic.ClickButton3();
        }
        
        GUILayout.FlexibleSpace();
        var styleButton4 = new GUIStyle(GUI.skin.button);
        styleButton4.alignment = TextAnchor.MiddleCenter;
        if(GUILayout.Button("Button",styleButton4))
        {
            _Logic.ClickButton4();
        }
        
        GUILayout.EndHorizontal();
        
        var styleButton7 = new GUIStyle(GUI.skin.button);
        styleButton7.alignment = TextAnchor.MiddleCenter;
        if(GUILayout.Button("Button",styleButton7))
        {
            _Logic.ClickButton7();
        }
        
        var styleButton6 = new GUIStyle(GUI.skin.button);
        styleButton6.alignment = TextAnchor.MiddleCenter;
        if(GUILayout.Button("Button",styleButton6))
        {
            _Logic.ClickButton6();
        }

    }
}
