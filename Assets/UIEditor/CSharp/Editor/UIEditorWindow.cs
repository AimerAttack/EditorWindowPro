//-----------------------------------------------------------------------------------------
// UI Editor
// Copyright © Argiris Baltzis - All Rights Reserved
//
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
//-----------------------------------------------------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Callbacks;

public enum InputStateId
{
    None = 0,
    ResizingControl,
    DraggingSelectedControls,
    ScrollingCanvas,
    Scaling,
    Rotating,
    ListenForMouseDragOrMouseUp,
    SelectRegion,
}


[InitializeOnLoad]
public class UIEditorWindow : EditorWindow
{
    public class CanvasWithControlsGroupData
    {
        public Canvas Canvas;
        public List<ObjectData> AllControls = new List<ObjectData>(10000);
        public List<ObjectData> AllActiveControls = new List<ObjectData>(10000);
        public Dictionary<GameObject, ObjectData> GameObjectToObjectData = new Dictionary<GameObject, ObjectData>();

        public ObjectData GetObjectData(GameObject fromGameObject)
        {
            ObjectData returnValue = null;
            if (GameObjectToObjectData.TryGetValue(fromGameObject, out returnValue))
                return returnValue;

            return null;
        }
    }

    public class ObjectData
    {
        public Vector3[] Corners = new Vector3[4];
        public Rect Rect;
        public GameObject GameObject;
        public CanvasWithControlsGroupData CanvasGroupRef;
    }



    public List<ObjectData> HighlightedObjects = new List<ObjectData>();

    private UIRenderer uiRenderer;
    private static object CompilerChanged;
    public GameObject CanvasRenderCamera;
    private RenderTexture RenderTexture;
    private UIEditorContextMenu InterfaceContextMenu;
    public UIEditorInput Input;
    private List<CanvasWithControlsGroupData> AllCanvasInHierarchy = new List<CanvasWithControlsGroupData>();
    public UIEditorToolbox Toolbox = new UIEditorToolbox();
    public UIEditorToolbar Toolbar = new UIEditorToolbar();
    private static bool HasCompiled;
    public Rect ToolbarRect;
    
    public Vector3 GameViewRealMousePosition;
    public Canvas ActiveCanvas;

    public float InverseZoomScale
    {
        get
        {
            return 1 / UIEditorHelpers.GetZoomScaleFactor();
        }
    }

    public Vector3 ZoomScalePositionOffset
    {
        get
        {
            float deviceWidth = UIEditorVariables.DeviceWidth;
            float deviceHeight = UIEditorVariables.DeviceHeight;

            Vector2 offset = new Vector2(deviceWidth - (deviceWidth * UIEditorHelpers.GetZoomScaleFactor()), deviceHeight - (deviceHeight * UIEditorHelpers.GetZoomScaleFactor()));
            offset.x /= 2;
            offset.y /= 2;

            return offset;
        }
    }


    [MenuItem("Window/Interface")]
    static void ShowFromMenu()
    {
        EditorWindow.GetWindow(typeof(UIEditorWindow), false, "Interface", true);
    }

    void OnLostFocus()
    {
        if (Input != null) Input.InputState = InputStateId.None;
    }

    void OnFocus()
    {
        if (Input != null) Input.InputState = InputStateId.None;
    }

    public ObjectData GetObjectData(GameObject fromGameObject)
    {
        Canvas canvas = UIEditorHelpers.FindRootCanvas(fromGameObject);
        if (canvas != null)
        {
            for (int i = 0; i < AllCanvasInHierarchy.Count; ++i)
            {
                if (AllCanvasInHierarchy[i].Canvas == canvas)
                {
                    return AllCanvasInHierarchy[i].GetObjectData(fromGameObject);
                }
            }
        }

        return null;
    }

    private bool UseGameWindowSize
    {
        get
        {
            if (EditorApplication.isPlaying || EditorApplication.isPaused || EditorApplication.isPlayingOrWillChangePlaymode)
                return true;

            return false;
        }
    }

    private bool SkipPaint = false;
    void Update()
    {
        // skip 1 update to fix transform flickering
        if (!HasCompiled)
        {
            SkipPaint = true;
            HasCompiled = true;
            return;
        }

        UIEditorSceneSettings sceneSettings = GetSceneSettings();

        AllCanvasInHierarchy.Clear();
        IEnumerable<GameObject> rootObjects = SceneRoots();
        foreach (var v in rootObjects)
        {
            if (v == null) continue;
            Canvas scene = v.GetComponent<Canvas>();
            if (scene != null)
            {
                // change layer to UI
                if (scene.gameObject.layer == LayerMask.NameToLayer("Default"))
                    scene.gameObject.layer = LayerMask.NameToLayer("UI");

                if (scene.renderMode == RenderMode.WorldSpace)
                {

                }
                else if (scene.renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    CanvasWithControlsGroupData canvasGroup = new CanvasWithControlsGroupData() { Canvas = scene/*, ResetRenderMode = scene.renderMode, ResetWorldCamera = scene.worldCamera*/ };
                    AllCanvasInHierarchy.Add(canvasGroup);
                    BuildControlsList(canvasGroup, scene.transform);

                    // do not render this frame as there are transformation artifacts
                }
                else
                {
                    if (scene.worldCamera == null) continue;

                    CanvasWithControlsGroupData canvasGroup = new CanvasWithControlsGroupData() { Canvas = scene/*, ResetRenderMode = scene.renderMode, ResetWorldCamera = scene.worldCamera*/ };
                    AllCanvasInHierarchy.Add(canvasGroup);
                    BuildControlsList(canvasGroup, scene.transform);
                }
            }
        }

        // if (AllCanvasInHierarchy.Count > 0) activeCanvas = AllCanvasInHierarchy[0].Canvas;
        if (Selection.activeGameObject != null)
        {
            Canvas activeGameObjectCanvas = Selection.activeGameObject.GetComponentInParent<Canvas>();
            if (activeGameObjectCanvas != null) ActiveCanvas = activeGameObjectCanvas;
        }

        Initialize(AllCanvasInHierarchy, sceneSettings);

        RenderToRenderTexture(sceneSettings);


        Repaint();
    }


    // Update is called once per frame
    void RenderToRenderTexture(UIEditorSceneSettings sceneSettings)
    {
        CanvasRenderCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
        CanvasRenderCamera.GetComponent<Camera>().backgroundColor = sceneSettings.BackgroundColor;
        CanvasRenderCamera.GetComponent<Camera>().targetTexture = RenderTexture;

        CanvasRenderCamera.SetActive(true);
        if (sceneSettings.BackgroundRender == BackgroundRenderId.Scene && sceneSettings != null)
        {
            if (sceneSettings.BackgroundCamera == null)
            {
                CanvasRenderCamera.GetComponent<Camera>().Render();
            }
            else
            {
                bool switchOnUILayer = false;
                if (UIEditorHelpers.IsLayerOn(sceneSettings.BackgroundCamera.cullingMask, "UI"))
                {
                    sceneSettings.BackgroundCamera.cullingMask = UIEditorHelpers.SwitchLayerOff(sceneSettings.BackgroundCamera.cullingMask, "UI");
                    switchOnUILayer = true;
                }

                RenderTexture previous = sceneSettings.BackgroundCamera.targetTexture;
                Color previousColor = sceneSettings.BackgroundCamera.backgroundColor;
                sceneSettings.BackgroundCamera.targetTexture = RenderTexture;
                sceneSettings.BackgroundCamera.backgroundColor = new Color(sceneSettings.BackgroundCamera.backgroundColor.r, sceneSettings.BackgroundCamera.backgroundColor.g, sceneSettings.BackgroundCamera.backgroundColor.b, 1.0f);
                sceneSettings.BackgroundCamera.Render();
                sceneSettings.BackgroundCamera.targetTexture = previous;
                sceneSettings.BackgroundCamera.backgroundColor = previousColor;

                if (switchOnUILayer)
                {
                    sceneSettings.BackgroundCamera.cullingMask = UIEditorHelpers.SwitchLayerOn(sceneSettings.BackgroundCamera.cullingMask, "UI");
                }
            }
        }
        else
        {
            CanvasRenderCamera.GetComponent<Camera>().Render();
        }

        for (int i = 0; i < AllCanvasInHierarchy.Count; ++i)
        {
            if (AllCanvasInHierarchy[i].Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                AllCanvasInHierarchy[i].Canvas.worldCamera = CanvasRenderCamera.GetComponent<Camera>();
            }

            UnityEngine.RenderTexture RT = AllCanvasInHierarchy[i].Canvas.worldCamera.targetTexture;
            AllCanvasInHierarchy[i].Canvas.worldCamera.targetTexture = RenderTexture;
            int cullingMask = AllCanvasInHierarchy[i].Canvas.worldCamera.cullingMask;
            CameraClearFlags clearFags = AllCanvasInHierarchy[i].Canvas.worldCamera.clearFlags;
            Color clearColor = AllCanvasInHierarchy[i].Canvas.worldCamera.backgroundColor;
            RenderMode renderMode = AllCanvasInHierarchy[i].Canvas.renderMode;

            AllCanvasInHierarchy[i].Canvas.renderMode = RenderMode.ScreenSpaceCamera;
            AllCanvasInHierarchy[i].Canvas.worldCamera.backgroundColor = new Color(0, 0, 0, 0);
            AllCanvasInHierarchy[i].Canvas.worldCamera.clearFlags = CameraClearFlags.Depth;
            AllCanvasInHierarchy[i].Canvas.worldCamera.cullingMask = 1 << LayerMask.NameToLayer("UI");
            AllCanvasInHierarchy[i].Canvas.worldCamera.Render();

            for (int j = 0; j < AllCanvasInHierarchy[i].AllControls.Count; ++j)
            {
                AllCanvasInHierarchy[i].AllControls[j].Rect = UIEditorHelpers.GetScreenRect(
                    AllCanvasInHierarchy[i].AllControls[j].GameObject.GetComponent<RectTransform>(), AllCanvasInHierarchy[i].Canvas,
                    ref AllCanvasInHierarchy[i].AllControls[j].Corners);
            }


            AllCanvasInHierarchy[i].Canvas.worldCamera.targetTexture = RT;
            AllCanvasInHierarchy[i].Canvas.worldCamera.cullingMask = cullingMask;
            AllCanvasInHierarchy[i].Canvas.worldCamera.backgroundColor = clearColor;
            AllCanvasInHierarchy[i].Canvas.worldCamera.clearFlags = clearFags;
            AllCanvasInHierarchy[i].Canvas.renderMode = renderMode;

            if (renderMode == RenderMode.ScreenSpaceOverlay)
            {
                AllCanvasInHierarchy[i].Canvas.worldCamera = null;

            }
        }


        CanvasRenderCamera.SetActive(false);
        CanvasRenderCamera.GetComponent<Camera>().targetTexture = null;
    }

    private void Initialize(List<CanvasWithControlsGroupData> activeScenes, UIEditorSceneSettings sceneSettings)
    {
        if (RenderTexture != null)
        {
            int newWidth = UIEditorVariables.DeviceWidth;
            int newHeight = UIEditorVariables.DeviceHeight;

            if (newWidth != RenderTexture.width || newHeight != RenderTexture.height)
            {
                DestroyImmediate(RenderTexture);
                RenderTexture = null;
            }
        }

        bool recreatedTexture = false;
        if (RenderTexture == null)
        {
            int newWidth = UIEditorVariables.DeviceWidth;
            int newHeight = UIEditorVariables.DeviceHeight;

            RenderTexture = new RenderTexture(newWidth, newHeight, 24, RenderTextureFormat.ARGB32);
            RenderTexture.antiAliasing = 4;
            recreatedTexture = true;
        }

        if (CanvasRenderCamera == null || recreatedTexture)
        {
            if (CanvasRenderCamera != null) DestroyImmediate(CanvasRenderCamera);

            CanvasRenderCamera = new GameObject("UIEditorCamera");
            Camera camera = CanvasRenderCamera.AddComponent<Camera>();
            camera.cullingMask = 1 << LayerMask.NameToLayer("UI");
            camera.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0);
            camera.clearFlags = CameraClearFlags.SolidColor;
            CanvasRenderCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;

            // clear texture immediately
            CanvasRenderCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            CanvasRenderCamera.GetComponent<Camera>().backgroundColor = sceneSettings.BackgroundColor;
            CanvasRenderCamera.GetComponent<Camera>().targetTexture = RenderTexture;
            CanvasRenderCamera.GetComponent<Camera>().Render();
            CanvasRenderCamera.GetComponent<Camera>().targetTexture = null;
            CanvasRenderCamera.gameObject.SetActive(false);

        }

        if (uiRenderer == null)
        {
            uiRenderer = new UIRenderer();
        }

        if (Input == null)
        {
            Input = new UIEditorInput();
        }

        if (InterfaceContextMenu == null)
        {
            InterfaceContextMenu = new UIEditorContextMenu();
        }

    }

    private UIEditorSceneSettings mySceneSettings;
    public UIEditorSceneSettings GetSceneSettings()
    {
        if (mySceneSettings != null) return mySceneSettings;

        mySceneSettings = GameObject.FindObjectOfType<UIEditorSceneSettings>();
        if (mySceneSettings == null)
        {
            GameObject newGameObject = new GameObject("Settings");
            mySceneSettings = newGameObject.AddComponent<UIEditorSceneSettings>();
        }

        mySceneSettings.gameObject.hideFlags = HideFlags.None;


        return mySceneSettings;
    }

    public static IEnumerable<GameObject> SceneRoots()
    {
#if UNITY_5_3_OR_NEWER
        List<GameObject> allObjects = new List<GameObject>();
        UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects(allObjects);
        return allObjects;
#else
        var prop = new HierarchyProperty(HierarchyType.GameObjects);
        var expanded = new int[0];
        while (prop.Next(expanded))
        {
            yield return prop.pptrValue as GameObject;
        }
#endif
    }

    void BuildControlsList(CanvasWithControlsGroupData scene, Transform transformComponent)
    {
        RectTransform control = transformComponent.GetComponent<RectTransform>();
        if (control != null)
        {
            if (scene.Canvas.transform != transformComponent) // ignore root canvas, we dont need them
            {
                ObjectData newObjectData = new ObjectData() { GameObject = control.gameObject, CanvasGroupRef = scene };
                scene.AllControls.Add(newObjectData);

                scene.GameObjectToObjectData.Add(control.gameObject, newObjectData);

                if (transformComponent.gameObject.activeInHierarchy)
                {
                    scene.AllActiveControls.Add(newObjectData);
                }
            }

            for (int i = 0; i < transformComponent.childCount; ++i)
            {
                BuildControlsList(scene, transformComponent.GetChild(i));
            }
        }
    }

    void OnGUI()
    {
        UIEditorSceneSettings sceneSettings = GetSceneSettings();

        if (SkipPaint)
        {
            SkipPaint = false;
        }
        else
        {
            for (int i = 0; i < AllCanvasInHierarchy.Count; ++i)
            {
                for (int a = 0; a < AllCanvasInHierarchy[i].AllControls.Count; ++a)
                {
                    if (AllCanvasInHierarchy[i].AllControls[a].GameObject == null)
                    {
                        AllCanvasInHierarchy[i].AllActiveControls.Remove(AllCanvasInHierarchy[i].AllControls[a]);
                        AllCanvasInHierarchy[i].AllControls.RemoveAt(a);
                        a--;
                    }
                }
            }
            //if (EditorApplication.isPlaying || EditorApplication.isPaused || EditorApplication.isPlayingOrWillChangePlaymode) return;

            if (Input == null || InterfaceContextMenu == null || uiRenderer == null)
            {
                Initialize(AllCanvasInHierarchy, sceneSettings);
            }

            Input.OnGUI(this, AllCanvasInHierarchy);


            try
            {
                InterfaceContextMenu.OnGUI(this);
            }
            catch (System.Exception ex)
            {
                Debug.Log("UI Exception: " + ex.ToString());
            }

            try
            {
                Toolbox.HandleToolboxDrags(this);
            }
            catch (System.Exception ex)
            {
                Debug.Log("UI Exception: " + ex.ToString());
            }

            if (Event.current.type == EventType.Repaint)
            {
                try
                {
                    uiRenderer.PrepareDraw(position.width, position.height);
                    uiRenderer.DrawEditorGrid(position);
                    if (UIEditorVariables.ZoomIndex == UIEditorHelpers.DefaultZoomIndex()) RenderTexture.filterMode = FilterMode.Point;
                    else RenderTexture.filterMode = FilterMode.Bilinear;

                    uiRenderer.DrawRenderTexture(RenderTexture, UIEditorHelpers.GetZoomScaleFactor());
                    if (UIEditorVariables.GridVisible) uiRenderer.DrawGrid(this, ActiveCanvas);

                    uiRenderer.DrawVirtualWindow(UIEditorHelpers.GetZoomScaleFactor());

                    if (AllCanvasInHierarchy.Count > 0)
                    {
                        if (Event.current.control && UIEditorSelectionHelpers.TemporaryParentChange != null)
                        {
                            uiRenderer.DrawChangeParentHelpers(UIEditorSelectionHelpers.TemporaryParentChange, UIEditorHelpers.GetZoomScaleFactor());
                        }

                        if (UIEditorSelectionHelpers.MouseOverHighlightedObject != null)
                        {
                            uiRenderer.DrawHighlightedObjects(UIEditorSelectionHelpers.MouseOverHighlightedObject, UIEditorHelpers.GetZoomScaleFactor(), this);
                        }

                        uiRenderer.DrawSelectedEditorHelpers(UIEditorHelpers.GetZoomScaleFactor(), this);
                    }

                    uiRenderer.DrawSelectRegion(UIEditorHelpers.GetZoomScaleFactor(), this);

                }
                catch (System.Exception ex)
                {
                    Debug.Log("UI Exception: " + ex.ToString());
                }
            }
        }

        Toolbar.DrawToolbar(sceneSettings, this);
        Toolbox.DrawToolboxControls();

        if (Event.current.type == EventType.ScrollWheel)
        {
            int zoomDelta = UIEditorVariables.ZoomIndex;
            if (Event.current.delta.y < 0) zoomDelta++;
            else zoomDelta--;

            if (zoomDelta < 0) zoomDelta = 0;
            if (zoomDelta >= UIEditorHelpers.ZoomScales.Length) zoomDelta = UIEditorHelpers.ZoomScales.Length - 1;


            UIEditorVariables.ZoomIndex = zoomDelta;
        }
    }
}


