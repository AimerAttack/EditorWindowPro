
using System;
using EditorUIMaker;
using UnityEditor;
using UnityEngine;
using Sirenix.Serialization;


public class NewWindow : EditorWindow,ISerializationCallbackReceiver
{
    [MenuItem("Tools/NewWindow")]
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
        GUILib.VerticelRect((() =>
        {
            var stylewwww = new GUIStyle(GUI.skin.button);
            stylewwww.alignment = TextAnchor.UpperRight;
            if(GUILib.Button("Button",stylewwww,GUILayout.MinHeight(102)))
            {
                _Logic.Clickwwww();
            }
        
        }),null,null);
        
        GUILib.HorizontalRect((() =>
        {
            GUILib.ScrollView(ref _Logic.ScrollView1,() =>
            {
                var styleButton3 = new GUIStyle(GUI.skin.button);
                styleButton3.alignment = TextAnchor.MiddleCenter;
                if(GUILib.Button("dwadwa",styleButton3,GUILayout.MinHeight(29)))
                {
                    _Logic.ClickButton3();
                }
            
            },GUILayout.MinHeight(366));
        }),null,null);

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
