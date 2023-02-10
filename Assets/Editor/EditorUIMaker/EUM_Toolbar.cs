using Amazing.Editor.Library;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Toolbar : I_EUM_Drawable
    {
        public const float s_Height = 18;

        public EUM_Toolbar()
        {
            EUM_Setting.ZoomIndex = EUM_Setting.DefaultZoomIndex();
        }

        public void Draw(ref Rect rect)
        {
            var drawRect = new Rect(rect.x, rect.y, rect.width, s_Height);
            rect.yMin += s_Height;

            GUILayout.BeginArea(drawRect);

            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Button("File", "ToolbarPopup", GUILayout.Width(40)))
            {
                var optionsMenu = new GenericMenu();
                optionsMenu.AddItem(new GUIContent("Open"), false, OpenFile);
                optionsMenu.AddItem(new GUIContent("New"),false, NewFile);
                var buttonRect = GUILayoutUtility.GetLastRect();
                var dropdownRect = new Rect(buttonRect);
                dropdownRect.y += s_Height;
                optionsMenu.DropDown(dropdownRect);
            }
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Zoom:", GUILayout.Width(40));
            EUM_Setting.ZoomIndex = EditorGUILayout.Popup(EUM_Setting.ZoomIndex, EUM_Setting.GetZoomScalesText(), GUILayout.Width(60));
            EditorGUILayout.EndHorizontal();
            
            if(GUILayout.Button("Fit Canvas", "ToolbarButton"))
            {
                
            }
            
            GUILayout.FlexibleSpace();
            
            GUILib.Toggle(ref EUM_Helper.Instance.Preview, new GUIContent("Preview"), new GUIStyle("ToolbarButton"));

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        void OpenFile()
        {
            
        }

        void NewFile()
        {
            
        }
    }
}