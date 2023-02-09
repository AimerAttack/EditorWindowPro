using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class UIEditorToolbar
    {



    public void DrawToolbar(UIEditorSceneSettings sceneSettings, UIEditorWindow window)
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        window.ToolbarRect = new Rect(0, 0, window.position.width, 18);

        {
            EditorGUILayout.LabelField(" Toolbox:", GUILayout.Width(60));
            int gridAlreadyVisibleIndex = UIEditorVariables.ToolboxVisible == true ? 0 : 1;
            int newVisibleIndex = EditorGUILayout.Popup(gridAlreadyVisibleIndex, new string[] { "On", "Off", "Refresh Controls" }, GUILayout.Width(40));
            if (newVisibleIndex != gridAlreadyVisibleIndex)
            {
                if (newVisibleIndex == 0) UIEditorVariables.ToolboxVisible = true;
                else if (newVisibleIndex == 1) UIEditorVariables.ToolboxVisible = false;
                else if (newVisibleIndex == 2) UIEditorLibraryControl.RequiresToolboxRebuild = true;
            }
        }

        GUILayout.FlexibleSpace();

        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            int selectedIndex = 0;

            string[] backgroundStrings = new string[] { "Clear", "White", "Light Gray", "Gray", "Dark Gray", "Black", "Scene" };

            Color[] colorValues = new Color[]
            {
                new Color(1, 1, 1, 1),
                new Color(0.75f, 0.75f, 0.75f, 1),
                new Color(0.50f, 0.50f, 0.50f, 1),
                new Color(0.25f, 0.25f, 0.25f, 1),
                 new Color(0, 0, 0, 1),
            };

            if (sceneSettings.BackgroundRender == BackgroundRenderId.None)
            {
                selectedIndex = 0;
            }
            else if (sceneSettings.BackgroundRender == BackgroundRenderId.Scene)
            {
                selectedIndex = backgroundStrings.Length - 1;
            }
            else
            {
                for (int i = 0; i < colorValues.Length; ++i)
                {
                    if (sceneSettings.BackgroundColor == colorValues[i])
                    {
                        selectedIndex = i + 1;
                        break;
                    }
                }
            }

            EditorGUILayout.LabelField(" Background:", GUILayout.Width(80));
            int newSelectedIndex = EditorGUILayout.Popup(selectedIndex, backgroundStrings, GUILayout.Width(100));
            if (newSelectedIndex != selectedIndex)
            {
                if (newSelectedIndex == 0)
                {
                    sceneSettings.BackgroundRender = BackgroundRenderId.None;
                    sceneSettings.BackgroundColor = new Color(0, 0, 0, 0);
                    if (sceneSettings != null) sceneSettings.BackgroundCamera = null;
                }
                else if (newSelectedIndex == backgroundStrings.Length - 1)
                {
                    sceneSettings.BackgroundRender = BackgroundRenderId.Scene;
                    sceneSettings.BackgroundColor = new Color(0, 0, 0, 0);
                }
                else
                {
                    sceneSettings.BackgroundRender = BackgroundRenderId.Color;
                    sceneSettings.BackgroundColor = colorValues[newSelectedIndex - 1];
                    if (sceneSettings != null) sceneSettings.BackgroundCamera = null;
                }
            }

            if (sceneSettings.BackgroundRender == BackgroundRenderId.Scene)
            {
                sceneSettings.BackgroundCamera = (Camera)EditorGUILayout.ObjectField(sceneSettings.BackgroundCamera, typeof(Camera), true, GUILayout.Width(130));
            }

            EditorGUILayout.EndHorizontal();
        }

        {
          //  int selectedGroup = UIEditorVariables.GridSnap == true ? 0 : 1;

            EditorGUILayout.BeginHorizontal();
            //int newGroup = EditorGUILayout.Popup(selectedGroup, new string[] { "Snap to grid", "Do not snap" }, EditorStyles.toolbarPopup, GUILayout.Width(90));
            //if (newGroup != selectedGroup)
            //{
            //    if (newGroup == 0) UIEditorVariables.GridSnap = true;
            //    else UIEditorVariables.GridSnap = false;
            //}
            EditorGUILayout.LabelField(" Grid:", GUILayout.Width(40));
            int gridAlreadyVisibleIndex = UIEditorVariables.GridVisible == true ? 0 : 1;
            int newVisibleIndex = EditorGUILayout.Popup(gridAlreadyVisibleIndex, new string[] { "Visible", "Hidden" }, GUILayout.Width(70));
            if (newVisibleIndex != gridAlreadyVisibleIndex)
            {
                if (newVisibleIndex == 0) UIEditorVariables.GridVisible = true;
                else UIEditorVariables.GridVisible = false;
            }

            UIEditorVariables.GridSize = EditorGUILayout.IntField(UIEditorVariables.GridSize, GUILayout.Width(40));
            if (UIEditorVariables.GridSize < 2) UIEditorVariables.GridSize = 2;
            if (UIEditorVariables.GridSize > 2048) UIEditorVariables.GridSize = 2048;
            EditorGUILayout.EndHorizontal();
        }

        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(" Zoom:", GUILayout.Width(50));
            UIEditorVariables.ZoomIndex = EditorGUILayout.Popup(UIEditorVariables.ZoomIndex, UIEditorHelpers.GetZoomScalesText(), GUILayout.Width(60));
            EditorGUILayout.EndHorizontal();
        }


        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            bool isFirsttime = !UIEditorVariables.HasSceneLayerVisible;
            int selectedIndex = UIEditorVariables.SceneLayerVisible == true ? 0 : 1;
            EditorGUILayout.LabelField(" Scene UI:", GUILayout.Width(65));
            int newSelectedIndex = EditorGUILayout.Popup(selectedIndex, new string[] { "Visible", "Hidden" }, GUILayout.Width(70));
            if (newSelectedIndex != selectedIndex || isFirsttime)
            {
                UIEditorVariables.SceneLayerVisible = newSelectedIndex == 0 ? true : false;
                if (newSelectedIndex == 0) UIEditorHelpers.ShowUILayer();
                else UIEditorHelpers.HideUILayer();

            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.EndHorizontal();
    }
}

