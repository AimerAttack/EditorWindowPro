
using UnityEditor;
using UnityEngine;
public class NewWindow : EditorWindow
{
    [MenuItem("Tools/123")]
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
        
        if(GUILayout.Button("Button1"))
        {
            _Logic.ClickButton1();
        }
        
        GUILayout.Label("Label1");
        GUILayout.Space(5);
        if(GUILayout.Button("Button2"))
        {
            _Logic.ClickButton2();
        }
        
        GUILayout.BeginHorizontal();
        
        GUILayout.Label("Label2");
        GUILayout.Label("Label3");
        GUILayout.EndHorizontal();

    }
}
