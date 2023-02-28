
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
        
        var styledwa = new GUIStyle(GUI.skin.button);
        styledwa.alignment = TextAnchor.MiddleCenter;
        if(GUILayout.Button("dwa",styledwa))
        {
            _Logic.Clickdwa();
        }
        
        GUILayout.FlexibleSpace();
        var styleButton2 = new GUIStyle(GUI.skin.button);
        styleButton2.alignment = TextAnchor.MiddleCenter;
        if(GUILayout.Button("Button2",styleButton2))
        {
            _Logic.ClickButton2();
        }
        
        GUILayout.BeginHorizontal();
        
        var styleButton3 = new GUIStyle(GUI.skin.button);
        styleButton3.alignment = TextAnchor.MiddleCenter;
        if(GUILayout.Button("Button3",styleButton3))
        {
            _Logic.ClickButton3();
        }
        
        GUILayout.FlexibleSpace();
        var styleButton4 = new GUIStyle(GUI.skin.button);
        styleButton4.alignment = TextAnchor.MiddleCenter;
        if(GUILayout.Button("Button4",styleButton4))
        {
            _Logic.ClickButton4();
        }
        
        GUILayout.EndHorizontal();

    }
}
