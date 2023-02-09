//-----------------------------------------------------------------------------------------
// UI Editor
// Copyright © Argiris Baltzis - All Rights Reserved
//
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
//-----------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

public static class UIEditorHelpers
{
    public class ZoomScaleData
    {
        public string Display;
        public float ScaleAmount;

        public ZoomScaleData(string display, float scale)
        {
            Display = display;
            ScaleAmount = scale;
        }
    }

    public static void SetDirtyAllComponentsAndGameObject(GameObject gameObject)
    {
        Component[] components = gameObject.GetComponents<Component>();
        for (int i = 0; i < components.Length; ++i)
            EditorUtility.SetDirty(components[i]);

        EditorUtility.SetDirty(gameObject);
    }

    public static ZoomScaleData[] ZoomScales = new ZoomScaleData[]
    {
        new ZoomScaleData("40%", 0.4f),
        new ZoomScaleData("50%", 0.5f),
        new ZoomScaleData("60%", 0.6f),
        new ZoomScaleData("70%", 0.7f),
        new ZoomScaleData("80%", 0.8f),
        new ZoomScaleData("90%", 0.9f),
        new ZoomScaleData("100%", 1.0f),
        new ZoomScaleData("110%", 1.1f),
        new ZoomScaleData("120%", 1.2f),
        new ZoomScaleData("130%", 1.3f),
        new ZoomScaleData("140%", 1.4f),
        new ZoomScaleData("150%", 1.5f),
        new ZoomScaleData("160%", 1.6f),
        new ZoomScaleData("180%", 1.8f),
        new ZoomScaleData("200%", 2.0f),
        new ZoomScaleData("300%", 3.0f),
        new ZoomScaleData("400%", 4.0f),
    };

    public static int DefaultZoomIndex()
    {
        return 6;
    }

    public static float GetZoomScaleFactor()
    {
        return ZoomScales[UIEditorVariables.ZoomIndex].ScaleAmount;
    }

    public static float GetZoomScaleFactorFromIndex(int index)
    {
        return ZoomScales[index].ScaleAmount;
    }

    public static string[] GetZoomScalesText()
    {
        List<string> zoomText = new List<string>();
        for (int i = 0; i < ZoomScales.Length; ++i)
        {
            zoomText.Add(ZoomScales[i].Display);
        }

        return zoomText.ToArray();
    }

    public static Canvas FindRootCanvas(GameObject gameObject)
    {
        Canvas canvas = gameObject.GetComponentInParent<Canvas>();
        if (canvas != null && canvas.isRootCanvas)
        {
            return canvas;
        }
        else if (canvas == null || canvas.transform.parent == null)
        {
            return null;
        }
        else
        {
            return FindRootCanvas(canvas.gameObject.transform.parent.gameObject);
        }
    }

    public static Color Color(byte r, byte g, byte b, byte a)
    {
        return new Color((float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f, (float)a / 255.0f);
    }

    public static Color Color(byte r, byte g, byte b)
    {
        return Color(r, g, b, 255);
    }

    public static string ColorToString(Color color)
    {
        return color.r.ToString() + ", " + color.g.ToString() + ", " + color.b.ToString() + ", " + color.a.ToString() + ", ";

    }

    public static bool IsParentInList(List<GameObject> list, GameObject startingObject)
    {
        if (startingObject.transform.parent != null)
        {
            if (list.Contains(startingObject.transform.parent.gameObject))
            {
                return true;
            }
            else
            {
                return IsParentInList(list, startingObject.transform.parent.gameObject);
            }
        }

        return false;
    }

    public static bool IsLayerOn(int layerMask, string layerName)
    {
        if (layerMask == (layerMask | (1 << LayerMask.NameToLayer(layerName))))
            return true;

        return false;
    }

    public static int SwitchLayerOn(int layerMask, string layerName)
    {
        layerMask |= 1 << LayerMask.NameToLayer(layerName);
        return layerMask;
    }

    public static int SwitchLayerOff(int layerMask, string layerName)
    {
        layerMask &= ~(1 << LayerMask.NameToLayer(layerName));
        return layerMask;
    }

    public static void ToggleLayerOnOff(int layerMask, string layerName)
    {
        layerMask ^= 1 << LayerMask.NameToLayer(layerName);
    }

    public static void HideUILayer()
    {
        LayerMask layerNumberBinary = 1 << LayerMask.NameToLayer("UI");
        LayerMask flippedVisibleLayers = ~Tools.visibleLayers;
        Tools.visibleLayers = ~(flippedVisibleLayers | layerNumberBinary);
    }

    public static void ShowUILayer()
    {
        LayerMask layerNumberBinary = 1 << LayerMask.NameToLayer("UI");
        Tools.visibleLayers = Tools.visibleLayers |= layerNumberBinary;
    }

     public static Color ParseColor (string col) 
     {
         string[] strings = col.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);

         Color output = new Color(1, 1, 1, 1);
         for (var i = 0; i < 4; i++) 
         {
              output[i] = System.Single.Parse(strings[i]);
         }

         return output;
     }

    public static void SetLayerRecursively(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;

        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            SetLayerRecursively(gameObject.transform.GetChild(i).gameObject, layer);
        }
    }

    public static T GetComponentInChildren<T>(GameObject root) where T : Component
    {
        T found = root.GetComponent<T>();
        if (found != null)
        {
            return found;
        }
        else
        {
            for (int i = 0; i < root.transform.childCount; ++i)
            {
                found = GetComponentInChildren<T>(root.transform.GetChild(i).gameObject);
                if (found != null) return found;
            }
        }

        return null;
    }

    public static Vector2 GetParentSize(GameObject gameObject)
    {
        if (gameObject.transform.parent != null)
        {
            RectTransform rectTransform = gameObject.transform.parent.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                return new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
            }
        }

        return Vector2.zero;
    }

    public static List<Rect> Get8ScaleBoxesAroundObject(UIEditorWindow.ObjectData myObjectData, float zoomScale, Vector2 zoomScaleOffset, bool useLargeSize)
    {
        float smallSquareSize = 4;
        float extraPadding = 1;

        if (useLargeSize) smallSquareSize += 2;

        List<Rect> rectList = new List<Rect>();

        Vector2 topLeft = new Vector2(myObjectData.Rect.xMin, myObjectData.Rect.yMin);
        Vector2 topRight = new Vector2(myObjectData.Rect.xMax, myObjectData.Rect.yMin);
        Vector2 bottomLeft = new Vector2(myObjectData.Rect.xMin, myObjectData.Rect.yMax);
        Vector2 bottomRight = new Vector2(myObjectData.Rect.xMax, myObjectData.Rect.yMax);

        topLeft = UIEditorHelpers.TransformCoordToScreenPixels(topLeft, zoomScale, zoomScaleOffset);
        topRight = UIEditorHelpers.TransformCoordToScreenPixels(topRight, zoomScale, zoomScaleOffset);
        bottomLeft = UIEditorHelpers.TransformCoordToScreenPixels(bottomLeft, zoomScale, zoomScaleOffset);
        bottomRight = UIEditorHelpers.TransformCoordToScreenPixels(bottomRight, zoomScale, zoomScaleOffset);

        float horizontalMid = topLeft.x + (((topRight.x - topLeft.x) - smallSquareSize) / 2);
        float verticalMid = topLeft.y + (((bottomRight.y - topRight.y) - smallSquareSize) / 2);


        // TOP
        rectList.Add(new Rect(topLeft.x - smallSquareSize - extraPadding, topLeft.y - smallSquareSize - extraPadding, smallSquareSize, smallSquareSize));
        rectList.Add(new Rect(horizontalMid, topLeft.y - smallSquareSize - extraPadding, smallSquareSize, smallSquareSize));
        rectList.Add(new Rect(topRight.x + extraPadding, topLeft.y - smallSquareSize - extraPadding, smallSquareSize, smallSquareSize));

        // MIDDLE
        rectList.Add(new Rect(topLeft.x - smallSquareSize - extraPadding, verticalMid, smallSquareSize, smallSquareSize));
        rectList.Add(new Rect(topRight.x + extraPadding, verticalMid, smallSquareSize, smallSquareSize));

        // BOTTOM
        rectList.Add(new Rect(topLeft.x - smallSquareSize - extraPadding, bottomLeft.y + extraPadding, smallSquareSize, smallSquareSize));
        rectList.Add(new Rect(horizontalMid, bottomLeft.y + extraPadding, smallSquareSize, smallSquareSize));
        rectList.Add(new Rect(topRight.x + extraPadding, bottomLeft.y + extraPadding, smallSquareSize, smallSquareSize));

        return rectList;
    }


    public static void SetPositionToMouseAt(Vector2 mousePosition, RectTransform rectTransform, UIEditorWindow window)
    {
        //rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        //rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

        //float canvasScaleFactor = 1.0f;
        float canvasScaleFactorInverse = 1.0f;

        Canvas canvas = rectTransform.GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            //canvasScaleFactor = canvas.scaleFactor;
            canvasScaleFactorInverse = 1.0f / canvas.scaleFactor;
        }

        float parentWidth = UIEditorVariables.DeviceWidth;
        float parentHeight = UIEditorVariables.DeviceHeight;
        RectTransform parentRect = rectTransform.parent.GetComponent<RectTransform>();
        if (parentRect != null && parentRect.gameObject == canvas.gameObject)
        {

        }
        else if (parentRect != null)
        {
            parentWidth = parentRect.rect.width;
            parentHeight = parentRect.rect.height;
        }

        // float gridSizeAdjusted = UIEditorVariables.GridSize * UIEditorHelpers.GetZoomScaleFactor() * canvas.scaleFactor;


        Vector2 transformedPosition = mousePosition - (UIEditorVariables.SceneScrolling * UIEditorHelpers.GetZoomScaleFactor());
        Vector2 screenCenterOffset = new Vector2(UIEditorVariables.DeviceWidth - (UIEditorVariables.DeviceWidth * UIEditorHelpers.GetZoomScaleFactor()), UIEditorVariables.DeviceHeight - (UIEditorVariables.DeviceHeight * UIEditorHelpers.GetZoomScaleFactor()));
        transformedPosition -= screenCenterOffset / 2;
        transformedPosition *= window.InverseZoomScale;
        transformedPosition.y *= -1;


        Vector2 anchorPositionAtParent = new Vector2(
           parentWidth * rectTransform.anchorMin.x,
                        parentHeight * (1 - rectTransform.anchorMax.y));

        transformedPosition.x -= anchorPositionAtParent.x;
        transformedPosition.y += anchorPositionAtParent.y;

        transformedPosition.x = (int)(transformedPosition.x * canvasScaleFactorInverse);
        transformedPosition.y = (int)(transformedPosition.y * canvasScaleFactorInverse);

        rectTransform.anchoredPosition = transformedPosition;
    }

    public static bool IsMyChild(GameObject child, GameObject owner)
    {
        for (int i = 0; i < owner.transform.childCount; ++i)
        {
            if (owner.transform.GetChild(i).gameObject == child) return true;

            if (IsMyChild(child, owner.transform.GetChild(i).gameObject))
                return true;
        }

        return false;
    }


    public static void OnAfterCreateControl(Vector2 mousePosition, UIEditorWindow window)
    {
        RectTransform rect = Selection.activeGameObject.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.position = Vector3.zero;
            rect.localPosition = Vector3.zero;
            rect.localRotation = Quaternion.identity;
            rect.localScale = Vector3.one;

            if (rect.anchorMin == Vector2.zero && rect.anchorMax == Vector2.one)
            {
                // stretched objects should not be positioned
            }
            else
            {
                UIEditorHelpers.SetPositionToMouseAt(mousePosition, rect, window);
            }

            if (UIEditorSelectionHelpers.MouseOverObject != null)
            {
                rect.SetParent(UIEditorSelectionHelpers.MouseOverObject.GameObject.transform);

                // reset stretching position after parent change
                if (rect.anchorMin == Vector2.zero && rect.anchorMax == Vector2.one)
                {
                    rect.offsetMin = Vector2.zero;
                    rect.offsetMax = Vector2.zero;
                }
            }
        }
    }


    /// <summary>
    /// Used to get assets of a certain type and file extension from entire project
    public static T[] GetAssetsOfType<T>(string fileExtension) where T : UnityEngine.Object
    {
        List<T> tempObjects = new List<T>();
        DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
        FileInfo[] goFileInfo = directory.GetFiles("*" + fileExtension, SearchOption.AllDirectories);

        int i = 0; int goFileInfoLength = goFileInfo.Length;
        FileInfo tempGoFileInfo; string tempFilePath;
        T tempGO;
        for (; i < goFileInfoLength; i++)
        {
            tempGoFileInfo = goFileInfo[i];
            if (tempGoFileInfo == null)
                continue;

            tempFilePath = tempGoFileInfo.FullName;
            tempFilePath = tempFilePath.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
            tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, typeof(T)) as T;
            if (tempGO == null)
            {
                continue;
            }
            else if (!(tempGO is T))
            {
                continue;
            }

            tempObjects.Add(tempGO);
        }

        return tempObjects.ToArray();
    }

    public static void CreateBoundingGrabRects(out Rect top, out Rect bottom, out Rect left, out Rect right, float paddingSize, Rect transformedRect)
    {
        top = new Rect(transformedRect.x - paddingSize, transformedRect.y - paddingSize, transformedRect.width + (paddingSize * 2), paddingSize);
        
        bottom = new Rect(transformedRect.x - paddingSize, transformedRect.y + transformedRect.height, transformedRect.width + (paddingSize * 2), paddingSize);
        
        left = new Rect(transformedRect.x - paddingSize, transformedRect.y - paddingSize, paddingSize, transformedRect.height + (paddingSize*2));
        
        right = new Rect(transformedRect.x + transformedRect.width, transformedRect.y - paddingSize, paddingSize, transformedRect.height + (paddingSize * 2));
    }

    public static void CreateBoundingRectWithInsideAndOutsideBorder(out Rect top, out Rect bottom, out Rect left, out Rect right, float paddingSize, Rect transformedRect)
    {
        top = new Rect(transformedRect.x - paddingSize, transformedRect.y - paddingSize, transformedRect.width + (paddingSize * 2), paddingSize * 2);

        bottom = new Rect(transformedRect.x - paddingSize, transformedRect.y + transformedRect.height - paddingSize, transformedRect.width + (paddingSize * 2), paddingSize * 2);

        left = new Rect(transformedRect.x - paddingSize, transformedRect.y - paddingSize, paddingSize * 2, transformedRect.height + (paddingSize * 2));

        right = new Rect(transformedRect.x + transformedRect.width - paddingSize, transformedRect.y - paddingSize, paddingSize * 2, transformedRect.height + (paddingSize * 2));
    }


    public static Vector2 TransformCoordToScreenPixels(Vector2 resizedRectTop, float windowScale, Vector2 windowZoomPositionOffset)
    {
        Vector2 scrolling = UIEditorVariables.SceneScrolling;
        resizedRectTop.x += scrolling.x;
        resizedRectTop.y += scrolling.y;


        float totalScale = windowScale;

        resizedRectTop = new Vector2(resizedRectTop.x * totalScale, resizedRectTop.y * totalScale);

        resizedRectTop.x += windowZoomPositionOffset.x;
        resizedRectTop.y += windowZoomPositionOffset.y;

        return resizedRectTop;

    }

    public static Rect TransformRectToScreenPixels(Rect resizedRectTop, float windowScale, Vector2 windowZoomPositionOffset)
    {
        Vector2 scrolling = UIEditorVariables.SceneScrolling;
        resizedRectTop.x += scrolling.x;
        resizedRectTop.y += scrolling.y;


        float totalScale = windowScale;

        resizedRectTop = new Rect(resizedRectTop.x * totalScale, resizedRectTop.y * totalScale, resizedRectTop.width * totalScale, resizedRectTop.height * totalScale);

        resizedRectTop.x += windowZoomPositionOffset.x;
        resizedRectTop.y += windowZoomPositionOffset.y;

        return resizedRectTop;

    }

    public static void TransformBoundingGrabRects(ref Rect resizedRectTop, ref Rect resizedRectBottom, ref Rect resizedRectLeft, ref Rect resizedRectRight, float windowScale, Vector2 windowZoomPositionOffset)
    {
        Vector2 scrolling = UIEditorVariables.SceneScrolling;
        resizedRectTop.x += scrolling.x;
        resizedRectTop.y += scrolling.y;

        resizedRectBottom.x += scrolling.x;
        resizedRectBottom.y += scrolling.y;

        resizedRectLeft.x += scrolling.x;
        resizedRectLeft.y += scrolling.y;

        resizedRectRight.x += scrolling.x;
        resizedRectRight.y += scrolling.y;

        float totalScale = windowScale;

        resizedRectTop = new Rect(resizedRectTop.x * totalScale, resizedRectTop.y * totalScale, resizedRectTop.width * totalScale, resizedRectTop.height * totalScale);
        resizedRectBottom = new Rect(resizedRectBottom.x * totalScale, resizedRectBottom.y * totalScale, resizedRectBottom.width * totalScale, resizedRectBottom.height * totalScale);
        resizedRectLeft = new Rect(resizedRectLeft.x * totalScale, resizedRectLeft.y * totalScale, resizedRectLeft.width * totalScale, resizedRectLeft.height * totalScale);
        resizedRectRight = new Rect(resizedRectRight.x * totalScale, resizedRectRight.y * totalScale, resizedRectRight.width * totalScale, resizedRectRight.height * totalScale);

        resizedRectTop.x += windowZoomPositionOffset.x;
        resizedRectTop.y += windowZoomPositionOffset.y;

        resizedRectBottom.x += windowZoomPositionOffset.x;
        resizedRectBottom.y += windowZoomPositionOffset.y;

        resizedRectLeft.x += windowZoomPositionOffset.x;
        resizedRectLeft.y += windowZoomPositionOffset.y;

        resizedRectRight.x += windowZoomPositionOffset.x;
        resizedRectRight.y += windowZoomPositionOffset.y;
    }

    public static EditorWindow GetFocusedWindow(string window)
    {
        FocusOnWindow(window);
        return EditorWindow.focusedWindow;
    }

    public static void FocusOnWindow(string window)
    {
        EditorApplication.ExecuteMenuItem("Window/" + window);
    }

    //public static Rect GetTransformedRectInScreenCoordinates(GameObject gameObject, Canvas canvas)
    //{
    //    return GetScreenRect(gameObject.GetComponent<RectTransform>(), canvas);
    //}


    public static Rect GetScreenRect(RectTransform rectTransform, Canvas canvas, ref Vector3[] corners)
    {
        rectTransform.GetWorldCorners(corners);

        float xMin = float.PositiveInfinity;
        float xMax = float.NegativeInfinity;
        float yMin = float.PositiveInfinity;
        float yMax = float.NegativeInfinity;

        for (int i = 0; i < 4; i++)
        {
            Vector3 screenCoord = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[i]);
            screenCoord.y = canvas.pixelRect.height - screenCoord.y;

            if (screenCoord.x < xMin)
                xMin = screenCoord.x;
            if (screenCoord.x > xMax)
                xMax = screenCoord.x;
            if (screenCoord.y < yMin)
                yMin = screenCoord.y;
            if (screenCoord.y > yMax)
                yMax = screenCoord.y;

            corners[i] = screenCoord;
        }

        Rect result = new Rect(xMin, yMin, xMax - xMin, yMax - yMin);

        return result;
    }
}

