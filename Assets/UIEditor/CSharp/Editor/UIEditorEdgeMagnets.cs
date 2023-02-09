using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class UIEditorEdgeMagnets
{
    public enum MagnetizedSideId
    {
        None = 0,
        Left,
        Top,
        Right,
        Bottom,
    }

    public class MagnetGroup
    {
        public MagnetizedSideId Side;
        public RectTransform UIObject;
    }

    public List<MagnetGroup> Magnetized = new List<MagnetGroup>();
    public Vector2 MagnetizedPosition;
    public Vector2 MagnetizedOffsetMin;
    public Vector2 MagnetizedOffsetMax;

    private const float MagnetRadius = 7;
    private const float GapBetweenMagnetLines = 8;

    public void Clear()
    {
        Magnetized.Clear();
    }


    public bool IsMagnetized(RectTransform rect, MagnetizedSideId side)
    {
        for (int i = 0; i < Magnetized.Count; ++i)
            if (Magnetized[i].UIObject == rect && side == Magnetized[i].Side)
                return true;

        return false;
    }

    public void CreateMagnet(RectTransform rect, MagnetizedSideId side)
    {
        Magnetized.Add(
            new MagnetGroup() { UIObject = rect, Side = side }
            );
    }

    public void RemoveMagnet(RectTransform rect, MagnetizedSideId side)
    {
        for (int i = 0; i < Magnetized.Count; ++i)
        {
            if (Magnetized[i].UIObject == rect && side == Magnetized[i].Side)
            {
                Magnetized.RemoveAt(i);
                break;
            }
        }
    }

    private Rect GetObjectBoundingBox(RectTransform rectTransform)
    {
        Rect newRect = new Rect(
            rectTransform.offsetMin.x,
            rectTransform.offsetMax.y,
            rectTransform.rect.width,
            rectTransform.rect.height);

        return newRect;

    }

    private float GetFarRight(RectTransform rectTransform)
    {
        float farRight = UIEditorVariables.DeviceWidth;
        if (rectTransform.parent != null && rectTransform.parent.GetComponent<RectTransform>() != null)
            farRight = rectTransform.parent.GetComponent<RectTransform>().rect.width;
        return farRight;
    }

    private float GetFarBottom(RectTransform rectTransform)
    {
        float farBottom = -UIEditorVariables.DeviceHeight;
        if (rectTransform.parent != null && rectTransform.parent.GetComponent<RectTransform>() != null)
            farBottom = -rectTransform.parent.GetComponent<RectTransform>().rect.height;

        return farBottom;
    }

    public bool ShouldBeMagnetized(RectTransform myRectTransform, Vector2 position, Vector2 offsetMin, Vector2 offsetMax, MagnetizedSideId side, out float finalPosition)
    {
        float parentWidth = UIEditorVariables.DeviceWidth;
        float parentHeight = UIEditorVariables.DeviceHeight;

        float canvasMagnetRadius = MagnetRadius;

        Canvas parentCanvas = myRectTransform.GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            canvasMagnetRadius *= 1 / parentCanvas.scaleFactor;
        }

        if (myRectTransform.parent != null && myRectTransform.parent.GetComponent<RectTransform>() != null)
        {
            parentWidth = myRectTransform.parent.GetComponent<RectTransform>().rect.width;
            parentHeight = myRectTransform.parent.GetComponent<RectTransform>().rect.height;
        }

        List<float> horizontalPositions = new List<float>();
        List<float> verticalPositions = new List<float>();
        horizontalPositions.Add(0);
        horizontalPositions.Add(parentWidth);
        verticalPositions.Add(0);
        verticalPositions.Add(parentHeight);

        // Gap between objects
        //horizontalPositions.Add(0 + GapBetweenMagnetLines);
        //horizontalPositions.Add(parentWidth - GapBetweenMagnetLines);
        //verticalPositions.Add(0 + GapBetweenMagnetLines);
        //verticalPositions.Add(parentHeight - GapBetweenMagnetLines);


        List<float> neighborHorizontalPositions = new List<float>();
        List<float> neighborVerticalPositions = new List<float>();


        //SameParentRectTranforms.Clear();
        if (myRectTransform.parent != null)
        {


            for (int i = 0; i < myRectTransform.parent.transform.childCount; ++i)
            {
                RectTransform childTransform = myRectTransform.parent.transform.GetChild(i).GetComponent<RectTransform>();
                if (childTransform == null) continue;
                if (childTransform == myRectTransform) continue;
                if (UIEditorSelectionHelpers.Selected.Contains(childTransform.gameObject)) continue;

                if (childTransform != null)
                {
                    // stretched
                    if (childTransform.anchorMin.x != childTransform.anchorMax.x)
                    {
                        // neighborHorizontalPositions.Add(childTransform.offsetMin.x);
                        // neighborHorizontalPositions.Add(childTransform.offsetMax.x);
                    }
                    else
                    {
                        float farLeftX = childTransform.anchorMin.x * parentWidth;
                        farLeftX += childTransform.anchoredPosition.x;
                        farLeftX -= childTransform.rect.width * childTransform.pivot.x;

                        // with 0 being far left, how far on right are we?
                        neighborHorizontalPositions.Add(farLeftX);

                        float farRightX = childTransform.anchorMin.x * parentWidth;
                        farRightX += childTransform.anchoredPosition.x;
                        farRightX += childTransform.rect.width * (1 - childTransform.pivot.x);

                        // with 0 being far left, how far on right are we?
                        neighborHorizontalPositions.Add(farRightX);
                    }


                    if (childTransform.anchorMin.y != childTransform.anchorMax.y)
                    {

                    }
                    else
                    {
                        float farTopY = (1 - childTransform.anchorMin.y) * parentHeight;
                        farTopY -= childTransform.anchoredPosition.y;
                        farTopY -= childTransform.rect.height * (1 - childTransform.pivot.y);

                        // with 0 being far left, how far on right are we?
                        neighborVerticalPositions.Add(farTopY);

                        float farBottomY = (1 - childTransform.anchorMin.y) * parentHeight;
                        farBottomY -= childTransform.anchoredPosition.y;
                        farBottomY += childTransform.rect.height * childTransform.pivot.y;

                        // with 0 being far left, how far on right are we?
                        neighborVerticalPositions.Add(farBottomY);
                    }
                }
            }
        }


        finalPosition = 0;

        // Non-Stretched
        float myFarLeftX = myRectTransform.anchorMin.x * parentWidth;
        myFarLeftX += position.x;
        myFarLeftX -= myRectTransform.rect.width * myRectTransform.pivot.x;

        float myFarRightX = myRectTransform.anchorMin.x * parentWidth;
        myFarRightX += position.x;
        myFarRightX += myRectTransform.rect.width * (1 - myRectTransform.pivot.x);

        float myFarTopY = (1 - myRectTransform.anchorMin.y) * parentHeight;
        myFarTopY -= position.y;
        myFarTopY -= myRectTransform.rect.height * (1 - myRectTransform.pivot.y);

        float myFarBottomY = (1 - myRectTransform.anchorMin.y) * parentHeight;
        myFarBottomY -= position.y;
        myFarBottomY += myRectTransform.rect.height * myRectTransform.pivot.y;

        switch (side)
        {
            case MagnetizedSideId.Left:
                {
                    // Against parent
                    if (UIEditorVariables.EdgeParentSnap && !UIEditorVariables.DisableAllSnap)
                    {
                        float relativeAnchorPosition = myRectTransform.anchorMin.x * parentWidth;
                        relativeAnchorPosition += offsetMin.x;

                        for (int i = 0; i < horizontalPositions.Count; ++i)
                        {
                            if (relativeAnchorPosition >= (horizontalPositions[i] - canvasMagnetRadius) && relativeAnchorPosition <= (horizontalPositions[i] + canvasMagnetRadius))
                            {
                                finalPosition = myRectTransform.anchoredPosition.x - relativeAnchorPosition + horizontalPositions[i];
                                //Debug.Log("LEFT MAGNET: ON");
                                return true;
                            }
                        }
                    }

                    // Against neighbors
                    if (UIEditorVariables.NeighborEdgeSnap && !UIEditorVariables.DisableAllSnap)

                    {
                        for (int i = 0; i < neighborHorizontalPositions.Count; ++i)
                        {
                            if (myFarLeftX >= (neighborHorizontalPositions[i] - canvasMagnetRadius) && myFarLeftX <= (neighborHorizontalPositions[i] + canvasMagnetRadius))
                            {
                                finalPosition = myRectTransform.anchoredPosition.x - (myFarLeftX - neighborHorizontalPositions[i]);
                                //Debug.Log("NEIGHBOR LEFT MAGNET: ON");
                                return true;
                            }
                        }
                    }



                    break;
                }

            case MagnetizedSideId.Top:
                {
                    // Against parent
                    if (UIEditorVariables.EdgeParentSnap && !UIEditorVariables.DisableAllSnap)

                    {
                        float relativeAnchorPosition = (1 - myRectTransform.anchorMin.y) * parentHeight;
                        relativeAnchorPosition -= offsetMax.y;

                        for (int i = 0; i < verticalPositions.Count; ++i)
                        {
                            if (relativeAnchorPosition >= (verticalPositions[i] - canvasMagnetRadius) && relativeAnchorPosition <= (verticalPositions[i] + canvasMagnetRadius))
                            {
                                finalPosition = myRectTransform.anchoredPosition.y + (relativeAnchorPosition - verticalPositions[i]);
                                //Debug.Log("TOP MAGNET: ON");

                                return true;
                            }
                        }
                    }

                    // Against neighbors
                    if (UIEditorVariables.NeighborEdgeSnap && !UIEditorVariables.DisableAllSnap)

                    {
                        for (int i = 0; i < neighborVerticalPositions.Count; ++i)
                        {
                            if (myFarTopY >= (neighborVerticalPositions[i] - canvasMagnetRadius) && myFarTopY <= (neighborVerticalPositions[i] + canvasMagnetRadius))
                            {
                                finalPosition = myRectTransform.anchoredPosition.y + (myFarTopY - neighborVerticalPositions[i]);
                                // Debug.Log("NEIGHBOR TOP MAGNET: ON");
                                return true;
                            }
                        }
                    }

                    break;
                }

            case MagnetizedSideId.Right:
                {
                    if (UIEditorVariables.EdgeParentSnap && !UIEditorVariables.DisableAllSnap)
                    {
                        float relativeAnchorPosition = myRectTransform.anchorMax.x * parentWidth;
                        relativeAnchorPosition += offsetMax.x;

                        for (int i = 0; i < horizontalPositions.Count; ++i)
                        {
                            if ((relativeAnchorPosition) >= (horizontalPositions[i] - canvasMagnetRadius) && (relativeAnchorPosition) <= (horizontalPositions[i] + canvasMagnetRadius))
                            {
                                finalPosition = myRectTransform.anchoredPosition.x + (horizontalPositions[i] - relativeAnchorPosition);
                                // Debug.Log("RIGHT MAGNET: ON");

                                return true;
                            }
                        }
                    }

                    // Against neighbors
                    if (UIEditorVariables.NeighborEdgeSnap && !UIEditorVariables.DisableAllSnap)

                    {
                        for (int i = 0; i < neighborHorizontalPositions.Count; ++i)
                        {
                            if (myFarRightX >= (neighborHorizontalPositions[i] - canvasMagnetRadius) && myFarRightX <= (neighborHorizontalPositions[i] + canvasMagnetRadius))
                            {
                                finalPosition = myRectTransform.anchoredPosition.x - (myFarRightX - neighborHorizontalPositions[i]);
                                //Debug.Log("NEIGHBOR RIGHT MAGNET: ON");
                                return true;
                            }
                        }
                    }

                    break;
                }

            case MagnetizedSideId.Bottom:
                {
                    // Against parent
                    if (UIEditorVariables.EdgeParentSnap && !UIEditorVariables.DisableAllSnap)

                    {
                        float relativeAnchorPosition = (1 - myRectTransform.anchorMax.y) * parentHeight;
                        relativeAnchorPosition -= offsetMin.y;

                        for (int i = 0; i < verticalPositions.Count; ++i)
                        {
                            if ((relativeAnchorPosition) >= (verticalPositions[i] - canvasMagnetRadius) && (relativeAnchorPosition) <= (verticalPositions[i] + canvasMagnetRadius))
                            {
                                finalPosition = myRectTransform.anchoredPosition.y - (verticalPositions[i] - relativeAnchorPosition);
                                //Debug.Log("BOTTOm MAGNET: ON");
                                return true;
                            }
                        }
                    }


                    // Against neighbors
                    if (UIEditorVariables.NeighborEdgeSnap && !UIEditorVariables.DisableAllSnap)

                    {
                        for (int i = 0; i < neighborVerticalPositions.Count; ++i)
                        {
                            if (myFarBottomY >= (neighborVerticalPositions[i] - canvasMagnetRadius) && myFarBottomY <= (neighborVerticalPositions[i] + canvasMagnetRadius))
                            {
                                finalPosition = myRectTransform.anchoredPosition.y + (myFarBottomY - neighborVerticalPositions[i]);
                                // Debug.Log("NEIGHBOR TOP MAGNET: ON");
                                return true;
                            }
                        }
                    }

                    break;
                }
        }


        return false;
    }

    public void HandleMagnetMovement(RectTransform rectTransform, Vector2 finalAmount, bool allowEdgeMagnets)
    {
        //float parentWidth = UIEditorVariables.DeviceWidth;
        //float parentHeight = UIEditorVariables.DeviceHeight;

        //if (rectTransform.parent != null && rectTransform.parent.GetComponent<RectTransform>() != null)
        //{
        //    parentWidth = rectTransform.parent.GetComponent<RectTransform>().rect.width;
        //    parentHeight = rectTransform.parent.GetComponent<RectTransform>().rect.height;
        //}

        float finalPosition = 0;

        // LEFT
        if (allowEdgeMagnets && IsMagnetized(rectTransform, MagnetizedSideId.Left))
        {
            MagnetizedPosition.x += finalAmount.x;
            MagnetizedOffsetMin.x += finalAmount.x;
            MagnetizedOffsetMax.x += finalAmount.x;

            if (ShouldBeMagnetized(rectTransform, MagnetizedPosition, MagnetizedOffsetMin, MagnetizedOffsetMax, MagnetizedSideId.Left, out finalPosition))
            {
                // do nothing, its already magnetized
            }
            else
            {
                // De-magnetize
                RemoveMagnet(rectTransform, MagnetizedSideId.Left);
                rectTransform.anchoredPosition = new Vector2(MagnetizedPosition.x, rectTransform.anchoredPosition.y);

            }
        }
        else if (allowEdgeMagnets && IsMagnetized(rectTransform, MagnetizedSideId.Right))
        {
            MagnetizedPosition.x += finalAmount.x;
            MagnetizedOffsetMin.x += finalAmount.x;
            MagnetizedOffsetMax.x += finalAmount.x;

            if (ShouldBeMagnetized(rectTransform, MagnetizedPosition, MagnetizedOffsetMin, MagnetizedOffsetMax, MagnetizedSideId.Right, out finalPosition))
            {
                // do nothing, its already magnetized
            }
            else
            {
                // De-magnetize
                RemoveMagnet(rectTransform, MagnetizedSideId.Right);
                rectTransform.anchoredPosition = new Vector2(MagnetizedPosition.x, rectTransform.anchoredPosition.y);

            }
        }
        else
        {
            rectTransform.anchoredPosition += new Vector2(finalAmount.x, 0);

            MagnetizedSideId magnetSide = MagnetizedSideId.None;

            // APPLY LEFT MAGNET
            if (allowEdgeMagnets && ShouldBeMagnetized(rectTransform, rectTransform.anchoredPosition, rectTransform.offsetMin, rectTransform.offsetMax, MagnetizedSideId.Left, out finalPosition))
            {
                //float relativeAnchorPosition = rectTransform.anchorMin.x * parentWidth;
                // relativeAnchorPosition += rectTransform.offsetMin.x;

                magnetSide = MagnetizedSideId.Left;
                rectTransform.anchoredPosition = new Vector2(finalPosition, rectTransform.anchoredPosition.y);
            }

            // APPLY RIGHT MAGNET
            else if (allowEdgeMagnets && ShouldBeMagnetized(rectTransform, rectTransform.anchoredPosition, rectTransform.offsetMin, rectTransform.offsetMax, MagnetizedSideId.Right, out finalPosition))
            {
                //float relativeAnchorPosition = rectTransform.anchorMax.x * parentWidth;
                //relativeAnchorPosition += rectTransform.offsetMax.x;

                magnetSide = MagnetizedSideId.Right;
                rectTransform.anchoredPosition = new Vector2(finalPosition, rectTransform.anchoredPosition.y);
            }

            if (magnetSide != MagnetizedSideId.None)
            {
                CreateMagnet(rectTransform, magnetSide);
                MagnetizedPosition.x = rectTransform.anchoredPosition.x;
                MagnetizedOffsetMin.x = rectTransform.offsetMin.x;
                MagnetizedOffsetMax.x = rectTransform.offsetMax.x;
            }
        }

        // TOP
        if (allowEdgeMagnets && IsMagnetized(rectTransform, MagnetizedSideId.Top))
        {
            MagnetizedPosition.y += finalAmount.y;
            MagnetizedOffsetMin.y += finalAmount.y;
            MagnetizedOffsetMax.y += finalAmount.y;

            if (allowEdgeMagnets && ShouldBeMagnetized(rectTransform, MagnetizedPosition, MagnetizedOffsetMin, MagnetizedOffsetMax, MagnetizedSideId.Top, out finalPosition))
            {
                // do nothing, its already magnetized
            }
            else
            {
                // De-magnetize
                RemoveMagnet(rectTransform, MagnetizedSideId.Top);
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, MagnetizedPosition.y);
            }
        }
        else if (allowEdgeMagnets && IsMagnetized(rectTransform, MagnetizedSideId.Bottom))
        {
            MagnetizedPosition.y += finalAmount.y;
            MagnetizedOffsetMin.y += finalAmount.y;
            MagnetizedOffsetMax.y += finalAmount.y;

            if (allowEdgeMagnets && ShouldBeMagnetized(rectTransform, MagnetizedPosition, MagnetizedOffsetMin, MagnetizedOffsetMax, MagnetizedSideId.Bottom, out finalPosition))
            {
                // do nothing, its already magnetized
            }
            else
            {
                // De-magnetize
                RemoveMagnet(rectTransform, MagnetizedSideId.Bottom);
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, MagnetizedPosition.y);
            }
        }
        else
        {
            rectTransform.anchoredPosition += new Vector2(0, finalAmount.y);

            MagnetizedSideId magnetSide = MagnetizedSideId.None;

            // APPLY TOP MAGNET
            if (allowEdgeMagnets && ShouldBeMagnetized(rectTransform, rectTransform.anchoredPosition, rectTransform.offsetMin, rectTransform.offsetMax, MagnetizedSideId.Top, out finalPosition))
            {
                // float relativeAnchorPosition = (1 - rectTransform.anchorMin.y) * parentHeight;
                // relativeAnchorPosition -= rectTransform.offsetMax.y;

                magnetSide = MagnetizedSideId.Top;
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, finalPosition);
            }

            // APPLY BOTTOM MAGNET
            else if (allowEdgeMagnets && ShouldBeMagnetized(rectTransform, rectTransform.anchoredPosition, rectTransform.offsetMin, rectTransform.offsetMax, MagnetizedSideId.Bottom, out finalPosition))
            {
                // float relativeAnchorPosition = (1 - rectTransform.anchorMax.y) * parentHeight;
                //  relativeAnchorPosition -= rectTransform.offsetMin.y;

                magnetSide = MagnetizedSideId.Bottom;
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, finalPosition);
            }

            if (magnetSide != MagnetizedSideId.None)
            {
                CreateMagnet(rectTransform, magnetSide);
                MagnetizedPosition.y = rectTransform.anchoredPosition.y;
                MagnetizedOffsetMin.y = rectTransform.offsetMin.y;
                MagnetizedOffsetMax.y = rectTransform.offsetMax.y;
            }
        }
    }
}

