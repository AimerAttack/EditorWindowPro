
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

        

        _Logic.Init();
    }

    

    void OnGUI()
    {
        if(GUILib.Color("Color",ref _Logic.Color1))
        {
            _Logic.Color1ValueChange();
        }


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
