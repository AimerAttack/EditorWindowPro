//-----------------------------------------------------------------------------------------
// UI Editor
// Copyright © Argiris Baltzis - All Rights Reserved
//
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
//-----------------------------------------------------------------------------------------
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class UIEditorContextMenu
{
    private enum AnchorId
    {
        HorizontalLeft,
        HorizontalMiddle,
        HorizontalRight,
        HorizontalStretch,

        VerticalTop,
        VerticalMiddle,
        VerticalBottom,
        VerticalStretch,

        StretchAll,
    }

    private enum PivotId
    {
        Left,
        MiddleHorizontal,
        Right,

        Top,
        MiddleVertical,
        Bottom,

        //MiddleLeft, MiddleCenter, MiddleRight,
        //BottomLeft, BottomMiddle, BottomRight,
        //HorizontalStretch,
        //VerticalStretch,
        //Stretch,
        
    }

    private Vector2 MousePositionAtRightClick;
    private UIEditorWindow Window;

    private bool HasRectTransformOnSelection()
    {
        for (int i = 0; i < Selection.gameObjects.Length; ++i)
        {
            if (Selection.gameObjects[i].GetComponent<RectTransform>() != null)
            {
                return true;
            }
        }

        return false;
    }

    public void OnGUI(UIEditorWindow window)
    {
        Window = window;
        MousePositionAtRightClick = Event.current.mousePosition;

        if (Event.current.type == EventType.ContextClick/* && !window.Input.HasScrolledWithRightButton*/)
        {
            UIEditorSelectionHelpers.HighlightedObjectOnContextMenu = UIEditorSelectionHelpers.MouseOverHighlightedObject;

            bool hasNativeSizeFunctionality = false;
            for (int i = 0; i < UIEditorSelectionHelpers.Selected.Count; ++i)
            {
                GameObject element = UIEditorSelectionHelpers.Selected[i];

                if (element.GetComponent<UnityEngine.UI.Image>() != null || element.GetComponent<UnityEngine.UI.Text>() != null || element.GetComponent<UnityEngine.UI.RawImage>() != null)
                {
                    hasNativeSizeFunctionality = true;
                    break;
                }
            }

            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Duplicate"), false, DuplicateSelectedObjects, null);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Order/Bring to Front"), false, SendTopFront, null);
            menu.AddItem(new GUIContent("Order/Send to Back"), false, SendFarBack, null);
            menu.AddItem(new GUIContent("Order/Bring Forward by 1"), false, BringToFrontBy1, null);
            menu.AddItem(new GUIContent("Order/Send Backward by 1"), false, SendToBackBy1, null);
            menu.AddSeparator("");

            BuildUIObjectsMenu(menu, window);



            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Pivot/Left (Horizontal)"), HasPivotAt(PivotId.Left), SetPivot, PivotId.Left);
            menu.AddItem(new GUIContent("Pivot/Middle (Horizontal)"), HasPivotAt(PivotId.MiddleHorizontal), SetPivot, PivotId.MiddleHorizontal);
            menu.AddItem(new GUIContent("Pivot/Right (Horizontal)"), HasPivotAt(PivotId.Right), SetPivot, PivotId.Right);
            menu.AddSeparator("Pivot/");
            menu.AddItem(new GUIContent("Pivot/Top (Vertical)"), HasPivotAt(PivotId.Top), SetPivot, PivotId.Top);
            menu.AddItem(new GUIContent("Pivot/Middle (Vertical)"), HasPivotAt(PivotId.MiddleVertical), SetPivot, PivotId.MiddleVertical);
            menu.AddItem(new GUIContent("Pivot/Bottom (Vertical)"), HasPivotAt(PivotId.Bottom), SetPivot, PivotId.Bottom);
            //menu.AddSeparator("Pivot/");
            //menu.AddItem(new GUIContent("Pivot/Bottom Left"), HasPivotAt(PivotId.BottomLeft), SetPivot, PivotId.BottomLeft);
            //menu.AddItem(new GUIContent("Pivot/Bottom Middle"), HasPivotAt(PivotId.BottomMiddle), SetPivot, PivotId.BottomMiddle);
            //menu.AddItem(new GUIContent("Pivot/Bottom Right"), HasPivotAt(PivotId.BottomRight), SetPivot, PivotId.BottomRight);

            //menu.AddItem(new GUIContent("Anchor/Top Left"), HasAnchorAt(AnchorId.TopLeft), SetAnchor, AnchorId.TopLeft);
            //menu.AddItem(new GUIContent("Anchor/Top Middle"), HasAnchorAt(AnchorId.TopMiddle), SetAnchor, AnchorId.TopMiddle);
            //menu.AddItem(new GUIContent("Anchor/Top Right"), HasAnchorAt(AnchorId.TopRight), SetAnchor, AnchorId.TopRight);
            //menu.AddSeparator("Anchor/");
            //menu.AddItem(new GUIContent("Anchor/Middle Left"), HasAnchorAt(AnchorId.MiddleLeft), SetAnchor, AnchorId.MiddleLeft);
            //menu.AddItem(new GUIContent("Anchor/Center"), HasAnchorAt(AnchorId.MiddleCenter), SetAnchor, AnchorId.MiddleCenter);
            //menu.AddItem(new GUIContent("Anchor/Middle Right"), HasAnchorAt(AnchorId.MiddleRight), SetAnchor, AnchorId.MiddleRight);
            //menu.AddSeparator("Anchor/");
            //menu.AddItem(new GUIContent("Anchor/Bottom Left"), HasAnchorAt(AnchorId.BottomLeft), SetAnchor, AnchorId.BottomLeft);
            //menu.AddItem(new GUIContent("Anchor/Bottom Middle"), HasAnchorAt(AnchorId.BottomMiddle), SetAnchor, AnchorId.BottomMiddle);
            //menu.AddItem(new GUIContent("Anchor/Bottom Right"), HasAnchorAt(AnchorId.BottomRight), SetAnchor, AnchorId.BottomRight);
            //menu.AddSeparator("Anchor/");
            //menu.AddItem(new GUIContent("Anchor/Horizontal Stretch"), HasAnchorAt(AnchorId.HorizontalStretch), SetAnchor, AnchorId.HorizontalStretch);
            //menu.AddItem(new GUIContent("Anchor/Vertical Stretch"), HasAnchorAt(AnchorId.VerticalStretch), SetAnchor, AnchorId.VerticalStretch);
            //menu.AddItem(new GUIContent("Anchor/Stretch"), HasAnchorAt(AnchorId.Stretch), SetAnchor, AnchorId.Stretch);

            menu.AddItem(new GUIContent("Anchor/Left (Horizontal)"), HasAnchorAt(AnchorId.HorizontalLeft), SetAnchor, AnchorId.HorizontalLeft);
            menu.AddItem(new GUIContent("Anchor/Middle (Horizontal)"), HasAnchorAt(AnchorId.HorizontalMiddle), SetAnchor, AnchorId.HorizontalMiddle);
            menu.AddItem(new GUIContent("Anchor/Right (Horizontal)"), HasAnchorAt(AnchorId.HorizontalRight), SetAnchor, AnchorId.HorizontalRight);
            menu.AddItem(new GUIContent("Anchor/Stretch (Horizontal)"), HasAnchorAt(AnchorId.HorizontalStretch), SetAnchor, AnchorId.HorizontalStretch);
            menu.AddSeparator("Anchor/");
            menu.AddItem(new GUIContent("Anchor/Top (Vertical)"), HasAnchorAt(AnchorId.VerticalTop), SetAnchor, AnchorId.VerticalTop);
            menu.AddItem(new GUIContent("Anchor/Middle (Vertical)"), HasAnchorAt(AnchorId.VerticalMiddle), SetAnchor, AnchorId.VerticalMiddle);
            menu.AddItem(new GUIContent("Anchor/Bottom (Vertical)"), HasAnchorAt(AnchorId.VerticalBottom), SetAnchor, AnchorId.VerticalBottom);
            menu.AddItem(new GUIContent("Anchor/Stretch (Vertical)"), HasAnchorAt(AnchorId.VerticalStretch), SetAnchor, AnchorId.VerticalStretch);
            menu.AddSeparator("Anchor/");
            menu.AddItem(new GUIContent("Anchor/Stretch All"), HasAnchorAt(AnchorId.VerticalStretch) && HasAnchorAt(AnchorId.HorizontalStretch), SetAnchor, AnchorId.StretchAll);


            if (Selection.objects.Length > 1)
            {
                menu.AddItem(new GUIContent("Align Objects/Left"), false, AlignLeft, null);
                menu.AddItem(new GUIContent("Align Objects/Right"), false, AlignRight, null);
                menu.AddItem(new GUIContent("Align Objects/Top"), false, AlignTop, null);
                menu.AddItem(new GUIContent("Align Objects/Bottom"), false, AlignBottom, null);
                menu.AddItem(new GUIContent("Align Objects/Horizontally"), false, AlignHorizontally, null);
                menu.AddItem(new GUIContent("Align Objects/Vertical"), false, AlignVerically, null);
                menu.AddSeparator("Align Objects/");
                menu.AddItem(new GUIContent("Align Objects/Same Width"), false, SameSizeWidth, null);
                menu.AddItem(new GUIContent("Align Objects/Same Height"), false, SameSizeHeight, null);
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Align Objects"));
            }

            {
                menu.AddItem(new GUIContent("Snapping/Grid"), UIEditorVariables.GridSnap, SetSnapTo, 0);
                menu.AddItem(new GUIContent("Snapping/Boundary Edges"), UIEditorVariables.EdgeParentSnap, SetSnapTo, 1);
                menu.AddItem(new GUIContent("Snapping/Neighbor Edges"), UIEditorVariables.NeighborEdgeSnap, SetSnapTo, 2);
                menu.AddSeparator("Snapping/");
                menu.AddItem(new GUIContent("Snapping/Disable All        Ctrl+G"), UIEditorVariables.DisableAllSnap, DisableSnapToggle, 0);

            }

            menu.AddSeparator("");

            bool hasObjectsToShow = false;
            bool hasObjectsToHide = false;

            for (int i = 0; i < Selection.gameObjects.Length; ++i)
            {
                if (Selection.gameObjects[i].activeSelf) hasObjectsToHide = true;
                else hasObjectsToShow = true;
            }

            if (hasObjectsToShow) menu.AddItem(new GUIContent("Show"), false, Show, null);
            else menu.AddDisabledItem(new GUIContent("Show"));

            if (hasObjectsToHide) menu.AddItem(new GUIContent("Hide"), false, Hide, null);
            else menu.AddDisabledItem(new GUIContent("Hide"));

            //menu.AddSeparator("Size/");
            //if (hasNativeSizeFunctionality) menu.AddItem(new GUIContent("Size/Native Size"), false, SetNativeSize, null);
            //else menu.AddDisabledItem(new GUIContent("Size/Native Size"));

            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Reset/Position"), false, ResetPosition, null);
            menu.AddItem(new GUIContent("Reset/Rotation"), false, ResetRotation, null);
            menu.AddItem(new GUIContent("Reset/Scale"), false, ResetScale, null);
            menu.AddSeparator("Reset/");
            if (hasNativeSizeFunctionality) menu.AddItem(new GUIContent("Reset/Size"), false, SetNativeSize, null);
            else menu.AddDisabledItem(new GUIContent("Reset/Native Size"));
            menu.AddSeparator("Reset/");
            menu.AddItem(new GUIContent("Reset/View"), false, ResetView, null);

            menu.ShowAsContext();

            Event.current.Use();
        }
    }

    private void DisableSnapToggle(object obj)
    {
        UIEditorVariables.DisableAllSnap = !UIEditorVariables.DisableAllSnap;
    }

    private void SetSnapTo(object obj)
    {
        if ((int)obj == 0)
        {
            UIEditorVariables.GridSnap = !UIEditorVariables.GridSnap;
        }
        else if ((int)obj == 1)
        {
            UIEditorVariables.EdgeParentSnap = !UIEditorVariables.EdgeParentSnap;
        }
        else if ((int)obj == 2)
        {
            UIEditorVariables.NeighborEdgeSnap = !UIEditorVariables.NeighborEdgeSnap;
        }
    }

    private void ResetView(object obj)
    {
        UIEditorVariables.SceneScrolling = Vector2.zero;
    }

    private void SetNativeSize(object obj)
    {
        for (int i = 0; i < UIEditorSelectionHelpers.Selected.Count; ++i)
        {
            GameObject element = UIEditorSelectionHelpers.Selected[i];

            UnityEngine.UI.Image image = element.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                if (image.sprite != null)
                {
                    element.GetComponent<RectTransform>().sizeDelta = new Vector2(
                        image.sprite.textureRect.size.x,
                        image.sprite.textureRect.size.y);

                }
            }

            UnityEngine.UI.RawImage texture = element.GetComponent<UnityEngine.UI.RawImage>();
            if (texture != null)
            {
                if (texture.texture != null)
                {
                    element.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.texture.width, texture.texture.height);
                }
            }

            UnityEngine.UI.Text text = element.GetComponent<UnityEngine.UI.Text>();
            if (text != null)
            {
                RectTransform rectTransform = element.GetComponent<RectTransform>();

                //// 1. resize height
                //float prefHeight = UnityEngine.UI.LayoutUtility.GetPreferredHeight(rectTransform);
                //rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, prefHeight);

                // 2. now resize width
                float prefWidth = UnityEngine.UI.LayoutUtility.GetPreferredWidth(rectTransform);
                float prefHeight = UnityEngine.UI.LayoutUtility.GetPreferredHeight(rectTransform);

                rectTransform.sizeDelta = new Vector2(prefWidth, prefHeight);
            }
        }
    }

    private bool HasAnchorAt(AnchorId id)
    {
        for (int i = 0; i < Selection.gameObjects.Length; ++i)
        {
            RectTransform rect = Selection.gameObjects[i].GetComponent<RectTransform>();
            if (rect == null) continue;

            switch (id)
            {
                case AnchorId.HorizontalLeft:
                    if (rect.anchorMin.x == 0 && rect.anchorMax.x == 0) return true;
                    break;
                case AnchorId.HorizontalMiddle:
                    if (rect.anchorMin.x == 0.5f && rect.anchorMax.x == 0.5f) return true;
                    break;
                case AnchorId.HorizontalRight:
                    if (rect.anchorMin.x == 1 && rect.anchorMax.x == 1) return true;
                    break;
                case AnchorId.HorizontalStretch:
                    if (rect.anchorMin.x != rect.anchorMax.x) return true;
                    break;

                case AnchorId.VerticalBottom:
                    if (rect.anchorMin.y == 0 && rect.anchorMax.y == 0) return true;
                    break;
                case AnchorId.VerticalMiddle:
                    if (rect.anchorMin.y == 0.5f && rect.anchorMax.y == 0.5f) return true;
                    break;
                case AnchorId.VerticalTop:
                    if (rect.anchorMin.y == 1 && rect.anchorMax.y == 1) return true;
                    break;
                case AnchorId.VerticalStretch:
                    if (rect.anchorMin.y != rect.anchorMax.y) return true;
                    break;
            }
        }

        return false;
    }

    private bool HasPivotAt(PivotId id)
    {
        for (int i = 0; i < Selection.gameObjects.Length; ++i)
        {
            RectTransform rect = Selection.gameObjects[i].GetComponent<RectTransform>();
            if (rect == null) continue;

            if (rect.pivot.x == 0 && id == PivotId.Left) return true;
            if (rect.pivot.x == 0.5f && id == PivotId.MiddleHorizontal) return true;
            if (rect.pivot.x == 1 && id == PivotId.Right) return true;

            if ( rect.pivot.y == 1.0f && id == PivotId.Top) return true;
            if ( rect.pivot.y == 0.5f && id == PivotId.MiddleVertical) return true;
            if ( rect.pivot.y == 0.0f && id == PivotId.Bottom) return true;

            //if (rect.pivot.x == 0 && rect.pivot.y == 0 && id == PivotId.BottomLeft) return true;
            //if (rect.pivot.x == 0.5f && rect.pivot.y == 0 && id == PivotId.BottomMiddle) return true;
            //if (rect.pivot.x == 1 && rect.pivot.y == 0 && id == PivotId.BottomRight) return true;

            //if (rect.pivot.x == 0 && rect.pivot.y == 1 && id == PivotId.TopLeft) return true;
            //if (rect.pivot.x == 0.5f && rect.pivot.y == 1 && id == PivotId.TopMiddle) return true;
            //if (rect.pivot.x == 1 && rect.pivot.y == 1 && id == PivotId.TopRight) return true;

            //if (rect.pivot.x == 0 && rect.pivot.y == 0.5f && id == PivotId.MiddleLeft) return true;
            //if (rect.pivot.x == 0.5f && rect.pivot.y == 0.5f && id == PivotId.MiddleCenter) return true;
            //if (rect.pivot.x == 1 && rect.pivot.y == 0.5f && id == PivotId.MiddleRight) return true;

            //if (rect.pivot.x == 0 && rect.pivot.y == 0 && id == PivotId.BottomLeft) return true;
            //if (rect.pivot.x == 0.5f && rect.pivot.y == 0 && id == PivotId.BottomMiddle) return true;
            //if (rect.pivot.x == 1 && rect.pivot.y == 0 && id == PivotId.BottomRight) return true;
        }

        return false;
    }

    private void BuildUIObjectsMenu(GenericMenu menu, UIEditorWindow window)
    {
        List<UIEditorToolbox.NativeControlData> nativeControls = new List<UIEditorToolbox.NativeControlData>();

        window.Toolbox.CreateControlList(nativeControls);
        for (int i = 0; i < nativeControls.Count; ++i)
        {
            menu.AddItem(new GUIContent("UI/" + nativeControls[i].Display), false, CreateControlFromMenu, nativeControls[i]);

        }
    }

    public void Show(object obj)
    {
        for (int i = 0; i < Selection.gameObjects.Length; ++i)
        {
            Selection.gameObjects[i].SetActive(true);
        }
    }

    public void Hide(object obj)
    {
        for (int i = 0; i < Selection.gameObjects.Length; ++i)
        {
            Selection.gameObjects[i].SetActive(false);
        }
    }

    public static void SetPivot(object obj)
    {
        PivotId id = (PivotId)obj;

        for (int i = 0; i < UIEditorSelectionHelpers.Selected.Count; ++i)
        {
            RectTransform element = UIEditorSelectionHelpers.Selected[i].GetComponent<RectTransform>();
            if (element == null) continue;
            UnityEditor.Undo.RecordObject(element, "Modify Pivot");

            Rect savedRect = element.rect;

            switch (id)
            {
                case PivotId.Left:
                    element.pivot = new Vector2(0, element.pivot.y);
                    break;
                case PivotId.MiddleHorizontal:
                    element.pivot = new Vector2(0.5f, element.pivot.y);
                    break;
                case PivotId.Right:
                    element.pivot = new Vector2(1.0f, element.pivot.y);
                    break;

                case PivotId.Top:
                    element.pivot = new Vector2(element.pivot.x, 1.0f);
                    break;
                case PivotId.MiddleVertical:
                    element.pivot = new Vector2(element.pivot.x, 0.5f);
                    break;
                case PivotId.Bottom:
                    element.pivot = new Vector2(element.pivot.x, 0.0f);
                    break;


                    //case PivotId.TopLeft:
                    //    element.pivot = new Vector2(0, 1);
                    //    break;
                    //case PivotId.TopMiddle:
                    //    element.pivot = new Vector2(0.5f, 1);
                    //    break;
                    //case PivotId.TopRight:
                    //    element.pivot = new Vector2(1.0f, 1);
                    //    break;

                    //case PivotId.MiddleLeft:
                    //    element.pivot = new Vector2(0, 0.5f);
                    //    break;
                    //case PivotId.MiddleCenter:
                    //    element.pivot = new Vector2(0.5f, 0.5f);
                    //    break;
                    //case PivotId.MiddleRight:
                    //    element.pivot = new Vector2(1.0f, 0.5f);
                    //    break;

                    //case PivotId.BottomLeft:
                    //    element.pivot = new Vector2(0, 0);
                    //    break;
                    //case PivotId.BottomMiddle:
                    //    element.pivot = new Vector2(0.5f, 0);
                    //    break;
                    //case PivotId.BottomRight:
                    //    element.pivot = new Vector2(1.0f, 0);
                    //    break;
            }

            float offsetX = savedRect.x - element.rect.x;
            float offsetY = savedRect.y - element.rect.y;

            element.offsetMin += new Vector2(offsetX, offsetY);
            element.offsetMax += new Vector2(offsetX, offsetY);
        }

    }

    void SetAnchor(object obj)
    {
        AnchorId id = (AnchorId)obj;

        for (int i = 0; i < UIEditorSelectionHelpers.Selected.Count; ++i)
        {
            RectTransform element = UIEditorSelectionHelpers.Selected[i].GetComponent<RectTransform>();
            if (element == null) continue;

            UnityEditor.Undo.RecordObject(element, "Modify Anchor");

            Vector3 previousLocalPosition = element.localPosition;
            bool applyPositionChange = true;

            bool wasHorizontalStretched = false;
            bool wasVerticalStretch = false;

            if (element.anchorMin.x != element.anchorMax.x) wasHorizontalStretched = true;
            if (element.anchorMin.y != element.anchorMax.y) wasVerticalStretch = true;

            switch (id)
            {
                case AnchorId.HorizontalLeft:
                    element.anchorMin = new Vector2(0, element.anchorMin.y);
                    element.anchorMax = new Vector2(0, element.anchorMax.y);
                    if (wasHorizontalStretched) element.sizeDelta = new Vector2(100, element.sizeDelta.y);

                    break;
                case AnchorId.HorizontalMiddle:
                    element.anchorMin = new Vector2(0.5f, element.anchorMin.y);
                    element.anchorMax = new Vector2(0.5f, element.anchorMax.y);
                    if (wasHorizontalStretched) element.sizeDelta = new Vector2(100, element.sizeDelta.y);

                    break;
                case AnchorId.HorizontalRight:
                    element.anchorMin = new Vector2(1, element.anchorMin.y);
                    element.anchorMax = new Vector2(1, element.anchorMax.y);
                    if (wasHorizontalStretched) element.sizeDelta = new Vector2(100, element.sizeDelta.y);

                    break;
                case AnchorId.HorizontalStretch:
                    element.anchorMin = new Vector2(0, element.anchorMin.y);
                    element.anchorMax = new Vector2(1, element.anchorMax.y);

                    element.offsetMin = new Vector2(0, element.offsetMin.y);
                    element.offsetMax = new Vector2(0, element.offsetMax.y);
                    element.anchoredPosition = new Vector2(0, element.anchoredPosition.y);
                    applyPositionChange = false;

                    break;

                case AnchorId.VerticalBottom:
                    element.anchorMin = new Vector2(element.anchorMin.x, 0);
                    element.anchorMax = new Vector2(element.anchorMax.x, 0);
                    if (wasVerticalStretch) element.sizeDelta = new Vector2(element.sizeDelta.x, 100);

                    break;
                case AnchorId.VerticalMiddle:
                    element.anchorMin = new Vector2(element.anchorMin.x, 0.5f);
                    element.anchorMax = new Vector2(element.anchorMax.x, 0.5f);
                    if (wasVerticalStretch) element.sizeDelta = new Vector2(element.sizeDelta.x, 100);

                    break;
                case AnchorId.VerticalTop:
                    element.anchorMin = new Vector2(element.anchorMin.x, 1);
                    element.anchorMax = new Vector2(element.anchorMax.x, 1);
                    if (wasVerticalStretch) element.sizeDelta = new Vector2(element.sizeDelta.x, 100);

                    break;
                case AnchorId.VerticalStretch:
                    element.anchorMin = new Vector2(element.anchorMin.x, 0);
                    element.anchorMax = new Vector2(element.anchorMax.x, 1);

                    element.offsetMin = new Vector2(element.offsetMin.x, 0);
                    element.offsetMax = new Vector2(element.offsetMax.x, 0);
                    element.anchoredPosition = new Vector2(element.anchoredPosition.x, 0);
                    applyPositionChange = false;
                    break;

                case AnchorId.StretchAll:
                    element.anchorMin = new Vector2(0, 0);
                    element.anchorMax = new Vector2(1, 1);

                    element.offsetMin = new Vector2(0, 0);
                    element.offsetMax = new Vector2(0, 0);
                    element.anchoredPosition = new Vector2(0, 0);
                    applyPositionChange = false;

                    break;
            }

            if (applyPositionChange)
            {
                Vector3 difference = element.localPosition - previousLocalPosition;
                element.anchoredPosition += new Vector2(-difference.x, -difference.y);
            }

          
            //switch (id)
            //{
            //    case PivotId.TopLeft:
            //        element.anchorMin = new Vector2(0, 1);
            //        element.anchorMax = new Vector2(0, 1);
            //        break;
            //    case PivotId.TopMiddle:
            //        element.anchorMin = new Vector2(0.5f, 1);
            //        element.anchorMax = new Vector2(0.5f, 1);
            //        break;
            //    case PivotId.TopRight:
            //        element.anchorMin = new Vector2(1, 1);
            //        element.anchorMax = new Vector2(1, 1);
            //        break;

            //    case PivotId.MiddleLeft:
            //        element.anchorMin = new Vector2(0, 0.5f);
            //        element.anchorMax = new Vector2(0, 0.5f);
            //        break;
            //    case PivotId.MiddleCenter:
            //        element.anchorMin = new Vector2(0.5f, 0.5f);
            //        element.anchorMax = new Vector2(0.5f, 0.5f);
            //        break;
            //    case PivotId.MiddleRight:
            //        element.anchorMin = new Vector2(1, 0.5f);
            //        element.anchorMax = new Vector2(1, 0.5f);
            //        break;

            //    case PivotId.BottomLeft:
            //        element.anchorMin = new Vector2(0, 0);
            //        element.anchorMax = new Vector2(0, 0);
            //        break;
            //    case PivotId.BottomMiddle:
            //        element.anchorMin = new Vector2(0.5f, 0);
            //        element.anchorMax = new Vector2(0.5f, 0);
            //        break;
            //    case PivotId.BottomRight:
            //        element.anchorMin = new Vector2(1, 0);
            //        element.anchorMax = new Vector2(1, 0);
            //        break;


            //    case PivotId.HorizontalStretch:
            //        element.anchorMin = new Vector2(0, element.anchorMin.y);
            //        element.anchorMax = new Vector2(1, element.anchorMax.y);

            //        element.offsetMin = new Vector2(0, element.offsetMin.y);
            //        element.offsetMax = new Vector2(0, element.offsetMax.y);
            //        break;

            //    case PivotId.VerticalStretch:
            //        element.anchorMin = new Vector2(element.anchorMin.x, 0);
            //        element.anchorMax = new Vector2(element.anchorMax.x, 1);

            //        element.offsetMin = new Vector2(element.offsetMin.x, 0);
            //        element.offsetMax = new Vector2(element.offsetMax.x, 0);
            //        break;

            //    case PivotId.Stretch:
            //        element.anchorMin = new Vector2(0, 0);
            //        element.anchorMax = new Vector2(1, 1);
            //        element.offsetMin = Vector2.zero;
            //        element.offsetMax = Vector2.zero;

            //        break;
            //}

           // ResetPosition(null);
        }
    }

    public static void DuplicateSelectedObjects(object obj)
    {
        UIEditorHelpers.GetFocusedWindow("Hierarchy").SendEvent(EditorGUIUtility.CommandEvent("Duplicate"));
    }

    void ResetPosition(object obj)
    {
        for (int i = 0; i < UIEditorSelectionHelpers.Selected.Count; ++i)
        {
            RectTransform element = UIEditorSelectionHelpers.Selected[i].GetComponent<RectTransform>();
            if (element == null) continue;

            //float width = element.sizeDelta.x;
            //float height = element.sizeDelta.y;

            //element.offsetMin = new Vector2(-(width * element.pivot.x), -(height * element.pivot.y));
           // element.offsetMax = new Vector2((width * (1 - element.pivot.x)), (height * (1 - element.pivot.y)));
            element.anchoredPosition = Vector2.zero;
            element.anchoredPosition3D = Vector3.zero;

            if (element.anchorMin.x != element.anchorMax.x)
            {
                element.sizeDelta = new Vector2(0, element.sizeDelta.y);
            }

            if (element.anchorMin.y != element.anchorMax.y)
            {
                element.sizeDelta = new Vector2(element.sizeDelta.x, 0);
            }

        }
    }

    void ResetRotation(object obj)
    {
        for (int i = 0; i < UIEditorSelectionHelpers.Selected.Count; ++i)
        {
            RectTransform element = UIEditorSelectionHelpers.Selected[i].GetComponent<RectTransform>();
            if (element == null) continue;
            element.localRotation = Quaternion.identity;
        }
    }

    void ResetScale(object obj)
    {
        for (int i = 0; i < UIEditorSelectionHelpers.Selected.Count; ++i)
        {
            RectTransform element = UIEditorSelectionHelpers.Selected[i].GetComponent<RectTransform>();
            if (element == null) continue;
            element.localScale = Vector3.one;
        }
    }

    void SameSizeWidth(object obj)
    {
        List<GameObject> selected = UIEditorSelectionHelpers.Selected;


        float limit = selected[0].GetComponent<RectTransform>().offsetMax.x - selected[0].GetComponent<RectTransform>().offsetMin.x;

        for (int i = 1; i < selected.Count; ++i)
        {
            UIEditorWindow.ObjectData objData = Window.GetObjectData(selected[i]);

            objData.GameObject.GetComponent<RectTransform>().offsetMax = new Vector2(
                objData.GameObject.GetComponent<RectTransform>().offsetMin.x + limit,
                 objData.GameObject.GetComponent<RectTransform>().offsetMax.y);
        }
    }

    void SameSizeHeight(object obj)
    {
        List<GameObject> selected = UIEditorSelectionHelpers.Selected;

        float limit = selected[0].GetComponent<RectTransform>().offsetMax.y - selected[0].GetComponent<RectTransform>().offsetMin.y;

        for (int i = 1; i < selected.Count; ++i)
        {
            UIEditorWindow.ObjectData objData = Window.GetObjectData(selected[i]);
            RectTransform rectTransform = objData.GameObject.GetComponent<RectTransform>();

            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMax.y - limit);
        }
    }

    void CreateControlFromMenu(object obj)
    {
        UIEditorToolbox.NativeControlData path = (UIEditorToolbox.NativeControlData)obj;
        if (path.CreateAction != null)
        {
            path.CreateAction.Invoke();
        }
        else
        {
            EditorApplication.ExecuteMenuItem(path.MenuPath);
        }

        if (Selection.activeGameObject != null)
        {
            UIEditorHelpers.OnAfterCreateControl(MousePositionAtRightClick, Window);
        }
    }

    void AlignTop(object obj)
    {
        List<GameObject> selected = UIEditorSelectionHelpers.Selected;

        float farTopPosition = float.MaxValue;
        for (int i = 0; i < selected.Count; ++i)
        {
            UIEditorWindow.ObjectData objData = Window.GetObjectData(selected[i]);

            if (objData.Rect.yMin < farTopPosition)
                farTopPosition = objData.Rect.yMin;
        }

        for (int i = 0; i < selected.Count; ++i)
        {
            UIEditorWindow.ObjectData objData = Window.GetObjectData(selected[i]);

            float difference = objData.Rect.yMin - farTopPosition;

            objData.GameObject.GetComponent<RectTransform>().offsetMin += new Vector2(0, difference);
            objData.GameObject.GetComponent<RectTransform>().offsetMax += new Vector2(0, difference);
        }
    }

    void AlignBottom(object obj)
    {
        List<GameObject> selected = UIEditorSelectionHelpers.Selected;

        float farTopPosition = float.MinValue;
        for (int i = 0; i < selected.Count; ++i)
        {
            UIEditorWindow.ObjectData objData = Window.GetObjectData(selected[i]);

            if (objData.Rect.yMax > farTopPosition)
                farTopPosition = objData.Rect.yMax;
        }

        for (int i = 0; i < selected.Count; ++i)
        {
            UIEditorWindow.ObjectData objData = Window.GetObjectData(selected[i]);

            float difference = objData.Rect.yMax - farTopPosition;

            objData.GameObject.GetComponent<RectTransform>().offsetMin += new Vector2(0, difference);
            objData.GameObject.GetComponent<RectTransform>().offsetMax += new Vector2(0, difference);
        }
    }

    void AlignLeft(object obj)
    {
        List<GameObject> selected = UIEditorSelectionHelpers.Selected;

        float limit = float.MaxValue;
        for (int i = 0; i < selected.Count; ++i)
        {
            UIEditorWindow.ObjectData objData = Window.GetObjectData(selected[i]);

            if (objData.Rect.xMin < limit)
                limit = objData.Rect.xMin;
        }

        for (int i = 0; i < selected.Count; ++i)
        {
            UIEditorWindow.ObjectData objData = Window.GetObjectData(selected[i]);

            float difference = objData.Rect.xMin - limit;

            objData.GameObject.GetComponent<RectTransform>().offsetMin -= new Vector2(difference, 0);
            objData.GameObject.GetComponent<RectTransform>().offsetMax -= new Vector2(difference, 0);
        }
    }

    void AlignRight(object obj)
    {
        List<GameObject> selected = UIEditorSelectionHelpers.Selected;

        float limit = float.MinValue;
        for (int i = 0; i < selected.Count; ++i)
        {
            UIEditorWindow.ObjectData objData = Window.GetObjectData(selected[i]);

            if (objData.Rect.xMax > limit)
                limit = objData.Rect.xMax;
        }

        for (int i = 0; i < selected.Count; ++i)
        {
            UIEditorWindow.ObjectData objData = Window.GetObjectData(selected[i]);

            float difference = objData.Rect.xMax - limit;

            objData.GameObject.GetComponent<RectTransform>().offsetMin -= new Vector2(difference, 0);
            objData.GameObject.GetComponent<RectTransform>().offsetMax -= new Vector2(difference, 0);
        }
    }

    void AlignHorizontally(object obj)
    {
        List<GameObject> selected = UIEditorSelectionHelpers.Selected;
        if (selected.Count < 1) return;

        float farRight = Window.GetObjectData(selected[0]).Rect.xMax;

        for (int i = 1; i < selected.Count; ++i)
        {
            UIEditorWindow.ObjectData objData = Window.GetObjectData(selected[i]);

            float difference = (farRight - objData.Rect.xMax) + objData.Rect.width;
            farRight += objData.Rect.width;

            objData.GameObject.GetComponent<RectTransform>().offsetMin += new Vector2(difference, 0);
            objData.GameObject.GetComponent<RectTransform>().offsetMax += new Vector2(difference, 0);
        }
    }

    void AlignVerically(object obj)
    {
        List<GameObject> selected = UIEditorSelectionHelpers.Selected;
        if (selected.Count < 1) return;

        float farBottom = Window.GetObjectData(selected[0]).Rect.yMax;

        for (int i = 1; i < selected.Count; ++i)
        {
            UIEditorWindow.ObjectData objData = Window.GetObjectData(selected[i]);

            float difference = (farBottom - objData.Rect.yMax) + objData.Rect.height;
            farBottom += objData.Rect.height;

            objData.GameObject.GetComponent<RectTransform>().offsetMin -= new Vector2(0, difference);
            objData.GameObject.GetComponent<RectTransform>().offsetMax -= new Vector2(0, difference);
        }
    }

    void SendFarBack(object obj)
    {
        for (int i = 0; i < UIEditorSelectionHelpers.Selected.Count; ++i)
        {
            UIEditorSelectionHelpers.Selected[i].transform.SetAsFirstSibling();
        }
    }

    void BringToFrontBy1(object obj)
    {
        for (int i = 0; i < UIEditorSelectionHelpers.Selected.Count; ++i)
        {
            int index = UIEditorSelectionHelpers.Selected[i].transform.GetSiblingIndex();
            UIEditorSelectionHelpers.Selected[i].transform.SetSiblingIndex(index + 1);

        }
    }

    void SendToBackBy1(object obj)
    {
        for (int i = 0; i < UIEditorSelectionHelpers.Selected.Count; ++i)
        {
            int index = UIEditorSelectionHelpers.Selected[i].transform.GetSiblingIndex();
            UIEditorSelectionHelpers.Selected[i].transform.SetSiblingIndex(index - 1);
        }
    }

    void SendTopFront(object obj)
    {
        for (int i = 0; i < UIEditorSelectionHelpers.Selected.Count; ++i)
        {
            UIEditorSelectionHelpers.Selected[i].transform.SetAsLastSibling();
        }
    }
}

