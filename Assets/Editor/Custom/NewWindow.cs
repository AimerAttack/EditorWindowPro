
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
        GUILayout.Label("TextField");
        var tmpTextField1 = GUILayout.TextField(_Logic.TextField1);
        if(tmpTextField1 != _Logic.TextField1)
        {
            _Logic.TextField1 = tmpTextField1;
            _Logic.TextField1ValueChange();
        }
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        var tmpSlider1 = EditorGUILayout.Slider("Slider",_Logic.Slider1, _Logic.Slider1Min, _Logic.Slider1Max);
        if(tmpSlider1 != _Logic.Slider1)
        {
            _Logic.Slider1 = tmpSlider1;
            _Logic.Slider1ValueChange();
        }
        GUILayout.EndHorizontal();

    }
}
