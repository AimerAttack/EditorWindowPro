using System;
using System.Collections.Generic;
using NUnit.Framework;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class TestDataWindow : OdinEditorWindow
    {
        [MenuItem("Tools/数据测试")]
        static void OpenWindow()
        {
            var window = GetWindow<TestDataWindow>();

            window.Init();
            
            window.ml.rect = new List<Rect>();
            window.ml.rect.Add(new Rect());
            window.ml.rect.Add(new Rect());
            window.ml.rect.Add(new Rect());

            window.ml.selectID = 3;
            
            window.AddItem(1);
            window.AddItem(2);
            window.AddItem(3);
            window.AddItem(4);
            window.AddItem(5);
            
            
            window.Show();
        }


        void Init()
        {
            ml = new MyClass();
            MyClass.Instance = ml; 
        }

        public Dictionary<int, SelectInfo> infos = new Dictionary<int, SelectInfo>();

        public List<int> ids = new List<int>();
        public List<SelectInfo> classes = new List<SelectInfo>();

        public MyClass ml;

        private SelectInfo SelectCls
        {
            get
            {
                var index = ids.IndexOf(ml.selectID);
                return classes[index];
            }
        }

        void AddItem(int i)
        {
            ids.Add(i);
            classes.Add(new SelectInfo());
            infos.Add(i,new SelectInfo());
        }


        
        protected override void OnGUI()
        {
            ml.a = EditorGUILayout.IntField("a", ml.a);
            for (int i = 0; i < ml.rect.Count; i++)
            {
                var index = i;
                ml.rect[index] = EditorGUILayout.RectField("rect" + index, ml.rect[index]);
            }
            GUILayout.Label("ids数量：" + ids.Count.ToString());
            GUILayout.Label("classes数量：" + classes.Count.ToString());
            GUILayout.Label("dic数量：" + infos.Count.ToString());
            if(SelectCls == null)
                Debug.Log("select cls is null");
            if(MyClass.Instance == null)
                Debug.Log("instance is null");
        }
        
        
        
        void OnEnable()
        {
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
        }

        void OnDisable()
        {
            AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
        }
        
        public void OnAfterAssemblyReload()
        {
            MyClass.Instance = ml;
            Debug.Log("After Assembly Reload");
        }
        

    }

  
    
    
    public class SelectInfo
    {
        
    }

    public class ClassB
    {
        public int a;
    }

    public class MyClass
    {
        [SerializeField]
        public static MyClass Instance;
        
        public MyClass()
        {
            Debug.Log("construct");
            b = new ClassB();
        }
        public int a;
        public List<Rect> rect;
        public int selectID;

        public ClassB b;
    }
}