using Amazing.Editor.Library;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Toolbox : I_EUM_Drawable
    {
        private EUM_Title _Title;
        private Vector2 _ScrollPos;
        private bool _ShowBuildIn;
        private bool _ShowCustom;
        private bool _ShowContainers;
        private bool _ShowControls;
        private bool _ShowNumericFields;

        public EUM_Toolbox()
        {
            _Title = new EUM_Title(new GUIContent("Toolbox"));
            _ShowBuildIn = true;
        }
        
        public void Draw(ref Rect rect)
        {
            GUILib.Rect(rect,GUILib.s_DefaultColor , 1f);
            
            _Title.Draw(ref rect);

            GUILayout.BeginArea(rect);
            
            GUILayout.Space(10);
            GUILib.HorizontalRect(DrawToggleTab);
            
            GUILib.ScrollView(ref _ScrollPos,DrawControls);
            GUILayout.EndArea();
        }

        void DrawToggleTab()
        {
            GUILayout.FlexibleSpace();
            if(GUILib.Toggle(ref _ShowBuildIn,new GUIContent("BuildIn"),new GUIStyle("Button")))
            {
                if(_ShowBuildIn)
                    _ShowCustom = false;
            }

            if (GUILib.Toggle(ref _ShowCustom, new GUIContent("Custom"),new GUIStyle("Button")))
            {
                if (_ShowCustom)
                    _ShowBuildIn = false;
            }
            GUILayout.FlexibleSpace();
        }
        
        void DrawControls()
        {
            if(_ShowBuildIn)
                DrawBuildIn();
            if(_ShowCustom)
                DrawCustom();
        }
        
        void DrawBuildIn()
        {
            GUILib.Toggle(ref _ShowContainers,new GUIContent( "Containers"),new GUIStyle("FoldoutHeader"));
            if(_ShowContainers)
                DrawContainers();

            GUILib.Toggle(ref _ShowControls,new GUIContent("Controls"),new GUIStyle("FoldoutHeader"));
            if(_ShowControls)
                DrawBaseControls();
            
            GUILib.Toggle(ref _ShowNumericFields, new GUIContent("NumericFields"),new GUIStyle("FoldoutHeader"));
            if(_ShowNumericFields)
                DrawNumericFields();
        }

        void DrawContainers()
        {
            
        }
        
        void DrawBaseControls()
        {
            GUILayout.Button("Button");
            GUILayout.Button("Label");
        }
        
        void DrawNumericFields()
        {
            
        }

        void DrawCustom()
        {
            
        }
    }
}