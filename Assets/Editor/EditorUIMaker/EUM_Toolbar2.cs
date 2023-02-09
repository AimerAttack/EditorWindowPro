using Amazing.Editor.Library;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Toolbar2 : I_EUM_Drawable
    {
        private const float s_Height = 18;

        public EUM_Toolbar2()
        {
            EditorUIMakerSetting.ZoomIndex = EditorUIMakerSetting.DefaultZoomIndex();
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
            EditorUIMakerSetting.ZoomIndex = EditorGUILayout.Popup(EditorUIMakerSetting.ZoomIndex, EditorUIMakerSetting.GetZoomScalesText(), GUILayout.Width(60));
            EditorGUILayout.EndHorizontal();
            
            if(GUILayout.Button("Fit Canvas", "ToolbarButton"))
            {
                
            }
            
            GUILayout.FlexibleSpace();
            
            if(GUILayout.Button("Preview","ToolbarButton"))
            {
                
            }

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