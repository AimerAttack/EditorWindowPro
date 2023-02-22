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
            EUM_Helper.Instance.ZoomIndex = EUM_Helper.DefaultZoomIndex();
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
                optionsMenu.AddItem(new GUIContent("New"), false, NewFile);
                var buttonRect = GUILayoutUtility.GetLastRect();
                var dropdownRect = new Rect(buttonRect);
                dropdownRect.y += s_Height;
                optionsMenu.DropDown(dropdownRect);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Zoom:", GUILayout.Width(40));
            EUM_Helper.Instance.ZoomIndex = EditorGUILayout.Popup(EUM_Helper.Instance.ZoomIndex,
                EUM_Helper.GetZoomScalesText(), GUILayout.Width(60));
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Fit Canvas", "ToolbarButton"))
            {

            }

            if (GUILayout.Button("Save", "ToolbarButton"))
            {
                SaveFile();
            }

            GUILayout.FlexibleSpace();

            if (GUILib.Toggle(ref EUM_Helper.Instance.Preview, new GUIContent("Preview"),
                    new GUIStyle("ToolbarButton")))
            {
                if (EUM_Helper.Instance.Preview)
                {
                    EUM_Helper.Instance.SelectWidget = null;
                    EUM_Helper.Instance.HoverWidget = null;
                    EUM_Helper.Instance.OnSelectWidgetChange?.Invoke();
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        void OpenFile()
        {
            var path = EditorUtility.OpenFilePanelWithFilters("Open File", "", new[] {"EUM_Object", "asset"});
            if(string.IsNullOrEmpty(path))
                return;
            var relativePath = Utility.Utility.GetRelativePathInProject(path);
            var data = AssetDatabase.LoadAssetAtPath<EUM_Object>(relativePath);
            LoadData(data);
        }

        void NewFile()
        {
            var path = EditorUtility.SaveFilePanelInProject("Save File", "NewFile", "asset", "Save File");
            var data = ScriptableObject.CreateInstance<EUM_Object>();
            AssetDatabase.CreateAsset(data,path);
            AssetDatabase.SaveAssets();
            LoadData(data);
        }

        void SaveFile()
        {
            if (string.IsNullOrEmpty(EUM_Helper.Instance.FilePath))
            {
                //新文件，需要选择保存路径
                var path = EditorUtility.SaveFilePanelInProject("Save File", "NewFile", "asset", "Save File");
                SaveToPath(path);
            }
            else
            {
                //已有文件，直接保存
                SaveToPath(EUM_Helper.Instance.FilePath);
            }
        }

        void LoadData(EUM_Object obj)
        {
            
        }

        void SaveToPath(string path)
        {
            EUM_Helper.Instance.FilePath = path;
        }
}
}