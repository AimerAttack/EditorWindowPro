
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
            var styleButton4 = new GUIStyle(GUI.skin.button);
            styleButton4.alignment = TextAnchor.MiddleCenter;
            if(GUILib.Button("Button",styleButton4,GUILayout.MinHeight(105)))
            {
                _Logic.ClickButton4();
            }
        
        }),null,null);
        
        GUILib.HorizontalRect((() =>
        {
            GUILib.ScrollView(ref _Logic.ScrollView1,() =>
            {
                var styleButton3 = new GUIStyle(GUI.skin.button);
                styleButton3.alignment = TextAnchor.MiddleCenter;
                if(GUILib.Button("Button",styleButton3,null))
                {
                    _Logic.ClickButton3();
                }
            
            },GUILayout.MinHeight(145));
        }),null,GUILayout.MinHeight(184));

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
