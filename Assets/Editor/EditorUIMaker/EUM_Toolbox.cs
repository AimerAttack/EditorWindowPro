using System.Collections.Generic;
using Amazing.Editor.Library;
using EditorUIMaker.Widgets;
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
        private Rect _Rect;
        private EUM_BaseWidget _FloatingWidget;

        private List<EUM_BaseWidget> _Controls = new List<EUM_BaseWidget>();

        public EUM_Toolbox()
        {
            _Title = new EUM_Title(new GUIContent("Toolbox"));
            _ShowBuildIn = true;

            _Controls.Add(new EUM_Button());
            _Controls.Add(new EUM_Label());
        }

        public void DrawWithRect(ref Rect rect)
        {
            _Rect = rect;
            GUILib.Rect(rect, GUILib.s_DefaultColor, 1f);

            _Title.DrawWithRect(ref rect);

            GUILayout.BeginArea(rect);

            GUILayout.Space(10);
            GUILib.HorizontalRect(DrawToggleTab);

            GUILib.ScrollView(ref _ScrollPos, DrawControls);
            GUILayout.EndArea();

            if (_FloatingWidget != null)
            {
                var floatRect = new Rect(Event.current.mousePosition, new Vector2(100, 30));
                _FloatingWidget.DrawWithRect(ref floatRect);
            }
        }

        void DrawToggleTab()
        {
            GUILayout.FlexibleSpace();
            if (GUILib.Toggle(ref _ShowBuildIn, new GUIContent("BuildIn"), new GUIStyle("Button")))
            {
                if (_ShowBuildIn)
                    _ShowCustom = false;
            }

            if (GUILib.Toggle(ref _ShowCustom, new GUIContent("Custom"), new GUIStyle("Button")))
            {
                if (_ShowCustom)
                    _ShowBuildIn = false;
            }

            GUILayout.FlexibleSpace();
        }

        void DrawControls()
        {
            if (_ShowBuildIn)
                DrawBuildIn();
            if (_ShowCustom)
                DrawCustom();
        }

        void DrawBuildIn()
        {
            GUILib.Toggle(ref _ShowContainers, new GUIContent("Containers"), new GUIStyle("FoldoutHeader"));
            if (_ShowContainers)
                DrawContainers();

            GUILib.Toggle(ref _ShowControls, new GUIContent("Controls"), new GUIStyle("FoldoutHeader"));
            if (_ShowControls)
                DrawBaseControls();

            GUILib.Toggle(ref _ShowNumericFields, new GUIContent("NumericFields"), new GUIStyle("FoldoutHeader"));
            if (_ShowNumericFields)
                DrawNumericFields();
        }

        void DrawContainers()
        {
        }

        void DrawBaseControls()
        {
            foreach (var control in _Controls)
            {
                GUILayout.Label(control.TypeName);

                var lastRect = GUILayoutUtility.GetLastRect();
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    if (lastRect.Contains(Event.current.mousePosition))
                    {
                        DragAndDrop.PrepareStartDrag();
                        DragAndDrop.objectReferences = new UnityEngine.Object[] {null};
                        DragAndDrop.SetGenericData("IsUIEditorControl", true);

                        DragAndDrop.StartDrag("Create a new control");
                        Event.current.Use();

                        _FloatingWidget = control.Clone();
                    }
                }
            }
        }

        void DrawNumericFields()
        {
        }

        void DrawCustom()
        {
        }

        public void HandleDrag()
        {
            switch (Event.current.type)
            {
                case EventType.DragPerform:
                {
                    object genericData = DragAndDrop.GetGenericData("IsUIEditorControl");
                    _FloatingWidget = null;

                    if (genericData != null && !_Rect.Contains(Event.current.mousePosition))
                    {
                        DragAndDrop.AcceptDrag();
                        string objectType = (string) DragAndDrop.GetGenericData("ControlType");
                        System.Action objectAction = (System.Action) DragAndDrop.GetGenericData("ControlAction");
                        UIEditorLibraryControl prefab = (UIEditorLibraryControl) DragAndDrop.GetGenericData("Prefab");

                        if (!string.IsNullOrEmpty(objectType))
                            EditorApplication.ExecuteMenuItem(objectType);
                        else if (prefab != null)
                        {
                        }
                        else if (objectAction != null)
                            objectAction.Invoke();

                        Event.current.Use();

                        if (EUM_Helper.VitualWindowRect.Contains(Event.current.mousePosition) &&
                            EUM_Helper.ViewportRect.Contains(Event.current.mousePosition)
                           )
                        {
                            Debug.Log("in");
                        }
                        else
                        {
                            Debug.LogError("not in");
                        }
                    }
                    else if (DragAndDrop.objectReferences != null && DragAndDrop.objectReferences.Length > 0)
                    {
                        for (int i = 0; i < DragAndDrop.objectReferences.Length; ++i)
                        {
                            Texture2D textureData = DragAndDrop.objectReferences[i] as Texture2D;
                            if (textureData != null)
                            {
                                if (textureData != null)
                                {
                                    Sprite spriteData =
                                        AssetDatabase.LoadAssetAtPath(DragAndDrop.paths[i], typeof(Sprite)) as Sprite;
                                    if (spriteData != null)
                                    {
                                        EditorApplication.ExecuteMenuItem("GameObject/UI/Image");
                                        Selection.activeGameObject.GetComponent<UnityEngine.UI.Image>().sprite =
                                            spriteData;
                                        Selection.activeGameObject.GetComponent<UnityEngine.RectTransform>().sizeDelta =
                                            new Vector2(textureData.width, textureData.height);
                                        Selection.activeGameObject.name = spriteData.name;
                                    }
                                    else
                                    {
                                        EditorApplication.ExecuteMenuItem("GameObject/UI/Raw Image");
                                        Selection.activeGameObject.GetComponent<UnityEngine.UI.RawImage>().texture =
                                            textureData;
                                        Selection.activeGameObject.GetComponent<UnityEngine.RectTransform>().sizeDelta =
                                            new Vector2(textureData.width, textureData.height);
                                        Selection.activeGameObject.name = textureData.name;
                                    }
                                }
                            }

                            GameObject isRect = DragAndDrop.objectReferences[i] as GameObject;
                            if (isRect != null)
                            {
                                if (isRect.GetComponent<RectTransform>() != null)
                                {
                                }
                            }
                        }
                    }

                    break;
                }
                case EventType.DragUpdated:
                {
                    if (DragAndDrop.GetGenericData("IsUIEditorControl") != null)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                    }
                    else if (DragAndDrop.objectReferences != null && DragAndDrop.objectReferences.Length > 0)
                    {
                        bool isTextureType = false;
                        bool isRectTransformType = false;

                        for (int i = 0; i < DragAndDrop.objectReferences.Length; ++i)
                        {
                            Texture2D isTexture = DragAndDrop.objectReferences[i] as Texture2D;
                            if (isTexture != null)
                            {
                                isTextureType = true;
                            }

                            GameObject isRectTransform = DragAndDrop.objectReferences[i] as GameObject;
                            if (isRectTransform != null)
                            {
                                if (isRectTransform.GetComponent<RectTransform>() != null)
                                    isRectTransformType = true;
                            }
                        }

                        if (isTextureType || isRectTransformType)
                        {
                            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                            DragAndDrop.AcceptDrag();
                        }
                    }

                    break;
                }
            }
        }
    }
}