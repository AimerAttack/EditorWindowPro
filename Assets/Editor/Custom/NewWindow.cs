
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

    }
}
