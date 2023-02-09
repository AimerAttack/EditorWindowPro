using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;


public class UIEditorToolbox
{

    public delegate void CreatePrefabDelegate(UIEditorLibraryControl control);

    public class NativeControlData
    {
        public string MenuPath;
        public string Display;
        public System.Action CreateAction;
        public UIEditorLibraryControl Prefab;
        public CreatePrefabDelegate CreatePrefabAction;
        public bool IsBreak;
    }

    private bool HasSearchedForUIFramework;
    private bool HasFoundUIFramework;
    private List<NativeControlData> UIFramworkControls = new List<NativeControlData>();
    private string UIFrameworkMenuPath;

    public bool IsUIFrameworkEnabled(List<NativeControlData> nativeControls, ref string menuPath)
    {
        try
        {
            menuPath = UIFrameworkMenuPath;
            nativeControls.AddRange(UIFramworkControls);

            if (HasSearchedForUIFramework) return HasFoundUIFramework;
            HasSearchedForUIFramework = false;

            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly.ManifestModule.Name.StartsWith("Assembly-CSharp-Editor"))
                {
                    var types = assembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (type.ToString() == "UIFrameworkUIEditorControls")
                        {
                            var classInstance = System.Activator.CreateInstance(type);

                            System.Reflection.FieldInfo[] members = type.GetFields();
                            for (int a = 0; a < members.Length; ++a)
                            {
                                if (members[a].FieldType == typeof(string[]) && members[a].Name == "NativeControls")
                                {
                                    UIFramworkControls.Clear();
                                    string[] controls = (string[])members[a].GetValue(classInstance);

                                    for (int b = 0; b < controls.Length; ++b)
                                        UIFramworkControls.Add(new NativeControlData() { MenuPath = "", Display = controls[b] });
                                }

                                if (members[a].FieldType == typeof(string) && members[a].Name == "MenuPath")
                                {
                                    UIFrameworkMenuPath = (string)members[a].GetValue(classInstance);
                                }
                            }

                            for (int b = 0; b < UIFramworkControls.Count; ++b)
                                UIFramworkControls[b].MenuPath = UIFrameworkMenuPath + UIFramworkControls[b].Display;

                            HasFoundUIFramework = true;
                        }
                    }
                }
            }

            if (UIFramworkControls.Count > 0)
            {
                UIFramworkControls.Add(new NativeControlData() { IsBreak = true });
                return true;

            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.ToString());
        }

        HasFoundUIFramework = false;
        return false;
    }

    //private bool IsTextMeshProEnabled()
    //{
    //    return true;
    //}

    private void CreateHorizontalLayout()
    {
        UnityEditor.EditorApplication.ExecuteMenuItem("GameObject/UI/Image");
        GameObject.DestroyImmediate(Selection.activeGameObject.GetComponent<UnityEngine.UI.Image>());
        GameObject.DestroyImmediate(Selection.activeGameObject.GetComponent<UnityEngine.CanvasRenderer>());

        Selection.activeGameObject.name = "Horizontal Layout";
        Selection.activeGameObject.AddComponent<UnityEngine.UI.HorizontalLayoutGroup>();
    }

    private void CreateVerticalLayout()
    {
        EditorApplication.ExecuteMenuItem("GameObject/UI/Image");
        GameObject.DestroyImmediate(Selection.activeGameObject.GetComponent<UnityEngine.UI.Image>());
        GameObject.DestroyImmediate(Selection.activeGameObject.GetComponent<UnityEngine.CanvasRenderer>());

        Selection.activeGameObject.name = "Vertical Layout";
        Selection.activeGameObject.AddComponent<UnityEngine.UI.VerticalLayoutGroup>();
    }

    private void CreatePrefab(UIEditorLibraryControl control)
    {
        EditorApplication.ExecuteMenuItem("GameObject/UI/Image");

        GameObject testImage = Selection.activeGameObject;

        Selection.activeGameObject = GameObject.Instantiate(control.gameObject) as GameObject;
        Selection.activeGameObject.transform.position = Vector3.zero;
        Selection.activeGameObject.transform.localPosition = Vector3.zero;

        Selection.activeGameObject.transform.localRotation = control.transform.localRotation;
        Selection.activeGameObject.transform.localScale = control.transform.localScale;
        Selection.activeGameObject.transform.SetParent(testImage.transform.parent);

        //if (UIEditorSelectionHelpers.MouseOverObject == null)
        //{
        //    Selection.activeGameObject.transform.SetParent(testImage.transform.parent);
        //    Selection.activeGameObject.transform.position = Vector3.zero;
        //    Selection.activeGameObject.GetComponent<RectTransform>().anchoredPosition = 
        //}
        Selection.activeGameObject.name = control.DisplayName;
        GameObject.DestroyImmediate(testImage);
    }

    public void CreatePrefab(GameObject control)
    {
        EditorApplication.ExecuteMenuItem("GameObject/UI/Image");

        GameObject testImage = Selection.activeGameObject;

        Selection.activeGameObject = GameObject.Instantiate(control) as GameObject;
        Selection.activeGameObject.transform.position = Vector3.zero;
        Selection.activeGameObject.transform.localPosition = Vector3.zero;

        Selection.activeGameObject.transform.localRotation = control.transform.localRotation;
        Selection.activeGameObject.transform.localScale = control.transform.localScale;
        Selection.activeGameObject.transform.SetParent(testImage.transform.parent);

        //if (UIEditorSelectionHelpers.MouseOverObject == null)
        //{
        //    Selection.activeGameObject.transform.SetParent(testImage.transform.parent);
        //    Selection.activeGameObject.transform.position = Vector3.zero;
        //    Selection.activeGameObject.GetComponent<RectTransform>().anchoredPosition = 
        //}
        Selection.activeGameObject.name = control.name;
        GameObject.DestroyImmediate(testImage);
    }

    private void CreateGridLayout()
    {
        EditorApplication.ExecuteMenuItem("GameObject/UI/Image");
        GameObject.DestroyImmediate(Selection.activeGameObject.GetComponent<UnityEngine.UI.Image>());
        GameObject.DestroyImmediate(Selection.activeGameObject.GetComponent<UnityEngine.CanvasRenderer>());

        Selection.activeGameObject.name = "Grid Layout";
        Selection.activeGameObject.AddComponent<UnityEngine.UI.GridLayoutGroup>();
    }

    private void CreateRichText()
    {
        EditorApplication.ExecuteMenuItem("GameObject/UI/Text");
        UnityEngine.UI.Text text = Selection.activeGameObject.GetComponent<UnityEngine.UI.Text>();
        text.name = "Rich Text";
        text.supportRichText = true;
        text.text = "We are <b>not</b> amused\nWe are <b><i>definitely not</i></b> amused\nWe are <color=green>green</color> with envy\nWe are <size=50>largely</size> unaffected.";
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(310, 140);
    }

    private void CreateContainer()
    {
        EditorApplication.ExecuteMenuItem("GameObject/UI/Image");
        GameObject.DestroyImmediate(Selection.activeGameObject.GetComponent<UnityEngine.UI.Image>());
        GameObject.DestroyImmediate(Selection.activeGameObject.GetComponent<UnityEngine.CanvasRenderer>());

        Selection.activeGameObject.name = "Container";
    }

    private void CreateMask()
    {
        EditorApplication.ExecuteMenuItem("GameObject/UI/Image");
        UnityEngine.UI.Mask mask = Selection.activeGameObject.AddComponent<UnityEngine.UI.Mask>();
        mask.showMaskGraphic = false;
        mask.name = "Mask";
    }

    private void CreateScrollView()
    {
        Sprite sprite1 = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");

        EditorApplication.ExecuteMenuItem("GameObject/UI/Image");
        Selection.activeGameObject.name = "Scroll View";
        GameObject scrollView = Selection.activeGameObject;
        scrollView.GetComponent<UnityEngine.UI.Image>().sprite = sprite1;
        scrollView.GetComponent<UnityEngine.UI.Image>().type = UnityEngine.UI.Image.Type.Sliced;
        scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 250);


        CreateMask();
        GameObject scrollRect = Selection.activeGameObject;
        scrollRect.GetComponent<UnityEngine.UI.Image>().sprite = sprite1;
        scrollRect.GetComponent<UnityEngine.UI.Image>().type = UnityEngine.UI.Image.Type.Sliced;
        scrollRect.name = "Scroll Rect";
        scrollRect.transform.SetParent(scrollView.transform);
        UnityEngine.UI.ScrollRect scrollRectUnity = scrollRect.AddComponent<UnityEngine.UI.ScrollRect>();
        scrollRectUnity.scrollSensitivity = 40;

        scrollRect.transform.SetParent(scrollRect.transform);
        scrollRect.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        scrollRect.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        scrollRect.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        scrollRect.GetComponent<RectTransform>().offsetMax = new Vector2(-20, 0);

        GameObject content = new GameObject("Content");
        RectTransform contentRect = content.AddComponent<RectTransform>();
        content.transform.SetParent(scrollRect.transform);
        content.transform.localPosition = Vector3.zero;
        contentRect.pivot = new Vector2(0, 1);
        contentRect.anchorMin = new Vector2(0, 1);
        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.sizeDelta = new Vector2(0, 400);
        contentRect.offsetMin = new Vector2(0, -400);
        contentRect.offsetMax = new Vector2(0, 0);
        content.transform.localScale = Vector3.one;

        scrollRectUnity.content = contentRect;
        scrollRectUnity.horizontal = false;
        EditorApplication.ExecuteMenuItem("GameObject/UI/Scrollbar");

        Selection.activeGameObject.transform.SetParent(scrollView.transform);
        scrollRectUnity.verticalScrollbar = Selection.activeGameObject.GetComponent<UnityEngine.UI.Scrollbar>();
        //scrollRectUnity.verticalScrollbar.direction = UnityEngine.UI.Scrollbar.Direction.BottomToTop;

        RectTransform scrollBar = scrollRectUnity.verticalScrollbar.GetComponent<RectTransform>();
        scrollBar.anchorMin = new Vector2(1, 0);
        scrollBar.anchorMax = new Vector2(1, 1);
        scrollRectUnity.verticalScrollbar.direction = UnityEngine.UI.Scrollbar.Direction.BottomToTop;
        scrollBar.sizeDelta = new Vector2(20, scrollBar.sizeDelta.y);
        scrollBar.offsetMin = new Vector2(-20, 0);
        scrollBar.offsetMax = new Vector2(0, 0);

        Selection.activeGameObject = scrollView;
    }

    public Rect ToolboxRect;
    private Vector2 ToolboxScrolling;

    public void CreateControlList(List<NativeControlData> nativeControls)
    {
        string menuPath = "GameObject/UI/";

        if (IsUIFrameworkEnabled(nativeControls, ref menuPath))
        {

        }
       // else
        {
            nativeControls.Add(new NativeControlData() { Display = "Container", CreateAction = CreateContainer });

            nativeControls.Add(new NativeControlData() { MenuPath = "GameObject/UI/Text", Display = "Text" });
            nativeControls.Add(new NativeControlData() { MenuPath = "GameObject/UI/Button", Display = "Button" });
            nativeControls.Add(new NativeControlData() { MenuPath = "GameObject/UI/Panel", Display = "Panel" });
            nativeControls.Add(new NativeControlData() { MenuPath = "GameObject/UI/Image", Display = "Image" });
            nativeControls.Add(new NativeControlData() { MenuPath = "GameObject/UI/Raw Image", Display = "Raw Image" });
            nativeControls.Add(new NativeControlData() { MenuPath = "GameObject/UI/Slider", Display = "Slider" });
            nativeControls.Add(new NativeControlData() { MenuPath = "GameObject/UI/Scrollbar", Display = "Scrollbar" });
            nativeControls.Add(new NativeControlData() { MenuPath = "GameObject/UI/Toggle", Display = "Toggle" });
            nativeControls.Add(new NativeControlData() { MenuPath = "GameObject/UI/Input Field", Display = "Input Field" });
            // nativeControls.Add(new NativeControlData() { MenuPath = "GameObject/UI/Canvas", Display = "Canvas" });
            nativeControls.Add(new NativeControlData() { Display = "Mask", CreateAction = CreateMask });
            nativeControls.Add(new NativeControlData() { Display = "Scroll View", CreateAction = CreateScrollView });
            nativeControls.Add(new NativeControlData() { Display = "Rich Text", CreateAction = CreateRichText });
            nativeControls.Add(new NativeControlData() { Display = "Horizonta Layout", CreateAction = CreateHorizontalLayout });
            nativeControls.Add(new NativeControlData() { Display = "Vertical Layout", CreateAction = CreateVerticalLayout });
            nativeControls.Add(new NativeControlData() { Display = "Grid Layout", CreateAction = CreateGridLayout });

#if UNITY_4_6 || UNITY_5_0 || UNITY_5_1

#else
            nativeControls.Add(new NativeControlData() { MenuPath = "GameObject/UI/Dropdown", Display = "Dropdown" });
#endif
        }

        for (int i = 0; i < UIEditorStartup.Prefabs.Count; ++i)
        {
            if (UIEditorStartup.Prefabs[i] == null)
            {
                UIEditorStartup.Prefabs.RemoveAt(i);
                i--;
                continue;
            }

            UIEditorLibraryControl libraryControl = UIEditorStartup.Prefabs[i].GetComponent<UIEditorLibraryControl>();
            if (libraryControl == null)
            {
                UIEditorStartup.Prefabs.RemoveAt(i);
                i--;
                continue;
            }

            nativeControls.Add(new NativeControlData()
            {
                Display = libraryControl.DisplayName,
                Prefab = libraryControl
            });

        }
    }

    List<NativeControlData> LoadedControls = new List<NativeControlData>();

    public const int ToolboxBaseX = 4;
    public const int ToolboxBaseY = 22;

    public const int ToolboxBoxWidth = 100;

    public void DrawToolboxControls()
    {
        if (!UIEditorVariables.ToolboxVisible) return;

        UIEditorStartup.LoadNewPrefabs();

        if (UIEditorLibraryControl.RequiresToolboxRebuild)
        {
            UIEditorLibraryControl.RequiresToolboxRebuild = false;
            UIEditorStartup.BuildPrefabControls();
        }

        LoadedControls.Clear();
        CreateControlList(LoadedControls);

        //if (IsTextMeshProEnabled())
        //{
        //    nativeControls.Add(new NativeControlData() { MenuPath = "GameObject/UI/TextMeshPro Text", Display = "TextMesh Pro" });
        //}


        {
            ToolboxRect = new Rect(ToolboxBaseX, ToolboxBaseY, ToolboxBoxWidth, Screen.height - 52);

            // ToolboxScrolling = GUI.BeginScrollView(new Rect(10, 20, 140, Screen.height - 80), ToolboxScrolling, new Rect(0, 0, 220, 200));
            GUIStyle boxSTyle = GUI.skin.box;
            GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.85f);
            Texture2D savedBackgroundTexture = boxSTyle.normal.background;
            boxSTyle.normal.background = Texture2D.whiteTexture;

            GUI.Box(ToolboxRect, string.Empty);

            boxSTyle.normal.background = savedBackgroundTexture;
            GUI.backgroundColor = Color.white;

            float startBoxY = ToolboxBaseY + 4;

            for (int i = 0; i < LoadedControls.Count; ++i)
            {
                if (LoadedControls[i].IsBreak)
                {
                    startBoxY += 4;

                }
                else
                {
                    Rect lastRect = new Rect(ToolboxBaseX + 0 + 4, startBoxY, ToolboxBoxWidth - 8, 24);
                    GUI.Box(lastRect, string.Empty, GUI.skin.box);

                    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                    {
                        if (lastRect.Contains(Event.current.mousePosition))
                        {
                            DragAndDrop.PrepareStartDrag();
                            DragAndDrop.objectReferences = new UnityEngine.Object[] { null };
                            DragAndDrop.SetGenericData("IsUIEditorControl", true);
                            DragAndDrop.SetGenericData("ControlType", LoadedControls[i].MenuPath);
                            DragAndDrop.SetGenericData("ControlAction", LoadedControls[i].CreateAction);
                            DragAndDrop.SetGenericData("Prefab", LoadedControls[i].Prefab);

                            DragAndDrop.StartDrag("Create a new control");
                            Event.current.Use();

                        }
                    }

                    //lastRect.x += 1;
                    lastRect.y += 3;

                    GUI.Label(lastRect, LoadedControls[i].Display);

                    startBoxY += 26;
                }
            }
        }
    }


    public void HandleToolboxDrags(UIEditorWindow window)
    {
        switch (Event.current.type)
        {
            case EventType.DragPerform:
                {
                    object genericData = DragAndDrop.GetGenericData("IsUIEditorControl");

                    if (genericData != null && !ToolboxRect.Contains(UIEditorInput.LastKnownMousePosition))
                    {
                        DragAndDrop.AcceptDrag();
                        string objectType = (string)DragAndDrop.GetGenericData("ControlType");
                        System.Action objectAction = (System.Action)DragAndDrop.GetGenericData("ControlAction");
                        UIEditorLibraryControl prefab = (UIEditorLibraryControl)DragAndDrop.GetGenericData("Prefab");

                        if (!string.IsNullOrEmpty(objectType))
                            EditorApplication.ExecuteMenuItem(objectType);
                        else if (prefab != null)
                        {
                            CreatePrefab(prefab);
                        }
                        else if (objectAction != null)
                            objectAction.Invoke();

                        Event.current.Use();

                        if (Selection.activeGameObject != null)
                            UIEditorHelpers.OnAfterCreateControl(Event.current.mousePosition, window);
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
                                    Sprite spriteData = AssetDatabase.LoadAssetAtPath(DragAndDrop.paths[i], typeof(Sprite)) as Sprite;
                                    if (spriteData != null)
                                    {
                                        EditorApplication.ExecuteMenuItem("GameObject/UI/Image");
                                        Selection.activeGameObject.GetComponent<UnityEngine.UI.Image>().sprite = spriteData;
                                        Selection.activeGameObject.GetComponent<UnityEngine.RectTransform>().sizeDelta = new Vector2(textureData.width, textureData.height);
                                        UIEditorHelpers.OnAfterCreateControl(Event.current.mousePosition, window);
                                        Selection.activeGameObject.name = spriteData.name;
                                    }
                                    else
                                    {
                                        EditorApplication.ExecuteMenuItem("GameObject/UI/Raw Image");
                                        Selection.activeGameObject.GetComponent<UnityEngine.UI.RawImage>().texture = textureData;
                                        Selection.activeGameObject.GetComponent<UnityEngine.RectTransform>().sizeDelta = new Vector2(textureData.width, textureData.height);
                                        UIEditorHelpers.OnAfterCreateControl(Event.current.mousePosition, window);
                                        Selection.activeGameObject.name = textureData.name;
                                    }
                                }
                            }

                            GameObject isRect = DragAndDrop.objectReferences[i] as GameObject;
                            if (isRect != null)
                            {
                                if (isRect.GetComponent<RectTransform>() != null)
                                {
                                    CreatePrefab(isRect);
                                    UIEditorHelpers.OnAfterCreateControl(Event.current.mousePosition, window);

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

