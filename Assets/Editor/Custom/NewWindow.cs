
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
        
        
        var tmpVector3Field1 = EditorGUILayout.Vector3Field("Vector3Field",_Logic.Vector3Field1);
        if(tmpVector3Field1 != _Logic.Vector3Field1)
        {
            _Logic.Vector3Field1 = tmpVector3Field1;
            _Logic.Vector3Field1ValueChange();
        }
        
        
        var tmpVector3IntField1 = EditorGUILayout.Vector3IntField("Vector3IntField",_Logic.Vector3IntField1);
        if(tmpVector3IntField1 != _Logic.Vector3IntField1)
        {
            _Logic.Vector3IntField1 = tmpVector3IntField1;
            _Logic.Vector3IntField1ValueChange();
        }

    }
}
