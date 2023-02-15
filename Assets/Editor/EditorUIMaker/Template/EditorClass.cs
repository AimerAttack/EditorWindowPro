using Amazing.Editor.Library;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.Template
{
    public class EditorClass : EditorWindow
    {
        // [MenuItem("Tools/EditorUIMaker/Template")]
        static void OpenWindow()
        {
            var window = GetWindow<EditorClass>();
            window.titleContent = new GUIContent("EditorClass");
            window.Show();
        }

        private EditorLogic _Logic;

        public EditorClass()
        {
            Init();
        }

        void Init()
        {
            _Logic = new EditorLogic();
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Button1"))
            {
                _Logic.ClickButton1();
            }

            var tmpToggle11 = _Logic.Toggle1;
            _Logic.Toggle1 = EditorGUILayout.Toggle("Toggle1", _Logic.Toggle1);
            if (_Logic.Toggle1 != tmpToggle11)
                _Logic.OnToggle1Changed(_Logic.Toggle1);

            GUILayout.Label("label1");
            
            if(GUILib.IntSlider("IntSlider1", ref _Logic.IntSlider1, 0, 100))
                _Logic.OnIntSlider1Changed(_Logic.IntSlider1);
            
            GUILayout.EndHorizontal();
        }

    }
}