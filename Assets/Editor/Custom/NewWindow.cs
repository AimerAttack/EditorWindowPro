
using System;
using EditorUIMaker;
using UnityEditor;
using UnityEngine;
using Sirenix.Serialization;

public class NewWindow : EditorWindow,ISerializationCallbackReceiver
{
    [MenuItem("Tools/222")]
    public static void ShowWindow()
    {
        var window = GetWindow<NewWindow>();
        window.titleContent = new GUIContent("NewWindow");
        window.Init();
        window.Show();
    }

    public NewWindow_Logic _Logic;

    void Init()
    {
        _Logic = new NewWindow_Logic(this);

        InitTreeView2();

        _Logic.Init();
    }

    private SimpleTreeView _TreeView2;
    public SimpleTreeView TreeView2 => _TreeView2;
    void InitTreeView2()
    {
        if(_TreeView2 != null)
            return;
        _TreeView2 = SimpleTreeView.Create("TreeView",0);
        _TreeView2.OnSelectionChanged += _Logic.TreeView2SelectChange;
        var expend = false;
        if(expend)
            _TreeView2.ExpandAll();
    }

    void OnGUI()
    {
        InitTreeView2();
        _TreeView2.Draw();
        
        GUILayout.BeginHorizontal();
        var styleButton1 = new GUIStyle(GUI.skin.button);
        styleButton1.alignment = TextAnchor.MiddleCenter;
        if(GUILib.Button("Button",styleButton1))
        {
            _Logic.ClickButton1();
        }
        
        GUILayout.EndHorizontal();
        
        GUILib.Space(30);

        Repaint();
    }

    void OnEnable()
    {
        AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
        AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
    }

    void OnDisable()
    {
        AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
        AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
    }
    
    void OnBeforeAssemblyReload()
    {
        _Logic.BeforeReloadDoMain();
    }
    
    void OnAfterAssemblyReload()
    {
        _Logic.AfterReloadDoMain();
    }

    [SerializeField] [HideInInspector] private SerializationData serializationData;

    public void OnBeforeSerialize()
    {
        UnitySerializationUtility.SerializeUnityObject((UnityEngine.Object) this, ref this.serializationData);
    }

    public void OnAfterDeserialize()
    {
        UnitySerializationUtility.DeserializeUnityObject((UnityEngine.Object) this, ref this.serializationData);
    }
}
