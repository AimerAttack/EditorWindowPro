
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
        
        var styleButton1 = new GUIStyle(GUI.skin.button);
        styleButton1.alignment = TextAnchor.UpperRight;
        if(GUILayout.Button("Button1",styleButton1))
        {
            _Logic.ClickButton1();
        }

    }
}
