
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
        
        GUILayout.BeginHorizontal();
        var tmpIntSlider1 = EditorGUILayout.IntSlider("IntSlider",_Logic.IntSlider1, _Logic.IntSlider1Min, _Logic.IntSlider1Max);
        if(tmpIntSlider1 != _Logic.IntSlider1)
        {
            _Logic.IntSlider1 = tmpIntSlider1;
            _Logic.IntSlider1ValueChange();
        }
        GUILayout.EndHorizontal();
        
        
        var tmpIntField1 = EditorGUILayout.IntField("IntField",_Logic.IntField1);
        if(tmpIntField1 != _Logic.IntField1)
        {
            _Logic.IntField1 = tmpIntField1;
            _Logic.IntField1ValueChange();
        }
        
        
        var tmpFloatField1 = EditorGUILayout.FloatField("FloatField",_Logic.FloatField1);
        if(tmpFloatField1 != _Logic.FloatField1)
        {
            _Logic.FloatField1 = tmpFloatField1;
            _Logic.FloatField1ValueChange();
        }
        
        
        var tmpLongField1 = EditorGUILayout.LongField("LongField",_Logic.LongField1);
        if(tmpLongField1 != _Logic.LongField1)
        {
            _Logic.LongField1 = tmpLongField1;
            _Logic.LongField1ValueChange();
        }

    }
}
