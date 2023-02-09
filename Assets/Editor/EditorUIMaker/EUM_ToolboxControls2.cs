using Amazing.Editor.Library;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_ToolboxControls2
    {
        public const int ToolboxBaseX = 4;
        public const int ToolboxBaseY = 22;
        public const int ToolboxBoxWidth = 100;

        private Rect _ToolboxRect;
        private Vector2 _ScrollPosition;

        public void Draw(Rect rect)
        {
            _ToolboxRect = new Rect(ToolboxBaseX, ToolboxBaseY, ToolboxBoxWidth, rect.height - 52);

            GUIStyle boxSTyle = GUI.skin.box;
            GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.85f);
            Texture2D savedBackgroundTexture = boxSTyle.normal.background;
            boxSTyle.normal.background = Texture2D.whiteTexture;
            GUI.Box(_ToolboxRect, string.Empty);
            boxSTyle.normal.background = savedBackgroundTexture;
            GUI.backgroundColor = Color.white;

            DrawControls();
        }

        void DrawControls()
        {
            float startBoxY = ToolboxBaseY + 4;
            Rect lastRect = new Rect(ToolboxBaseX + 0 + 4, startBoxY, ToolboxBoxWidth - 8, 24);
            GUI.Box(lastRect, string.Empty, GUI.skin.box);

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                if (lastRect.Contains(Event.current.mousePosition))
                {
                    DragAndDrop.PrepareStartDrag();
                    DragAndDrop.objectReferences = new UnityEngine.Object[] {null};
                    DragAndDrop.SetGenericData("IsUIEditorControl", true);

                    DragAndDrop.StartDrag("Create a new control");
                    Event.current.Use();
                }
            }

            lastRect.y += 3;

            GUI.Label(lastRect, "Display");
        }

        public void HandleToolboxDrags()
        {
            switch (Event.current.type)
            {
                case EventType.DragPerform:
                {
                    object genericData = DragAndDrop.GetGenericData("IsUIEditorControl");

                    if (genericData != null && !_ToolboxRect.Contains(UIEditorInput.LastKnownMousePosition))
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