//-----------------------------------------------------------------------------------------
// UI Editor
// Copyright © Argiris Baltzis - All Rights Reserved
//
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
//-----------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;


public class UIEditorInput
{
    public static Vector3 LastKnownMousePosition;
    private Vector3 LastKnownGridMousePosition;

    private UIEditorEdgeMagnets edgeMagnetsDoNotUse;
    protected UIEditorEdgeMagnets EdgeMagnets
    {
        get
        {
            if (edgeMagnetsDoNotUse == null) edgeMagnetsDoNotUse = new UIEditorEdgeMagnets();
            return edgeMagnetsDoNotUse;
        }
    }

    //private EditorModeId LastKnownEditorMode;
    public InputStateId InputState;
    private bool InputStateRezingControlLeft;
    private bool InputStateRezingControlRight;
    private bool InputStateRezingControlTop;
    private bool InputStateRezingControlBottom;
    private Vector2 PositionAtStartDrag;
    private Vector2 GridPositionAtStartDrag;
    private bool IsAboutToDragMoveObjectsForFirstTime;
    private bool IsAboutToResizeObjectsForFirstTime;


    public Rect ScaleLeftRightHotRect;
    public Rect ScaleTopBottomHotRect;
    public Rect ScaleMiddleHotRect;
    public bool IsScalingHorizontal;
    public bool IsScalingVertical;
    public Rect MoveRightOnlyHotRect;
    public Rect MoveUpOnlyHotRect;
    public bool IsMovingHorizontal;
    public bool IsMovingVertical;

    public Vector2 RotateCenterSizeHotRect;
    public float RotateMinSizeHotRect;
    public float RotateMaxSizeHotRect;
    public bool IsRotating;

    private bool HadScrollWhilstInScrollingCanvasState;

    private Vector2 MousePositionAtMouseDown;
    private const float MinimumDragToSnapToMoveRotateScaleResize = 2;
    public Rect SelectionRegionRect;

    private bool LeftDownWasOverSelectedObject;

    public void OnGUI(UIEditorWindow window, List<UIEditorWindow.CanvasWithControlsGroupData> activeScenes)
    {
        //EditorModeId newMode = EditorModeId.Stoped;
        //if (EditorApplication.isPlaying) newMode = EditorModeId.Playin;
        //else if (EditorApplication.isPaused) newMode = EditorModeId.Paused;
        //if (newMode != LastKnownEditorMode)
        //{
        //    InputState = InputStateId.None;
        //    LastKnownEditorMode = newMode;
        //}


        UIEditorSelectionHelpers.MouseOverObject = null;
        UIEditorSelectionHelpers.MouseOverHighlightedObject = null;
        List<UIEditorWindow.ObjectData> mouseOverElements = new List<UIEditorWindow.ObjectData>();

        //UIEditorVariables.SceneScrolling = Vector2.zero;
        Vector2 screenCenterOffset = new Vector2(UIEditorVariables.DeviceWidth - (UIEditorVariables.DeviceWidth * UIEditorHelpers.GetZoomScaleFactor()), UIEditorVariables.DeviceHeight - (UIEditorVariables.DeviceHeight * UIEditorHelpers.GetZoomScaleFactor()));
        float canvasScaleFactor = 1.0f;
        float canvasScaleFactorInverse = 1.0f;

        if (window.ActiveCanvas != null)
        {
            canvasScaleFactor = window.ActiveCanvas.scaleFactor;
            canvasScaleFactorInverse = 1.0f / window.ActiveCanvas.scaleFactor;
        }

        Vector2 sceneScrollingVariable = UIEditorVariables.SceneScrolling;
        
        if (Event.current != null)
        {
            LastKnownMousePosition = Event.current.mousePosition;

            if (UIEditorVariables.GridSnap)
            {
                float gridSizeAdjusted = UIEditorVariables.GridSize * UIEditorHelpers.GetZoomScaleFactor() * canvasScaleFactor;

                LastKnownGridMousePosition.x = ((int)((LastKnownMousePosition.x - screenCenterOffset.x / 2) / gridSizeAdjusted)) * gridSizeAdjusted;
                LastKnownGridMousePosition.y = ((int)((LastKnownMousePosition.y - screenCenterOffset.y / 2) / gridSizeAdjusted)) * gridSizeAdjusted;
                LastKnownGridMousePosition.x = (float)System.Math.Round(LastKnownGridMousePosition.x, 4, System.MidpointRounding.ToEven);
                LastKnownGridMousePosition.y = (float)System.Math.Round(LastKnownGridMousePosition.y, 4, System.MidpointRounding.ToEven);
            }
            else
            {
                LastKnownGridMousePosition = LastKnownMousePosition;
            }


            Vector3 zoomScaleOffset = window.ZoomScalePositionOffset;
            float zoomScale = UIEditorHelpers.GetZoomScaleFactor();
          
            for (int a = 0; a < activeScenes.Count; ++a)
            {
                for (int i = 0; i < activeScenes[a].AllActiveControls.Count; ++i)
                {
                    FindObjectOnMousePosition(activeScenes[a].AllActiveControls[i], mouseOverElements, zoomScale, zoomScaleOffset, sceneScrollingVariable, activeScenes[a].Canvas);
                }
            }

            if (mouseOverElements.Count > 0)
            {
                if (!UIEditorSelectionHelpers.Selected.Contains(mouseOverElements[mouseOverElements.Count - 1].GameObject))
                {
                    UIEditorSelectionHelpers.MouseOverHighlightedObject = mouseOverElements[mouseOverElements.Count - 1];
                }
             
                UIEditorSelectionHelpers.MouseOverObject = mouseOverElements[mouseOverElements.Count - 1];
            }
        }
      
        HandleCommands();

        List<GameObject> selectedObjects = UIEditorSelectionHelpers.Selected;

        if (Event.current.type == EventType.KeyDown)
        {
            float amount = 1;
            if (Event.current.shift) amount = 10;


            if (Event.current.keyCode == KeyCode.G && Event.current.control)
            {
                UIEditorVariables.DisableAllSnap = !UIEditorVariables.DisableAllSnap;
            }

                if (Event.current.keyCode == KeyCode.Plus || Event.current.keyCode == KeyCode.KeypadPlus)
            {
                int selectedIndex = UIEditorVariables.ZoomIndex + 1;
                if (selectedIndex >= UIEditorHelpers.ZoomScales.Length) selectedIndex = UIEditorHelpers.ZoomScales.Length - 1;
                UIEditorVariables.ZoomIndex = selectedIndex;
            }

            else if (Event.current.keyCode == KeyCode.Minus || Event.current.keyCode == KeyCode.KeypadMinus)
            {
                int selectedIndex = UIEditorVariables.ZoomIndex - 1;
                if (selectedIndex <= 0) selectedIndex = 0;
                UIEditorVariables.ZoomIndex = selectedIndex;
            }

            else if (Event.current.keyCode == KeyCode.UpArrow)
            {
                for (int a = 0; a < selectedObjects.Count; ++a)
                {
                    if (UIEditorHelpers.IsParentInList(selectedObjects, selectedObjects[a]))
                        continue;

                    if (Tools.current == Tool.Move)
                    {
                        MoveObject(selectedObjects[a], new Vector2(0, -amount), window, activeScenes, false);
                    }
                    else if (Tools.current == Tool.Rotate)
                    {
                        selectedObjects[a].transform.localRotation = Quaternion.Euler(selectedObjects[a].transform.localRotation.eulerAngles + new Vector3(1, 0, 0));
                    }
                    else if (Tools.current == Tool.Scale)
                    {
                        selectedObjects[a].transform.localScale += new Vector3(0, 0.1f, 0);
                    }
                    else if (Tools.current == Tool.Rect)
                    {
                        selectedObjects[a].transform.GetComponent<RectTransform>().offsetMax += new Vector2(0, 1);

                    }
                }
            }
            else if (Event.current.keyCode == KeyCode.DownArrow)
            {
                for (int a = 0; a < selectedObjects.Count; ++a)
                {
                    if (UIEditorHelpers.IsParentInList(selectedObjects, selectedObjects[a]))
                        continue;

                    if (Tools.current == Tool.Move)
                    {
                        MoveObject(selectedObjects[a], new Vector2(0, amount), window, activeScenes, false);
                    }
                    else if (Tools.current == Tool.Rotate)
                    {
                        selectedObjects[a].transform.localRotation = Quaternion.Euler(selectedObjects[a].transform.localRotation.eulerAngles + new Vector3(-1, 0, 0));

                    }
                    else if (Tools.current == Tool.Scale)
                    {
                        selectedObjects[a].transform.localScale += new Vector3(0, -0.1f, 0);

                    }
                    else if (Tools.current == Tool.Rect)
                    {
                        selectedObjects[a].transform.GetComponent<RectTransform>().offsetMin += new Vector2(0, -1);

                    }
                }
            }

            else if (Event.current.keyCode == KeyCode.LeftArrow)
            {
                for (int a = 0; a < selectedObjects.Count; ++a)
                {
                    if (UIEditorHelpers.IsParentInList(selectedObjects, selectedObjects[a]))
                        continue;

                    if (Tools.current == Tool.Move)
                    {
                        MoveObject(selectedObjects[a], new Vector2(-amount, 0), window, activeScenes, false);
                    }
                    else if (Tools.current == Tool.Rotate)
                    {
                        selectedObjects[a].transform.localRotation = Quaternion.Euler(selectedObjects[a].transform.localRotation.eulerAngles + new Vector3(0, 0, 1));

                    }
                    else if (Tools.current == Tool.Scale)
                    {
                        selectedObjects[a].transform.localScale += new Vector3(-0.1f, 0, 0);

                    }
                    else if (Tools.current == Tool.Rect)
                    {
                        selectedObjects[a].transform.GetComponent<RectTransform>().offsetMin += new Vector2(-1, 0);

                    }
                }
            }
            else if (Event.current.keyCode == KeyCode.RightArrow)
            {
                for (int a = 0; a < selectedObjects.Count; ++a)
                {
                    if (UIEditorHelpers.IsParentInList(selectedObjects, selectedObjects[a]))
                        continue;

                    if (Tools.current == Tool.Move)
                    {
                        MoveObject(selectedObjects[a], new Vector2(amount, 0), window, activeScenes, false);
                    }
                    else if (Tools.current == Tool.Rotate)
                    {
                        selectedObjects[a].transform.localRotation = Quaternion.Euler(selectedObjects[a].transform.localRotation.eulerAngles + new Vector3(0, 0, -1));

                    }
                    else if (Tools.current == Tool.Scale)
                    {
                        selectedObjects[a].transform.localScale += new Vector3(0.1f, 0, 0);

                    }
                    else if (Tools.current == Tool.Rect)
                    {
                        selectedObjects[a].transform.GetComponent<RectTransform>().offsetMax += new Vector2(1, 0);

                    }
                }
            }
        }

        if (!Event.current.isMouse)
        {
            if (Event.current.rawType == EventType.MouseUp)
            {
                InputState = InputStateId.None;
            }
        }
        else
        {
            CheckInputStateAgain:
            switch (InputState)
            {
                case InputStateId.None:
                    {
                        if (window.Toolbox.ToolboxRect.Contains(LastKnownMousePosition) || window.ToolbarRect.Contains(LastKnownMousePosition))
                        {
                            // ignore events if we hit the toolboxrect
                        }

                        // LEFT BUTTON
                        else if (Event.current.button == 0 && Event.current.type == EventType.MouseUp)
                        {

                        }
                        else if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                        {
                            LeftDownWasOverSelectedObject = false;

                            if (Tools.current == Tool.View)
                            {
                                HadScrollWhilstInScrollingCanvasState = false;
                                InputState = InputStateId.ScrollingCanvas;
                                PositionAtStartDrag.x = LastKnownMousePosition.x;
                                PositionAtStartDrag.y = LastKnownMousePosition.y;
                                GridPositionAtStartDrag = new Vector2(LastKnownGridMousePosition.x, LastKnownGridMousePosition.y);
                                Event.current.Use();
                            }
                            else if (window.Toolbox.ToolboxRect.Contains(LastKnownMousePosition))
                            {
                                // ignore events if we hit the toolboxrect
                            }
                            else
                            {
                                MousePositionAtMouseDown = LastKnownMousePosition;
                                InputState = InputStateId.ListenForMouseDragOrMouseUp;
                                EdgeMagnets.Clear();

                                for (int i = 0; i < selectedObjects.Count; ++i)
                                {
                                    UIEditorWindow.ObjectData objData = window.GetObjectData(selectedObjects[i]);
                                    if (objData != null)
                                    {
                                        if (mouseOverElements.Contains(objData))
                                        {
                                            LeftDownWasOverSelectedObject = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        // MIDDLE BUTTON
                        else if (Event.current.button == 2 || Event.current.button == 1)
                        {
                            if (Event.current.type == EventType.MouseUp)
                            {

                            }
                            else if (Event.current.type == EventType.MouseDown)
                            {
                                HadScrollWhilstInScrollingCanvasState = false;
                                InputState = InputStateId.ScrollingCanvas;
                                PositionAtStartDrag.x = LastKnownMousePosition.x;
                                PositionAtStartDrag.y = LastKnownMousePosition.y;
                                GridPositionAtStartDrag = new Vector2(LastKnownGridMousePosition.x, LastKnownGridMousePosition.y);
                                Event.current.Use();

                            }
                        }
                        else
                        {
                            // no buttons or anything, just mouse
                        }

                        break;
                    }

                case InputStateId.SelectRegion:
                    {
                        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                        {
                            InputState = InputStateId.None;
                            goto CheckInputStateAgain;
                        }

                        SelectionRegionRect = new Rect(
                                Mathf.Min(LastKnownMousePosition.x, MousePositionAtMouseDown.x),
                                Mathf.Min(LastKnownMousePosition.y, MousePositionAtMouseDown.y),
                                Mathf.Max(LastKnownMousePosition.x, MousePositionAtMouseDown.x) - Mathf.Min(LastKnownMousePosition.x, MousePositionAtMouseDown.x),
                                Mathf.Max(LastKnownMousePosition.y, MousePositionAtMouseDown.y) - Mathf.Min(LastKnownMousePosition.y, MousePositionAtMouseDown.y));


                        List<GameObject> objectsInisideRegion = new List<GameObject>();

                        for (int a = 0; a < activeScenes.Count; ++a)
                        {
                            for (int i = 0; i < activeScenes[a].AllActiveControls.Count; ++i)
                            {
                                IsElementInsideRegion(
                                    activeScenes[a].AllActiveControls[i],
                                    SelectionRegionRect,
                                    objectsInisideRegion,
                                    UIEditorHelpers.GetZoomScaleFactor(),
                                    window.ZoomScalePositionOffset,
                                    sceneScrollingVariable,
                                    activeScenes[a].Canvas);
                            }
                        }

                        Selection.objects = objectsInisideRegion.ToArray();

                        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                        {
                            InputState = InputStateId.None;
                        }
                        else if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
                        {
                            InputState = InputStateId.None;
                        }

                        break;
                    }

                case InputStateId.ListenForMouseDragOrMouseUp:
                    {
                        if (Event.current.button == 0)
                        {
                            if (Event.current.type == EventType.MouseDown)
                            {
                                InputState = InputStateId.None;
                                goto CheckInputStateAgain;
                            }

                            float distanceTravel = Vector2.Distance(MousePositionAtMouseDown, new Vector2(LastKnownMousePosition.x, LastKnownMousePosition.y));

                            float breakDistance = 15;

                            if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
                            {
                                // DO SELECTION STUFF ON MOUSE UP
                                if (mouseOverElements.Count > 0)
                                {
                                    GameObject topObject = mouseOverElements[mouseOverElements.Count - 1].GameObject;
                                    if (Event.current.control)
                                    {
                                        if (UIEditorSelectionHelpers.SelectionContains(topObject.gameObject))
                                            UIEditorSelectionHelpers.RemoveFromSelection(topObject.gameObject);
                                        else
                                            UIEditorSelectionHelpers.AddToSelection(topObject.gameObject);

                                    }
                                    else
                                    {
                                        UIEditorSelectionHelpers.Select(topObject);
                                    }
                                }
                                else
                                {
                                    UIEditorSelectionHelpers.ClearAllSelected();
                                }

                                InputState = InputStateId.None;
                                break;
                            }

                            if (distanceTravel >= MinimumDragToSnapToMoveRotateScaleResize)
                            {
                                if (!IsMouseOverMoveRotateScaleHotRect(UIEditorHelpers.GetZoomScaleFactor(), window.ZoomScalePositionOffset))
                                {
                                    // Detect control resize
                                    for (int a = 0; a < selectedObjects.Count; ++a)
                                    {
                                        UIEditorWindow.ObjectData objectData = window.GetObjectData(selectedObjects[a]);
                                        if (objectData == null) continue;

                                        List<Rect> corners = UIEditorHelpers.Get8ScaleBoxesAroundObject(objectData, UIEditorHelpers.GetZoomScaleFactor(), window.ZoomScalePositionOffset, true);
                                        //for (int b = 0; b < corners.Count; b++)
                                       //     corners[b] = UIEditorHelpers.TransformRectToScreenPixels(corners[b], UIEditorHelpers.GetZoomScaleFactor(), window.ZoomScalePositionOffset);

                                        //Rect resizedRectTop, resizedRectBottom, resizedRectLeft, resizedRectRight;
                                        //UIEditorHelpers.CreateBoundingRectWithInsideAndOutsideBorder(out resizedRectTop, out resizedRectBottom, out resizedRectLeft, out resizedRectRight, 6, objectData.Rect);

                                        //UIEditorHelpers.TransformBoundingGrabRects(ref resizedRectTop, ref resizedRectBottom, ref resizedRectLeft, ref resizedRectRight, UIEditorHelpers.GetZoomScaleFactor(), window.ZoomScalePositionOffset);

                                        bool hadResize = false;
                                        InputStateRezingControlTop = false;
                                        InputStateRezingControlBottom = false;
                                        InputStateRezingControlLeft = false;
                                        InputStateRezingControlRight = false;

                                        // top
                                        if (corners[0].Contains(MousePositionAtMouseDown) || corners[1].Contains(MousePositionAtMouseDown) || corners[2].Contains(MousePositionAtMouseDown))
                                        {
                                            hadResize = true;
                                            InputStateRezingControlTop = true;
                                        }

                                        // bottom
                                        if (corners[5].Contains(MousePositionAtMouseDown) || corners[6].Contains(MousePositionAtMouseDown) || corners[7].Contains(MousePositionAtMouseDown))
                                        {
                                            hadResize = true;
                                            InputStateRezingControlBottom = true;
                                        }

                                        //  left
                                        if (corners[0].Contains(MousePositionAtMouseDown) || corners[3].Contains(MousePositionAtMouseDown) || corners[5].Contains(MousePositionAtMouseDown))
                                        {
                                            hadResize = true;
                                            InputStateRezingControlLeft = true;
                                        }

                                        // right
                                        if (corners[2].Contains(MousePositionAtMouseDown) || corners[4].Contains(MousePositionAtMouseDown) || corners[7].Contains(MousePositionAtMouseDown))
                                        {
                                            hadResize = true;
                                            InputStateRezingControlRight = true;
                                        }


                                        if (hadResize)
                                        {
                                            PositionAtStartDrag.x = LastKnownMousePosition.x;
                                            PositionAtStartDrag.y = LastKnownMousePosition.y;
                                            GridPositionAtStartDrag = new Vector2(LastKnownGridMousePosition.x, LastKnownGridMousePosition.y);
                                            InputState = InputStateId.ResizingControl;
                                            Event.current.Use();
                                            IsAboutToResizeObjectsForFirstTime = true;

                                            break;
                                        }
                                    }
                                }

                                if (InputState == InputStateId.ResizingControl)
                                {
                                    // if we detected a resize then dont do anything else
                                }
                                else if (Tools.current == Tool.Rect)
                                {

                                }

                                // begin scale
                                else if (Tools.current == Tool.Scale)
                                {
                                    IsScalingHorizontal = IsMouseInsideWorldRect(ScaleLeftRightHotRect, UIEditorHelpers.GetZoomScaleFactor(), window.ZoomScalePositionOffset, UIEditorVariables.SceneScrolling);
                                    IsScalingVertical = IsMouseInsideWorldRect(ScaleTopBottomHotRect, UIEditorHelpers.GetZoomScaleFactor(), window.ZoomScalePositionOffset, UIEditorVariables.SceneScrolling);

                                    if (IsMouseInsideWorldRect(ScaleMiddleHotRect, UIEditorHelpers.GetZoomScaleFactor(), window.ZoomScalePositionOffset, UIEditorVariables.SceneScrolling))
                                    {
                                        IsScalingVertical = true;
                                        IsScalingHorizontal = true;
                                    }

                                    bool isMouseInsideBoundingBox = false;

                                    for (int i = 0; i < selectedObjects.Count; ++i)
                                    {
                                        UIEditorWindow.ObjectData objData = window.GetObjectData(selectedObjects[i]);
                                        if (objData != null)
                                        {
                                            if (mouseOverElements.Contains(objData))
                                            {
                                                isMouseInsideBoundingBox = true;
                                                if (!IsScalingHorizontal && !IsScalingVertical)
                                                {
                                                    IsScalingHorizontal = true;
                                                    IsScalingVertical = true;
                                                }
                                                break;
                                            }
                                        }
                                    }

                                    if (isMouseInsideBoundingBox || IsScalingHorizontal || IsScalingVertical)
                                    {
                                        PositionAtStartDrag.x = LastKnownMousePosition.x;
                                        PositionAtStartDrag.y = LastKnownMousePosition.y;
                                        GridPositionAtStartDrag = new Vector2(LastKnownGridMousePosition.x, LastKnownGridMousePosition.y);
                                        InputState = InputStateId.Scaling;
                                        IsAboutToDragMoveObjectsForFirstTime = true;
                                    }
                                }

                                // begin rotate
                                else if (Tools.current == Tool.Rotate)
                                {
                                    IsRotating = IsMouseInsideCircle(RotateCenterSizeHotRect, RotateMinSizeHotRect, RotateMaxSizeHotRect);

                                    for (int i = 0; i < selectedObjects.Count; ++i)
                                    {
                                        UIEditorWindow.ObjectData objData = window.GetObjectData(selectedObjects[i]);
                                        if (objData != null)
                                        {
                                            if (mouseOverElements.Contains(objData))
                                            {
                                                IsRotating = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (IsRotating)
                                    {
                                        PositionAtStartDrag.x = LastKnownMousePosition.x;
                                        PositionAtStartDrag.y = LastKnownMousePosition.y;
                                        GridPositionAtStartDrag = new Vector2(LastKnownGridMousePosition.x, LastKnownGridMousePosition.y);
                                        InputState = InputStateId.Rotating;
                                        IsAboutToDragMoveObjectsForFirstTime = true;
                                    }
                                }

                                // begin move
                                else
                                {
                                    IsMovingHorizontal = IsMouseInsideWorldRect(MoveRightOnlyHotRect, UIEditorHelpers.GetZoomScaleFactor(), window.ZoomScalePositionOffset, UIEditorVariables.SceneScrolling);
                                    IsMovingVertical = IsMouseInsideWorldRect(MoveUpOnlyHotRect, UIEditorHelpers.GetZoomScaleFactor(), window.ZoomScalePositionOffset, UIEditorVariables.SceneScrolling);
                                    bool isMouseInsideBoundingBox = false;

                                    for (int i = 0; i < selectedObjects.Count; ++i)
                                    {
                                        UIEditorWindow.ObjectData objData = window.GetObjectData(selectedObjects[i]);
                                        if (objData != null)
                                        {
                                            if (mouseOverElements.Contains(objData))
                                            {
                                                isMouseInsideBoundingBox = true;
                                                if (!IsMovingHorizontal && !IsMovingVertical)
                                                {
                                                    IsMovingHorizontal = true;
                                                    IsMovingVertical = true;
                                                }
                                                break;
                                            }
                                        }
                                    }

                                    if (isMouseInsideBoundingBox || IsMovingHorizontal || IsMovingVertical)
                                    {
                                        PositionAtStartDrag.x = LastKnownMousePosition.x;
                                        PositionAtStartDrag.y = LastKnownMousePosition.y;
                                        GridPositionAtStartDrag = new Vector2(LastKnownGridMousePosition.x, LastKnownGridMousePosition.y);
                                        InputState = InputStateId.DraggingSelectedControls;
                                        IsAboutToDragMoveObjectsForFirstTime = true;
                                    }
                                }

                                if (!LeftDownWasOverSelectedObject && distanceTravel > breakDistance)
                                {
                                    if (InputState == InputStateId.ListenForMouseDragOrMouseUp)
                                    {
                                        InputState = InputStateId.SelectRegion;

                                        SelectionRegionRect = new Rect(
                                       Mathf.Min(LastKnownMousePosition.x, MousePositionAtMouseDown.x),
                                       Mathf.Min(LastKnownMousePosition.y, MousePositionAtMouseDown.y),
                                       Mathf.Max(LastKnownMousePosition.x, MousePositionAtMouseDown.x) - Mathf.Min(LastKnownMousePosition.x, MousePositionAtMouseDown.x),
                                       Mathf.Max(LastKnownMousePosition.y, MousePositionAtMouseDown.y) - Mathf.Min(LastKnownMousePosition.y, MousePositionAtMouseDown.y));
                                    }
                                    else
                                    {
                                        InputState = InputStateId.None;
                                    }
                                }
                            }
                        }

                        break;
                    }

                case InputStateId.Scaling:
                    {
                        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                        {
                            InputState = InputStateId.None;
                            goto CheckInputStateAgain;
                        }

                        if ((selectedObjects.Count == 0 || Event.current.button != 0) || (Event.current.button == 0 && Event.current.type == EventType.MouseUp))
                        {
                            IsScalingHorizontal = false;
                            IsScalingVertical = false;
                            InputState = InputStateId.None;
                            break;
                        }

                        float widthResizeAmount = (LastKnownGridMousePosition.x - GridPositionAtStartDrag.x) * 0.01f;
                        float heightResizeAmount = (LastKnownGridMousePosition.y - GridPositionAtStartDrag.y) * 0.01f;

                        widthResizeAmount *= window.InverseZoomScale * canvasScaleFactorInverse;
                        heightResizeAmount *= window.InverseZoomScale * canvasScaleFactorInverse;


                        if (!IsScalingHorizontal) widthResizeAmount = 0;
                        if (!IsScalingVertical) heightResizeAmount = 0;

                        for (int i = 0; i < selectedObjects.Count; ++i)
                        {
                            if (UIEditorHelpers.IsParentInList(selectedObjects, selectedObjects[i]))
                                continue;

                            UIEditorWindow.ObjectData objData = window.GetObjectData(selectedObjects[i]);
                            if (objData != null)
                            {
                                UnityEditor.Undo.RecordObject(selectedObjects[i].GetComponent<RectTransform>(), "Scale");

                                //float scaleWidth = objData.GameObject.GetComponent<RectTransform>().offsetMax.x - objData.GameObject.GetComponent<RectTransform>().offsetMin.x;
                                //float newScaleWidth = scaleWidth + widthResizeAmount;
                                //Debug.Log(scaleWidth.ToString() + "_" + newScaleWidth.ToString());

                                //float finalScale = 1 - (newScaleWidth / scaleWidth);
                                //Debug.Log("finalscale:" + finalScale.ToString());

                                Vector3 scale = objData.GameObject.transform.localScale;
                                scale += new Vector3(widthResizeAmount, -heightResizeAmount, 0);
                                if (scale.x <= 0) scale.x = 0;
                                if (scale.y <= 0) scale.y = 0;
                                objData.GameObject.transform.localScale = scale;
                                //Debug.Log(objData.GameObject.transform.localScale.ToString());
                            }

                            UIEditorHelpers.SetDirtyAllComponentsAndGameObject(selectedObjects[i]);
                        }

                        PositionAtStartDrag.x = LastKnownMousePosition.x;
                        PositionAtStartDrag.y = LastKnownMousePosition.y;
                        GridPositionAtStartDrag = new Vector2(LastKnownGridMousePosition.x, LastKnownGridMousePosition.y);

                        break;
                    }


                case InputStateId.Rotating:
                    {
                        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                        {
                            InputState = InputStateId.None;
                            goto CheckInputStateAgain;
                        }

                        if ((selectedObjects.Count == 0 || Event.current.button != 0) || (Event.current.button == 0 && Event.current.type == EventType.MouseUp))
                        {
                            IsRotating = false;
                            InputState = InputStateId.None;
                            break;
                        }

                        float horizontalAmount = (LastKnownGridMousePosition.x - GridPositionAtStartDrag.x);
                        float verticalAmount = (LastKnownGridMousePosition.y - GridPositionAtStartDrag.y);

                        horizontalAmount *= window.InverseZoomScale * canvasScaleFactorInverse;
                        verticalAmount *= window.InverseZoomScale * canvasScaleFactorInverse;

                        for (int i = 0; i < selectedObjects.Count; ++i)
                        {
                            if (UIEditorHelpers.IsParentInList(selectedObjects, selectedObjects[i]))
                                continue;

                            UIEditorWindow.ObjectData objData = window.GetObjectData(selectedObjects[i]);
                            if (objData != null)
                            {
                                Vector3 euler = objData.GameObject.transform.localRotation.eulerAngles;
                                euler += new Vector3(0, 0, -verticalAmount + horizontalAmount);
                                objData.GameObject.transform.localRotation = Quaternion.Euler(euler);
                            }

                            UIEditorHelpers.SetDirtyAllComponentsAndGameObject(selectedObjects[i]);

                        }

                        PositionAtStartDrag.x = LastKnownMousePosition.x;
                        PositionAtStartDrag.y = LastKnownMousePosition.y;
                        GridPositionAtStartDrag = new Vector2(LastKnownGridMousePosition.x, LastKnownGridMousePosition.y);

                        break;
                    }

                case InputStateId.ResizingControl:
                    {
                        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                        {
                            InputState = InputStateId.None;
                            goto CheckInputStateAgain;
                        }

                        if ((selectedObjects.Count == 0 || Event.current.button != 0) || (Event.current.button == 0 && Event.current.type == EventType.MouseUp))
                        {
                            InputState = InputStateId.None;
                            break;
                        }

                        if (IsAboutToResizeObjectsForFirstTime)
                        {
                            IsAboutToResizeObjectsForFirstTime = false;
                        }

                        float widthResizeAmount = (LastKnownGridMousePosition.x - GridPositionAtStartDrag.x);
                        float heightResizeAmount = (LastKnownGridMousePosition.y - GridPositionAtStartDrag.y);

                        if (Event.current.control)
                        {
                            float savedHeight = heightResizeAmount;
                            float savedWidht = widthResizeAmount;

                            if ((InputStateRezingControlBottom && InputStateRezingControlRight) || (InputStateRezingControlLeft && InputStateRezingControlTop))
                            {
                                heightResizeAmount =  (savedWidht + savedHeight) / 2;
                                widthResizeAmount =  (savedWidht + savedHeight) / 2;
                            }
                            else
                            {
                                heightResizeAmount = (-savedWidht + savedHeight) / 2;
                                widthResizeAmount = (savedWidht - savedHeight) / 2;
                            }
                        }

                        if (UIEditorVariables.GridSnap)
                        {
                            float gridSizeAdjusted = UIEditorVariables.GridSize * UIEditorHelpers.GetZoomScaleFactor() * canvasScaleFactor;
                            widthResizeAmount = (((int)(System.Math.Round(widthResizeAmount / gridSizeAdjusted, 4, System.MidpointRounding.ToEven))) * gridSizeAdjusted);
                            heightResizeAmount = (((int)(System.Math.Round(heightResizeAmount / gridSizeAdjusted, 4, System.MidpointRounding.ToEven))) * gridSizeAdjusted);

                            if (widthResizeAmount != 0)
                            {
                                PositionAtStartDrag.x = LastKnownMousePosition.x;
                                GridPositionAtStartDrag.x = LastKnownGridMousePosition.x;
                            }

                            if (heightResizeAmount != 0)
                            {
                                PositionAtStartDrag.y = LastKnownMousePosition.y;
                                GridPositionAtStartDrag.y = LastKnownGridMousePosition.y;
                            }
                        }
                        else
                        {
                            PositionAtStartDrag.x = LastKnownMousePosition.x;
                            PositionAtStartDrag.y = LastKnownMousePosition.y;
                            GridPositionAtStartDrag = new Vector2(LastKnownGridMousePosition.x, LastKnownGridMousePosition.y);
                        }


                        for (int a = 0; a < selectedObjects.Count; ++a)
                        {
                            RectTransform rectTransform = selectedObjects[a].GetComponent<RectTransform>();
                            UnityEditor.Undo.RecordObject(rectTransform, "Resize");

                            float localWidthResizeAmount = widthResizeAmount * (1 / rectTransform.localScale.x);
                            float localHeightResizeAmount = heightResizeAmount * (1 / rectTransform.localScale.y);

                        
                            bool allowResizeHeight = true;
                            bool allowResizeWidth = true;


                           // bool isStretchedHorizontally = rectTransform.anchorMin.x != rectTransform.anchorMax.x;
                            //bool isStretchedVertically = rectTransform.anchorMin.y != rectTransform.anchorMax.y;
                           // bool isMidHorizontally = rectTransform.pivot.x == 0.5f;
                           // bool isMidVertically = rectTransform.pivot.y == 0.5f;


                          //  if (isMidHorizontally) localWidthResizeAmount *= 2;
                         //   if(isMidVertically) localHeightResizeAmount *= 2;

                            localWidthResizeAmount *= window.InverseZoomScale * canvasScaleFactorInverse;
                            localHeightResizeAmount *= window.InverseZoomScale * canvasScaleFactorInverse;

                            if (InputStateRezingControlTop && allowResizeHeight)
                            {
                                // rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 20, 100);
                                // rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.sizeDelta.y - localHeightResizeAmount);
                                //if (isMidVertically)
                                //{
                                //    rectTransform.sizeDelta -= new Vector2(0, localHeightResizeAmount);
                                //}
                                //else
                                {
                                    rectTransform.offsetMax -= new Vector2(0, localHeightResizeAmount);
                                    //  if (!isStretchedVertically)
                                    //      if (offsetMax.y <= offsetMin.y) offsetMax.y = offsetMin.y;

                                }
                            }

                            if (InputStateRezingControlBottom && allowResizeHeight)
                            {
                            //    if (isMidVertically)
                            //    {
                            //        rectTransform.sizeDelta += new Vector2(0, localHeightResizeAmount);
                            //    }
                            //    else
                                {
                                    rectTransform.offsetMin -= new Vector2(0, localHeightResizeAmount);
                                   // if (!isStretchedVertically)
                                   //     if (offsetMin.y >= offsetMax.y) offsetMin.y = offsetMax.y;
                                }
                            }

                            if (InputStateRezingControlLeft && allowResizeWidth)
                            {
                                //if (isMidHorizontally)
                                //{
                                //    rectTransform.sizeDelta -= new Vector2(localWidthResizeAmount, 0);
                                //}
                                //else
                                {
                                    rectTransform.offsetMin += new Vector2(localWidthResizeAmount, 0);
                                    //rectTransform.offsetMax += new Vector2(localWidthResizeAmount, 0);
                                    //if (!isStretchedHorizontally)
                                    //    if (offsetMin.x >= offsetMax.x) offsetMin.x = offsetMax.x;
                                }

                            }

                            if (InputStateRezingControlRight && allowResizeWidth)
                            {
                                //if (isMidHorizontally)
                                //{
                                //    rectTransform.sizeDelta += new Vector2(localWidthResizeAmount, 0);
                                //}
                                //else
                                {
                                    rectTransform.offsetMax += new Vector2(localWidthResizeAmount, 0);

                                   
                                   // if (!isStretchedHorizontally)
                                   //     if (offsetMax.x <= offsetMin.x) offsetMax.x = offsetMin.x;
                                }
                            }


                            //UIEditorHelpers.SetDirtyAllComponentsAndGameObject(selectedObjects[a]);
                        }

                        break;
                    }

                case InputStateId.DraggingSelectedControls:
                    {
                        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                        {
                            InputState = InputStateId.None;
                            goto CheckInputStateAgain;
                        }

                        // exit state
                        if (selectedObjects.Count == 0 || (Event.current.button == 0 && Event.current.type == EventType.MouseUp))
                        {
                            IsMovingHorizontal = false;
                            IsMovingVertical = false;
                            UIEditorSelectionHelpers.TemporaryParentChange = null;
                            InputState = InputStateId.None;
                            break;
                        }

                        float gridSizeAdjusted = UIEditorVariables.GridSize * UIEditorHelpers.GetZoomScaleFactor() * canvasScaleFactor;

                        // change parent of control
                        if (Event.current.control && selectedObjects.Count > 0)
                        {
                            for (int i = 0; i < selectedObjects.Count; ++i)
                            {
                                Transform newParent = selectedObjects[i].GetComponentInParent<Canvas>().transform;

                                if (UIEditorSelectionHelpers.TemporaryParentChange != null)
                                    newParent = UIEditorSelectionHelpers.TemporaryParentChange.GameObject.transform;

                                if (newParent == selectedObjects[i].transform.parent) continue;
                                selectedObjects[i].transform.SetParent(newParent.gameObject.transform, true);
                            }
                        }

                        if (IsAboutToDragMoveObjectsForFirstTime)
                        {
                            //if (UIEditorVariables.GridSnap)
                            //{
                            //    for (int a = 0; a < selectedObjects.Count; ++a)
                            //    {
                            //        RectTransform rectTransform = selectedObjects[a].GetComponent<RectTransform>();
                            //        float newX = ((int)(System.Math.Round(rectTransform.offsetMin.x / gridSizeAdjusted, 4, System.MidpointRounding.ToEven))) * gridSizeAdjusted;
                            //        float newY = ((int)(System.Math.Round(rectTransform.offsetMin.y / gridSizeAdjusted, 4, System.MidpointRounding.ToEven))) * gridSizeAdjusted;

                            //        float differenceX = newX - rectTransform.offsetMin.x;
                            //        float differenceY = newY - rectTransform.offsetMin.y;

                            //        differenceX *= window.InverseZoomScale * canvasScaleFactorInverse;
                            //        differenceY *= window.InverseZoomScale * canvasScaleFactorInverse;

                            //        rectTransform.offsetMin += new Vector2(differenceX, differenceY);
                            //        rectTransform.offsetMax += new Vector2(differenceX, differenceY);
                            //    }
                            //}
                            IsAboutToDragMoveObjectsForFirstTime = false;
                        }

                        float xAmount = ((LastKnownGridMousePosition.x - GridPositionAtStartDrag.x));
                        float yAmount = ((LastKnownGridMousePosition.y - GridPositionAtStartDrag.y));

                        if (IsMovingHorizontal && !IsMovingVertical) yAmount = 0;
                        else if (IsMovingVertical && !IsMovingHorizontal) xAmount = 0;

                        if (UIEditorVariables.GridSnap)
                        {
                            xAmount = (((int)(System.Math.Round(xAmount / gridSizeAdjusted, 4, System.MidpointRounding.ToEven))) * gridSizeAdjusted);
                            yAmount = (((int)(System.Math.Round(yAmount / gridSizeAdjusted, 4, System.MidpointRounding.ToEven))) * gridSizeAdjusted);

                            if (xAmount != 0)
                            {
                                PositionAtStartDrag.x = LastKnownMousePosition.x;
                                GridPositionAtStartDrag.x = LastKnownGridMousePosition.x;
                            }

                            if (yAmount != 0)
                            {
                                PositionAtStartDrag.y = LastKnownMousePosition.y;
                                GridPositionAtStartDrag.y = LastKnownGridMousePosition.y;
                            }
                        }
                        else
                        {
                            PositionAtStartDrag.x = LastKnownMousePosition.x;
                            PositionAtStartDrag.y = LastKnownMousePosition.y;
                            GridPositionAtStartDrag.x = LastKnownGridMousePosition.x;
                            GridPositionAtStartDrag.y = LastKnownGridMousePosition.y;
                        }

                        xAmount *= window.InverseZoomScale * canvasScaleFactorInverse;
                        yAmount *= window.InverseZoomScale * canvasScaleFactorInverse;

                        for (int a = 0; a < selectedObjects.Count; ++a)
                        {
                           if(UIEditorHelpers.IsParentInList(selectedObjects, selectedObjects[a]))
                               continue;

                           MoveObject(selectedObjects[a], new Vector2(xAmount, yAmount), window, activeScenes, selectedObjects.Count == 1);
                        }

                        // Change parent highlight
                        for (int a = 0; a < selectedObjects.Count; ++a)
                        {
                            UIEditorWindow.ObjectData objData = window.GetObjectData(selectedObjects[a]);
                            if (objData != null)
                                mouseOverElements.Remove(objData);
                        }

                        // set new temporary parent
                        UIEditorSelectionHelpers.TemporaryParentChange = null;
                        if (mouseOverElements.Count > 0)
                        {
                            for (int i = mouseOverElements.Count - 1; i >= 0; --i)
                            {
                                UIEditorWindow.ObjectData canditate = mouseOverElements[i];

                                bool isChildOfAnySelectedObject = false;
                                for (int a = 0; a < selectedObjects.Count; ++a)
                                {
                                    if (UIEditorHelpers.IsMyChild(canditate.GameObject, selectedObjects[a].gameObject))
                                    {
                                        isChildOfAnySelectedObject = true;
                                        break;
                                    }
                                }

                                if (!isChildOfAnySelectedObject)
                                {
                                    UIEditorSelectionHelpers.TemporaryParentChange = canditate;
                                    break;
                                }
                            }
                        }


                        break;
                    }

                case InputStateId.ScrollingCanvas:
                    {
                        float d = Vector2.Distance(PositionAtStartDrag, new Vector2(LastKnownMousePosition.x, LastKnownMousePosition.y));
                        if (d >= 1) HadScrollWhilstInScrollingCanvasState = true;

                        if (Event.current.button == 2 || Event.current.button == 1 || Event.current.button == 0)
                        {
                            if (Event.current.type == EventType.MouseUp)
                            {
                                InputState = InputStateId.None;

                                if (HadScrollWhilstInScrollingCanvasState)
                                {
                                    Event.current.Use();
                                }
                            }

                            UIEditorVariables.SceneScrolling = new Vector2(
                                (int)((LastKnownMousePosition.x - PositionAtStartDrag.x)) * window.InverseZoomScale,
                                (int)((LastKnownMousePosition.y - PositionAtStartDrag.y)) * window.InverseZoomScale)
                                + sceneScrollingVariable;

                            PositionAtStartDrag.x = LastKnownMousePosition.x;
                            PositionAtStartDrag.y = LastKnownMousePosition.y;
                            GridPositionAtStartDrag = new Vector2(LastKnownGridMousePosition.x, LastKnownGridMousePosition.y);
                        }

                        break;
                    }
            }
        }


        // Create mouse cursors for certain events
        // if(Tools.current != Tool.Rotate && Tools.current != Tool.Scale)
        if (!IsMouseOverMoveRotateScaleHotRect(UIEditorHelpers.GetZoomScaleFactor(), window.ZoomScalePositionOffset))
        {
            for (int a = 0; a < selectedObjects.Count; ++a)
            {
                UIEditorWindow.ObjectData objectData = window.GetObjectData(selectedObjects[a]);
                if (objectData == null) continue;

                List<Rect> corners = UIEditorHelpers.Get8ScaleBoxesAroundObject(objectData, UIEditorHelpers.GetZoomScaleFactor(), window.ZoomScalePositionOffset, true);
               // for (int b = 0; b < corners.Count; b++)
              //      corners[b] = UIEditorHelpers.TransformRectToScreenPixels(corners[b], UIEditorHelpers.GetZoomScaleFactor(), window.ZoomScalePositionOffset);

                EditorGUIUtility.AddCursorRect(corners[0], MouseCursor.ResizeUpLeft);
                EditorGUIUtility.AddCursorRect(corners[1], MouseCursor.ResizeVertical);
                EditorGUIUtility.AddCursorRect(corners[2], MouseCursor.ResizeUpRight);

                EditorGUIUtility.AddCursorRect(corners[3], MouseCursor.ResizeHorizontal);
                EditorGUIUtility.AddCursorRect(corners[4], MouseCursor.ResizeHorizontal);

                EditorGUIUtility.AddCursorRect(corners[5], MouseCursor.ResizeUpRight);
                EditorGUIUtility.AddCursorRect(corners[6], MouseCursor.ResizeVertical);
                EditorGUIUtility.AddCursorRect(corners[7], MouseCursor.ResizeUpLeft);
            }
        }

        if (Tools.current == Tool.View || InputState == InputStateId.ScrollingCanvas && HadScrollWhilstInScrollingCanvasState)
        {
            float offsetX = UIEditorToolbox.ToolboxBaseX + UIEditorToolbox.ToolboxBoxWidth;
            EditorGUIUtility.AddCursorRect(new Rect(offsetX, 20, window.position.width - offsetX, window.position.height - 20), MouseCursor.Pan);

        }
    }

    private bool IsMouseOverMoveRotateScaleHotRect(float zoomScale, Vector2 zoomScalePositionOffset)
    {
        if (IsMouseInsideWorldRect(MoveRightOnlyHotRect, zoomScale, zoomScalePositionOffset, UIEditorVariables.SceneScrolling)) return true;
        if (IsMouseInsideWorldRect(MoveUpOnlyHotRect, zoomScale, zoomScalePositionOffset, UIEditorVariables.SceneScrolling)) return true;

        if (IsMouseInsideWorldRect(ScaleLeftRightHotRect, zoomScale, zoomScalePositionOffset, UIEditorVariables.SceneScrolling)) return true;
        if (IsMouseInsideWorldRect(ScaleTopBottomHotRect, zoomScale, zoomScalePositionOffset, UIEditorVariables.SceneScrolling)) return true;

        if (IsMouseInsideCircle(RotateCenterSizeHotRect, RotateMinSizeHotRect, RotateMaxSizeHotRect)) return true;

        return false;
    }

    private bool IsPerformingAnyMoveRotateScale()
    {
        if (!IsMovingHorizontal && !IsMovingVertical && !IsScalingHorizontal && !IsScalingVertical && !IsRotating)
            return false;
        else
            return true;
    }

    private void MoveObject(GameObject control, Vector2 amount, UIEditorWindow window, List<UIEditorWindow.CanvasWithControlsGroupData> activeScenes, bool allowEdgeMagnets)
    {
        RectTransform rectTransform = control.GetComponent<RectTransform>();
        UnityEditor.Undo.RecordObject(rectTransform, "Move");

        Vector3 parentUp = Vector3.up;
        Vector3 parentRight = Vector3.right;

        if (control.transform.parent != null)
        {
            //Canvas canvas = control.transform.parent.GetComponent<Canvas>();
            //if (canvas != null && canvas.isRootCanvas)
            //{

            //}
            //else
            //{
            parentUp = control.transform.parent.up;
            parentRight = control.transform.parent.right;
            //}
        }

        Vector2 upAmount = new Vector2(parentUp.x, parentUp.y) * -amount.y;
        Vector2 rightAmount = new Vector2(parentRight.x, parentRight.y) * amount.x;
        Vector2 finalAmount = upAmount + rightAmount;

        EdgeMagnets.HandleMagnetMovement(rectTransform, finalAmount, allowEdgeMagnets);

        UIEditorHelpers.SetDirtyAllComponentsAndGameObject(control.gameObject);
    }

    private bool IsMouseInsideWorldRect(Rect rect, float zoomScale, Vector2 zoomScaleOffset, Vector2 sceneScrolling)
    {
        Rect transformedRect = rect;
        //transformedRect.x += sceneScrolling.x;
        //transformedRect.y += sceneScrolling.y;

        //transformedRect = new Rect(transformedRect.x * zoomScale, transformedRect.y * zoomScale, transformedRect.width * zoomScale, transformedRect.height * zoomScale);
        //transformedRect.x += zoomScaleOffset.x;
        //transformedRect.y += zoomScaleOffset.y;

        if (transformedRect.Contains(LastKnownMousePosition))
        {
            return true;
        }

        return false;
    }

    private bool IsMouseInsideCircle(Vector2 center, float min, float max)
    {
        float distance = Vector2.Distance(center, new Vector2(LastKnownMousePosition.x, LastKnownMousePosition.y));

        if (distance <= max && distance >= min)
            return true;

        return false;
    }

    private void IsElementInsideRegion(UIEditorWindow.ObjectData element, Rect region, List<GameObject> outputCollection, float zoomScale, Vector2 zoomScaleOffset, Vector2 sceneScrolling, Canvas canvasOwnerOfObject)
    {
        Rect transformedRect = element.Rect;
        transformedRect.x += sceneScrolling.x;
        transformedRect.y += sceneScrolling.y;

        transformedRect = new Rect(transformedRect.x * zoomScale, transformedRect.y * zoomScale, transformedRect.width * zoomScale, transformedRect.height * zoomScale);
        transformedRect.x += zoomScaleOffset.x;
        transformedRect.y += zoomScaleOffset.y;

        if (transformedRect.x > region.xMax) return;
        if (transformedRect.y > region.yMax) return;
        if (transformedRect.xMax < region.x) return;
        if (transformedRect.yMax < region.y) return;

        outputCollection.Add(element.GameObject);
    }

    private void FindObjectOnMousePosition(UIEditorWindow.ObjectData element, List<UIEditorWindow.ObjectData> outputCollection, float zoomScale, Vector2 zoomScaleOffset, Vector2 sceneScrolling, Canvas canvasOwnerOfObject)
    {
        Rect transformedRect = element.Rect;


        RectTransform rt = element.GameObject.GetComponent<RectTransform>();
        if (rt != null)
        {
            UnityEngine.UI.Graphic graphic = element.GameObject.GetComponent<UnityEngine.UI.Graphic>();

            if (rt.anchorMin.x != rt.anchorMax.x && rt.anchorMin.y != rt.anchorMax.y)
            {
                if (graphic == null || graphic.color == new Color(0, 0, 0, 0))
                {
                    // is stretched, do not make hit test for those
                    return;
                }
            }

            //if (graphic == null || (graphic != null && graphic.color == new Color(0, 0, 0, 0)))
            //{
            //    // it has no graphics, do not make hit test for those
            //    return;
            //}
        }

        transformedRect.x += sceneScrolling.x;
        transformedRect.y += sceneScrolling.y;

        transformedRect = new Rect(transformedRect.x * zoomScale, transformedRect.y * zoomScale, transformedRect.width * zoomScale, transformedRect.height * zoomScale);
        transformedRect.x += zoomScaleOffset.x;
        transformedRect.y += zoomScaleOffset.y;

        if (transformedRect.Contains(LastKnownMousePosition))
        {
            outputCollection.Add(element);
        }
    }

    private void HandleCommands()
    {
        Event e = Event.current;

        if (e.type == EventType.ValidateCommand)
        {
            if (e.commandName == "Delete" || e.commandName == "SoftDelete")
            {
                e.Use();
            }
            else if (e.commandName == "Cut" || e.commandName == "Copy" || e.commandName == "Paste" || e.commandName == "Duplicate")
            {
                e.Use();
            }
            else if (e.commandName == "SelectAll")
            {
                e.Use();
            }
            else if (e.commandName == "UndoRedoPerformed")
            {
                e.Use();
            }
            else
            {
                Debug.Log(e.commandName + " is not validated");
            }
        }
        else if (e.type == EventType.ExecuteCommand)
        {
            if (e.commandName == "Delete" || e.commandName == "SoftDelete")
            {
                UIEditorHelpers.GetFocusedWindow("Hierarchy").SendEvent(EditorGUIUtility.CommandEvent("Delete"));
            }
            else if (e.commandName == "Cut")
            {
                UIEditorHelpers.GetFocusedWindow("Hierarchy").SendEvent(EditorGUIUtility.CommandEvent("Cut"));
            }
            else if (e.commandName == "Copy")
            {
                UIEditorHelpers.GetFocusedWindow("Hierarchy").SendEvent(EditorGUIUtility.CommandEvent("Copy"));
            }
            else if (e.commandName == "Duplicate")
            {
                UIEditorHelpers.GetFocusedWindow("Hierarchy").SendEvent(EditorGUIUtility.CommandEvent("Duplicate"));
            }
            else if (e.commandName == "Paste")
            {
                UIEditorHelpers.GetFocusedWindow("Hierarchy").SendEvent(EditorGUIUtility.CommandEvent("Paste"));
            }
            else if (e.commandName == "SelectAll")
            {
                if (Selection.activeGameObject != null && Selection.activeGameObject.transform.parent != null)
                {
                    List<GameObject> newSelection = new List<GameObject>();
                    for (int i = 0; i < Selection.activeGameObject.transform.parent.childCount; ++i)
                    {
                        newSelection.Add(Selection.activeGameObject.transform.parent.GetChild(i).gameObject);
                    }

                    Selection.objects = newSelection.ToArray();
                }
            }
            else
            {
                //Debug.Log("ExecuteCommand Command:" + e.commandName);
            }
        }
    }
}

