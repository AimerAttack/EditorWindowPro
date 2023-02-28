
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
        styledwa.alignment = TextAnchor.UpperRight;
        if(GUILayout.Button("dwa",styledwa))
        {
            _Logic.Clickdwa();
        }

    }
}
