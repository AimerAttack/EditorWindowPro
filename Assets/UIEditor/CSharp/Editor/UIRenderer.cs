//-----------------------------------------------------------------------------------------
// UI Editor
// Copyright © Argiris Baltzis - All Rights Reserved
//
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
//-----------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class UIRenderer
{
    //private Material EditorGridMaterial;
    private Material EditorMaterial;
    private Material CheckerGridMaterial;

    //private Texture2D WhiteTexture;
    private float editorWindowWidth;
    private float editorWindowHeight;

    private const float MiddleBoxSize = 12;
    private const float MiddleBoxSizeHalf = 6;

    private Texture2D DotedLineTexture;
    private Texture2D CheckerGridTexture;
    private const int ChekerGridSize = 30;

    public void PrepareDraw(float windowWidth, float windowHeight)
    {
        editorWindowWidth = windowWidth;
        editorWindowHeight = windowHeight;

        if (EditorMaterial == null)
        {
            EditorMaterial = new Material(Shader.Find("UIEDITOR/DefaultShader"));
            CheckerGridMaterial = new Material(Shader.Find("UIEDITOR/DefaultShader"));
        }

        if (DotedLineTexture == null)
        {
            DotedLineTexture = new Texture2D(2, 1, TextureFormat.ARGB32, false);
            DotedLineTexture.SetPixel(0, 0, new Color(1, 1, 1, 1));
            DotedLineTexture.SetPixel(1, 0, new Color(0, 0, 0, 0));
            DotedLineTexture.Apply();
        }

       if (CheckerGridTexture == null)
        {
            Color color1 = new Color(45 / 255.0f, 45 / 255.0f, 45 / 255.0f, 1.0f);
            Color color2 = new Color(46 / 255.0f, 46 / 255.0f, 46 / 255.0f, 1.0f);

            CheckerGridTexture = new Texture2D(ChekerGridSize, ChekerGridSize, TextureFormat.ARGB32, false);
            for (int i = 0; i < CheckerGridTexture.height; ++i)
            {
                for (int j = 0; j < CheckerGridTexture.width; ++j)
                {
                    Color color = color1;
                    if (i > (CheckerGridTexture.height / 2) && j > (CheckerGridTexture.width / 2))
                    {
                        color = color2;
                    }
                    else if (i < (CheckerGridTexture.height / 2) && j < (CheckerGridTexture.width / 2))
                    {
                        color = color2;
                    }

                    CheckerGridTexture.SetPixel(j, i, color);
                }
            }

            CheckerGridTexture.wrapMode = TextureWrapMode.Repeat;
            CheckerGridTexture.Apply();
        }


        //if (EditorGridMaterial == null)
        //{
        //    EditorGridMaterial = Resources.Load<Material>("Materials/EditorGridMaterial");
        //}

        //if (WhiteTexture == null)
        //{
        //    WhiteTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false, true);
        //    for (int i = 0; i < WhiteTexture.width; ++i)
        //    {
        //        for (int j = 0; j < WhiteTexture.height; ++j)
        //        {
        //            WhiteTexture.SetPixel(i, j, Color.white);
        //        }
        //    }
        //    //WhiteTexture = Resources.Load<Texture2D>("Textures/white");
        //}
    }


    void DrawRectangleUnscaled(Rect position, Color color, Vector2 scrolling, float zoomScale = 1.0f, bool clip = false)
    {
        //position.x += scrolling.x;
        //position.y += scrolling.y;
        //position = new Rect(position.x * zoomScale, position.y * zoomScale, position.width * zoomScale, position.height * zoomScale);

        //float deviceWidth = UIEditorVariables.DeviceWidth;
        //float deviceHeight = UIEditorVariables.DeviceHeight;

        //Vector2 offset = new Vector2(deviceWidth - (deviceWidth * zoomScale), deviceHeight - (deviceHeight * zoomScale));
        //position.x += offset.x / 2;
        //position.y += offset.y / 2;

        if (position.x > editorWindowWidth) return;
        if (position.y > editorWindowHeight) return;

        float uvHeight = 1;
        if (clip)
        {
            if (position.y < 0)
            {
                float originalHeight = position.height;
                float difference = -position.y;
                position = new Rect(position.x, position.y + difference, position.width, position.height - difference);

                uvHeight = position.height / originalHeight;
                if (uvHeight < 0)
                {
                    return; // not visible
                }
            }
        }

        GL.Begin(GL.QUADS);

        if (position.width < 1 && position.width > 0) position.width = 1;
        if (position.height < 1 && position.height > 0) position.height = 1;

        GL.Color(color);
        GL.Vertex3(position.x, (position.y + position.height), 0);
        GL.TexCoord(new Vector3(0, uvHeight, 0));

        GL.Color(color);
        GL.Vertex3(position.x, position.y, 0);
        GL.TexCoord(new Vector3(1, uvHeight, 0));

        GL.Color(color);
        GL.Vertex3((position.x + position.width), position.y, 0);
        GL.TexCoord(new Vector3(1, 0, 0));

        GL.Color(color);
        GL.Vertex3((position.x + position.width), (position.y + position.height), 0);
        GL.TexCoord(new Vector3(0, 0, 0));

        GL.End();
    }


    void DrawTriangleUnscaled(Vector2 A, Vector2 B, Vector3 C, Color color, Vector2 scrolling, float zoomScale = 1.0f, bool clip = false)
    {
        GL.Begin(GL.TRIANGLES);

        GL.Color(color);
        GL.Vertex3(A.x, A.y, 0);
        GL.TexCoord(new Vector3(0, 0, 0));

        GL.Color(color);
        GL.Vertex3(B.x, B.y, 0);
        GL.TexCoord(new Vector3(0, 0, 0));

        GL.Color(color);
        GL.Vertex3(C.x, C.y, 0);
        GL.TexCoord(new Vector3(0, 0, 0));

        GL.End();
    }

    void DrawQuadUnscaled(Vector2 A, Vector2 B, Vector3 C, Vector3 D, Color color)
    {
        GL.Begin(GL.TRIANGLES);

        GL.Color(color);
        GL.Vertex3(A.x, A.y, 0);
        GL.TexCoord(new Vector3(0, 0, 0));

        GL.Color(color);
        GL.Vertex3(B.x, B.y, 0);
        GL.TexCoord(new Vector3(0, 0, 0));

        GL.Color(color);
        GL.Vertex3(C.x, C.y, 0);
        GL.TexCoord(new Vector3(0, 0, 0));

        GL.Color(color);
        GL.Vertex3(D.x, D.y, 0);
        GL.TexCoord(new Vector3(0, 0, 0));

        GL.End();
    }

    private Vector2 TransformPoint(Vector2 position, Vector2 scrolling, float zoomScale = 1.0f)
    {
        position.x += scrolling.x;
        position.y += scrolling.y;
        position *= zoomScale;

        float deviceWidth = UIEditorVariables.DeviceWidth;
        float deviceHeight = UIEditorVariables.DeviceHeight;

        Vector2 offset = new Vector2(deviceWidth - (deviceWidth * zoomScale), deviceHeight - (deviceHeight * zoomScale));
        position.x += offset.x / 2;
        position.y += offset.y / 2;

        return position;
    }

    void DrawRectangle(Rect position, Color color, Vector2 scrolling, float zoomScale = 1.0f, bool clip = false)
    {
        position.x += scrolling.x;
        position.y += scrolling.y;
        position = new Rect(position.x * zoomScale, position.y * zoomScale, position.width * zoomScale, position.height * zoomScale);

        float deviceWidth = UIEditorVariables.DeviceWidth;
        float deviceHeight = UIEditorVariables.DeviceHeight;

        Vector2 offset = new Vector2(deviceWidth - (deviceWidth * zoomScale), deviceHeight - (deviceHeight * zoomScale));
        position.x += offset.x / 2;
        position.y += offset.y / 2;

        if (position.x > editorWindowWidth && position.xMax > editorWindowWidth) return;
        if (position.y > editorWindowHeight && position.yMax > editorWindowHeight) return;

        float uvHeight = 1;
        if (clip)
        {
            if (position.y < 0)
            {
                float originalHeight = position.height;
                float difference = -position.y;
                position = new Rect(position.x, position.y + difference, position.width, position.height - difference);

                uvHeight = position.height / originalHeight;
                if (uvHeight < 0)
                {
                    return; // not visible
                }
            }
        }

        GL.Begin(GL.QUADS);

        if (position.width < 1 && position.width > 0) position.width = 1;
        if (position.height < 1 && position.height > 0) position.height = 1;

        GL.Color(color);
        GL.Vertex3(position.x, (position.y + position.height), 0);
        GL.TexCoord(new Vector3(0, uvHeight, 0));

        GL.Color(color);
        GL.Vertex3(position.x, position.y, 0);
        GL.TexCoord(new Vector3(1, uvHeight, 0));

        GL.Color(color);
        GL.Vertex3((position.x + position.width), position.y, 0);
        GL.TexCoord(new Vector3(1, 0, 0));

        GL.Color(color);
        GL.Vertex3((position.x + position.width), (position.y + position.height), 0);
        GL.TexCoord(new Vector3(0, 0, 0));

        GL.End();
    }

    private enum CircleQuadrantId
    {
        TopRight = 0,
        TopLeft,
        BottomLeft,
        BottomRight,
        All,
    }

    private void DrawCircleGL(Vector3 center, float size, CircleQuadrantId quadrant, Color color, float zoomScale, bool clip)
    {
        Matrix4x4 scaleMatrix = Matrix4x4.Scale(new Vector3(size, size, size));

        Vector3 scrolling = new Vector3(UIEditorVariables.SceneScrolling.x, UIEditorVariables.SceneScrolling.y, 0);

        int numVerts = 41;

        Vector3[] verts = new Vector3[numVerts];
        Vector2[] uvs = new Vector2[numVerts];
        int[] tris = new int[(numVerts * 3)];

        verts[0] = Vector3.zero + center;
        uvs[0] = new Vector2(0.5f, 0.5f);

        float angle = 90.0f / (float)(numVerts - 2);

        float startAngle = 90;

        if (quadrant == CircleQuadrantId.TopRight)
            startAngle = 180;
        else if (quadrant == CircleQuadrantId.BottomLeft)
            startAngle = 270;
        else if (quadrant == CircleQuadrantId.BottomRight)
            startAngle = 0;
        else if (quadrant == CircleQuadrantId.All)
        {
            angle = 360.0f / (float)(numVerts - 2);
            startAngle = 0;
        }

        for (int i = 1; i < numVerts; ++i)
        {
            verts[i] = Quaternion.AngleAxis(startAngle + (angle * (float)(i - 1)), Vector3.back) * Vector3.up;

            float normedHorizontal = (verts[i].x + 1.0f) * 0.5f;
            float normedVertical = (verts[i].y + 1.0f) * 0.5f;
            uvs[i] = new Vector2(normedHorizontal, normedVertical);

            verts[i] = scaleMatrix.MultiplyPoint(verts[i]);
            verts[i] += center;

        }

        for (int i = 0; i < verts.Length; ++i)
            verts[i] += scrolling;

        for (int i = 0; i + 2 < numVerts; ++i)
        {
            int index = i * 3;
            tris[index + 0] = 0;
            tris[index + 1] = i + 1;
            tris[index + 2] = i + 2;
        }


        float deviceWidth = UIEditorVariables.DeviceWidth;
        float deviceHeight = UIEditorVariables.DeviceHeight;

        Vector2 offset = new Vector2(deviceWidth - (deviceWidth * zoomScale), deviceHeight - (deviceHeight * zoomScale));
        offset /= 2;
        for (int i = 0; i < verts.Length; ++i)
        {

            verts[i] *= zoomScale;
            verts[i].x += offset.x;
            verts[i].y += offset.y;
        }

        if (clip)
        {
            for (int i = 0; i < verts.Length; ++i)
            {
                if (verts[i].y < 0)
                {
                    verts[i].y = 0;
                }
            }
        }

        GL.Begin(GL.TRIANGLES);

        for (int i = 0; i < tris.Length; i += 3)
        {
            int triIndex0 = tris[(i) + 0];
            int triIndex1 = tris[(i) + 1];
            int triIndex2 = tris[(i) + 2];

            GL.Color(color);
            GL.Vertex(verts[triIndex0]);
            //GL.TexCoord(uvs[triIndex0]);

            GL.Color(color);
            GL.Vertex(verts[triIndex1]);
           // GL.TexCoord(uvs[triIndex1]);

            GL.Color(color);
            GL.Vertex(verts[triIndex2]);
           // GL.TexCoord(uvs[triIndex2]);
        }

        GL.End();
    }

    private void DrawFatCircle(Vector3 center, float innerSize, float outSize, Color color)
    {
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        Matrix4x4 scaleMatrixInnerCircle = Matrix4x4.Scale(new Vector3(innerSize, innerSize, innerSize));
        Matrix4x4 scaleMatrixOutsideCircle = Matrix4x4.Scale(new Vector3(outSize, outSize, outSize));

        int numVerts = 41;
        float angle = 360.0f / (float)(numVerts - 2);

        for (int i = 0; i < numVerts; ++i)
        {
            Vector3 vInside = Quaternion.AngleAxis((angle * (float)(i - 1)), Vector3.back) * Vector3.up;
            vInside = scaleMatrixInnerCircle.MultiplyPoint(vInside);
            vInside += center;
            verts.Add(vInside);
        }

        for (int i = 0; i < numVerts; ++i)
        {
            Vector3 vOutside = Quaternion.AngleAxis((angle * (float)(i - 1)), Vector3.back) * Vector3.up;
            vOutside = scaleMatrixOutsideCircle.MultiplyPoint(vOutside);
            vOutside += center;
            verts.Add(vOutside);
        }

        for (int i = 0; i < numVerts - 2; ++i)
        {
            tris.Add(i);
            tris.Add(i + numVerts);
            tris.Add(i + 1 + numVerts);

            tris.Add(i);
            tris.Add(i + 1);
            tris.Add(i + 1 + numVerts);
        }

        GL.Begin(GL.TRIANGLES);

        for (int i = 0; i < tris.Count; i += 3)
        {
            int triIndex0 = tris[(i) + 0];
            int triIndex1 = tris[(i) + 1];
            int triIndex2 = tris[(i) + 2];

            GL.Color(color);
            GL.Vertex(verts[triIndex0]);
           // GL.TexCoord(Vector3.zero);

            GL.Color(color);
            GL.Vertex(verts[triIndex1]);
         //   GL.TexCoord(Vector3.zero);

            GL.Color(color);
            GL.Vertex(verts[triIndex2]);
          //  GL.TexCoord(Vector3.zero);
        }

        GL.End();
    }

    private void SetDefaultMaterial()
    {
        EditorMaterial.mainTexture = Texture2D.whiteTexture;
        EditorMaterial.color = Color.white;
        EditorMaterial.SetPass(0);
    }

    public void DrawEditorGrid(Rect windowRect)
    {
       // float squareSize = 20;
        float squaresonX = (windowRect.width / (float)ChekerGridSize) + 2;
        float squaresonY =  (windowRect.height / (float)ChekerGridSize) + 1;

        CheckerGridMaterial.color = Color.white;
        CheckerGridMaterial.mainTexture = CheckerGridTexture;
        CheckerGridMaterial.mainTextureScale = new Vector2(squaresonX, squaresonY);
        CheckerGridMaterial.SetPass(0);

        //Color color1 = new Color(45 / 255.0f, 45 / 255.0f, 45 / 255.0f, 1.0f);
        //Color color2 = new Color(47 / 255.0f, 47 / 255.0f, 47 / 255.0f, 1.0f);


        DrawRectangle(new Rect(0, 0, windowRect.width, windowRect.height), Color.white, Vector2.zero);
    }


    public void DrawControlThatHasNoVisuals(GameObject selected, float zoomScale, float editorWindowWdith, float editorWindowHeight, UIEditorWindow window)
    {
        UIEditorWindow.ObjectData objectData = window.GetObjectData(selected);
        if (objectData == null) return;

        Rect boundingRect = objectData.Rect;

        SetDefaultMaterial();

        float translatedX = boundingRect.x;
        float translatedY = boundingRect.y;

        Color color = UIEditorHelpers.Color(50, 75, 170, 128);

        Rect topRect, bottomRect, leftRect, rightRect;

        UIEditorHelpers.CreateBoundingGrabRects(out topRect, out bottomRect, out leftRect, out rightRect, 2, new Rect(translatedX, translatedY, boundingRect.width, boundingRect.height));

        // Square
        DrawRectangle(topRect, color, UIEditorVariables.SceneScrolling, zoomScale, true);
        DrawRectangle(bottomRect, color, UIEditorVariables.SceneScrolling, zoomScale, true);
        DrawRectangle(leftRect, color, UIEditorVariables.SceneScrolling, zoomScale, true);
        DrawRectangle(rightRect, color, UIEditorVariables.SceneScrolling, zoomScale, true);
    }

    public void DrawHighlightedObjects(UIEditorWindow.ObjectData newParent, float zoomScale, UIEditorWindow window)
    {
        SetDefaultMaterial();

        Rect boundingRect = newParent.Rect;// UIEditorHelpers.GetTransformedRectInScreenCoordinates(selected, selected.GetComponentInParent<Canvas>());

        //  float translatedX = boundingRect.x;
        // float translatedY = boundingRect.y;

        // Color color = UIEditorHelpers.Color(50, 75, 170, 245);
        Color color = UIEditorHelpers.Color(100, 149, 245);

        //UIEditorData22.Singleton.SelectedControlTopDragRect = new Rect(translatedX - 3, translatedY - 3, boundingRect.width + 6, 3);
        //UIEditorData22.Singleton.SelectedControlBottomDragRect = new Rect(translatedX - 3, translatedY + boundingRect.height, boundingRect.width + 6, 3);
        //UIEditorData22.Singleton.SelectedControlLeftDragRect = new Rect(translatedX - 3, translatedY - 3, 3, boundingRect.height + 6);
        //UIEditorData22.Singleton.SelectedControlRightDragRect = new Rect(translatedX + boundingRect.width, translatedY - 3, 3, boundingRect.height + 6);

        //Rect topRect;// = new Rect(translatedX - 3, translatedY - 3, boundingRect.width + 6, 3);
        //Rect bottomRect;// = new Rect(translatedX - 3, translatedY + boundingRect.height, boundingRect.width + 6, 3);
        //Rect leftRect;// = new Rect(translatedX - 3, translatedY - 3, 3, boundingRect.height + 6);
        //Rect rightRect;// = new Rect(translatedX + boundingRect.width, translatedY - 3, 3, boundingRect.height + 6);

        //UIEditorHelpers.CreateBoundingGrabRects(out topRect, out bottomRect, out leftRect, out rightRect, 1.5f, new Rect(translatedX, translatedY, boundingRect.width, boundingRect.height));

        //// Square
        //DrawRectangle(topRect, color, UIEditorVariables.SceneScrolling, zoomScale, true);
        //DrawRectangle(bottomRect, color, UIEditorVariables.SceneScrolling, zoomScale, true);
        //DrawRectangle(leftRect, color, UIEditorVariables.SceneScrolling, zoomScale, true);
        //DrawRectangle(rightRect, color, UIEditorVariables.SceneScrolling, zoomScale, true);


        Rect newRect = UIEditorHelpers.TransformRectToScreenPixels(boundingRect, zoomScale, window.ZoomScalePositionOffset);

        float size = 1.3f;

        // Square
        DrawRectangleUnscaled(new Rect(newRect.x - size, newRect.y - size, newRect.width + size * 2, size), color, UIEditorVariables.SceneScrolling, zoomScale, true);
        DrawRectangleUnscaled(new Rect(newRect.x - size, newRect.y + newRect.height, newRect.width + size * 2, size), color, UIEditorVariables.SceneScrolling, zoomScale, true);

        DrawRectangleUnscaled(new Rect(newRect.x - size, newRect.y, size, newRect.height), color, UIEditorVariables.SceneScrolling, zoomScale, true);
        DrawRectangleUnscaled(new Rect(newRect.x + newRect.width, newRect.y, size, newRect.height), color, UIEditorVariables.SceneScrolling, zoomScale, true);

        //Color savedColor = GUI.color;
        // GUI.color = color;
        if (Event.current.control) EditorGUI.LabelField(new Rect(newRect.x - 3, newRect.y - 16, 100, 200), newParent.GameObject.name);
        //GUI.color = savedColor;
    }

    public void DrawChangeParentHelpers(UIEditorWindow.ObjectData newParent, float zoomScale)
    {
        SetDefaultMaterial();

        Rect boundingRect = newParent.Rect;// UIEditorHelpers.GetTransformedRectInScreenCoordinates(selected, selected.GetComponentInParent<Canvas>());

        float translatedX = boundingRect.x;
        float translatedY = boundingRect.y;

        Color color = UIEditorHelpers.Color(100, 149, 245);

        Rect topRect;
        Rect bottomRect;
        Rect leftRect;
        Rect rightRect;

        UIEditorHelpers.CreateBoundingGrabRects(out topRect, out bottomRect, out leftRect, out rightRect, 1, 
            new Rect(translatedX, translatedY, boundingRect.width, boundingRect.height));

        // Square
        DrawRectangle(topRect, color, UIEditorVariables.SceneScrolling, zoomScale, true);
        DrawRectangle(bottomRect, color, UIEditorVariables.SceneScrolling, zoomScale, true);
        DrawRectangle(leftRect, color, UIEditorVariables.SceneScrolling, zoomScale, true);
        DrawRectangle(rightRect, color, UIEditorVariables.SceneScrolling, zoomScale, true);
    }

    public void DrawScaleGizmo(float zoomScale, UIEditorWindow window)
    {
        if (Selection.activeGameObject == null) return;
        SetDefaultMaterial();

        GameObject selected = Selection.activeGameObject;
        UIEditorWindow.ObjectData objectData = window.GetObjectData(selected);
        if (objectData == null) return;

        float lineLength = 50;

        //Vector2 middle = new Vector2(boundingRect.xMin + boundingRect.width * rectTransform.pivot.x, boundingRect.yMin + boundingRect.height * (1 - rectTransform.pivot.y));
        //middle += UIEditorVariables.SceneScrolling;
        //middle *= zoomScale;

        //Vector2 offset = new Vector2(UIEditorVariables.DeviceWidth - (UIEditorVariables.DeviceWidth * zoomScale), UIEditorVariables.DeviceHeight - (UIEditorVariables.DeviceHeight * zoomScale));
        //middle.x += offset.x / 2;
        //middle.y += offset.y / 2;
        Vector2 middle = GetUnscalePivot(objectData, zoomScale);

        float lineFat = 2;
        float lineFatHalf = lineFat / 2;
        float boxFat = 12;
        float boxFatHalf = boxFat / 2;

        Rect upwardsRect = new Rect(middle.x - lineFatHalf, middle.y, lineFat, -lineLength);
        Rect rightwardsRect = new Rect(middle.x, middle.y - lineFatHalf, lineLength, lineFat);

        Rect upwardsRectBox = new Rect(middle.x - boxFatHalf, middle.y - lineLength - boxFatHalf, boxFat, boxFat);
        Rect rightwardsRectBox = new Rect(middle.x + lineLength - boxFatHalf, middle.y - boxFatHalf, boxFat, boxFat);

        Rect middleBox = new Rect(middle.x - MiddleBoxSizeHalf, middle.y - MiddleBoxSizeHalf, MiddleBoxSize, MiddleBoxSize);

        Color upColor = Color.green;
        Color rightColor = Color.red;
        Color middleColor = new Color(0.9f, 0.9f, 0.9f, 1.0f);

        if (window.Input.IsScalingHorizontal) rightColor = Color.yellow;
        if (window.Input.IsScalingVertical) upColor = Color.yellow;

        // Square
        DrawRectangleUnscaled(upwardsRect, upColor, UIEditorVariables.SceneScrolling, zoomScale, true);
        DrawRectangleUnscaled(rightwardsRect, rightColor, UIEditorVariables.SceneScrolling, zoomScale, true);
        DrawRectangleUnscaled(upwardsRectBox, upColor, UIEditorVariables.SceneScrolling, zoomScale, true);
        DrawRectangleUnscaled(rightwardsRectBox, rightColor, UIEditorVariables.SceneScrolling, zoomScale, true);

        DrawRectangleUnscaled(middleBox, middleColor, UIEditorVariables.SceneScrolling, zoomScale, true);

        // add some padding on hitboxes
        window.Input.ScaleLeftRightHotRect = new Rect(rightwardsRectBox.x - 2, rightwardsRectBox.y - 2, rightwardsRectBox.width + 4, rightwardsRectBox.height + 4);
        window.Input.ScaleTopBottomHotRect = new Rect(upwardsRectBox.x - 2, upwardsRectBox.y - 2, upwardsRectBox.width + 4, upwardsRectBox.height + 4);
        window.Input.ScaleMiddleHotRect = new Rect(middleBox.x - 2, middleBox.y - 2, middleBox.width + 4, middleBox.height + 4); ;

    }

    public void DrawCenterGizmo(UIEditorWindow.ObjectData objectData, float zoomScale, UIEditorWindow window)
    {
        SetDefaultMaterial();

        Vector2 middle = GetUnscalePivot(objectData, zoomScale);

        Rect middleBox = new Rect(middle.x - MiddleBoxSizeHalf, middle.y - MiddleBoxSizeHalf, MiddleBoxSize, MiddleBoxSize);
        Color middleColor = new Color(0.9f, 0.9f, 0.9f, 1.0f);

        DrawRectangleUnscaled(middleBox, middleColor, UIEditorVariables.SceneScrolling, zoomScale, true);
    }


    public void DrawMoveGizmo(float zoomScale, UIEditorWindow window)
    {
        if (Selection.activeGameObject == null) return;
        SetDefaultMaterial();

        GameObject selected = Selection.activeGameObject;
        UIEditorWindow.ObjectData objectData = window.GetObjectData(selected);
        if (objectData == null) return;

        float lineLength = 50;
        Vector2 middle = GetUnscalePivot(objectData, zoomScale);

        float lineFat = 2;
        float lineFatHalf = lineFat / 2;
        float triangleSize = 8;
        float trianglePointScale = 2.1f;

        Rect upwardsRect = new Rect(middle.x - lineFatHalf, middle.y, lineFat, -lineLength);
        Rect rightwardsRect = new Rect(middle.x, middle.y - lineFatHalf, lineLength, lineFat);

        Rect middleBox = new Rect(middle.x - MiddleBoxSizeHalf, middle.y - MiddleBoxSizeHalf, MiddleBoxSize, MiddleBoxSize);


        Color upColor = Color.green;
        Color rightColor = Color.red;
        Color middleColor = new Color(0.9f, 0.9f, 0.9f, 1.0f);

        if (window.Input.IsMovingHorizontal) rightColor = Color.yellow;
        if (window.Input.IsMovingVertical) upColor = Color.yellow;

        // Square
        DrawRectangleUnscaled(upwardsRect, upColor, UIEditorVariables.SceneScrolling, zoomScale, true);
        DrawRectangleUnscaled(rightwardsRect, rightColor, UIEditorVariables.SceneScrolling, zoomScale, true);

        DrawRectangleUnscaled(middleBox, middleColor, UIEditorVariables.SceneScrolling, zoomScale, true);



        DrawTriangleUnscaled(
            new Vector2(middle.x - triangleSize, middle.y - lineLength),
            new Vector2(middle.x, middle.y - lineLength - triangleSize * trianglePointScale),
            new Vector2(middle.x + triangleSize, middle.y - lineLength),
            upColor,
            UIEditorVariables.SceneScrolling,
            zoomScale,
            true);


        DrawTriangleUnscaled(
             new Vector2(middle.x + lineLength, middle.y + triangleSize),
             new Vector2(middle.x + lineLength + triangleSize * trianglePointScale, middle.y),
             new Vector2(middle.x + lineLength, middle.y - triangleSize),
             rightColor,
             UIEditorVariables.SceneScrolling,
             zoomScale,
             true);

        // add some padding on hitboxes
        window.Input.MoveUpOnlyHotRect = new Rect(
            middle.x - triangleSize * 1.25f,
            middle.y - lineLength - triangleSize * trianglePointScale * 1.25f,
            triangleSize * 2 * 1.25f,
            lineLength + triangleSize * trianglePointScale * 1.25f + triangleSize * 1.25f);
       
        window.Input.MoveRightOnlyHotRect = new Rect(
            middle.x - triangleSize * 1.25f,
            middle.y - triangleSize * 1.25f,
            lineLength + triangleSize * 1.25f * trianglePointScale + triangleSize * 1.25f,
            triangleSize * 2 * 1.25f);

    }


    public void DrawRotateGizmo(float zoomScale, UIEditorWindow window)
    {
        if (Selection.activeGameObject == null) return;
        SetDefaultMaterial();

        GameObject selected = Selection.activeGameObject;
        UIEditorWindow.ObjectData objectData = window.GetObjectData(selected);
        if (objectData == null) return;

        Color middleColor = new Color(0.9f, 0.9f, 0.9f, 1.0f);
        Vector2 center = GetUnscalePivot(objectData, zoomScale);

        DrawRectangleUnscaled(new Rect(center.x - MiddleBoxSizeHalf, center.y - MiddleBoxSizeHalf, MiddleBoxSize, MiddleBoxSize), middleColor, UIEditorVariables.SceneScrolling, zoomScale, true);

        Color circleColor = new Color(0.15f, 0.15f, 1, 1);
        if (window.Input.IsRotating) circleColor = Color.yellow;

        DrawFatCircle(center, 60, 68, circleColor);

        window.Input.RotateCenterSizeHotRect = center;
        window.Input.RotateMaxSizeHotRect = 68 + 8;
        window.Input.RotateMinSizeHotRect = 60 - 8;

    }

    private Vector2 GetUnscalePivot(UIEditorWindow.ObjectData objectData, float zoomScale)
    {
        RectTransform rectTransform = objectData.GameObject.GetComponent<RectTransform>();
        //Rect boundingRect = objectData.Rect;

        Vector2[] localCorners = new Vector2[] 
        {
            new Vector2(objectData.Corners[0].x, objectData.Corners[0].y), 
            new Vector2(objectData.Corners[1].x, objectData.Corners[1].y), 
            new Vector2(objectData.Corners[2].x, objectData.Corners[2].y), 
            new Vector2(objectData.Corners[3].x, objectData.Corners[3].y), 
        };

        for (int i = 0; i < localCorners.Length; ++i)
        {
            Vector2 spot = localCorners[i];
            spot.x += UIEditorVariables.SceneScrolling.x;
            spot.y += UIEditorVariables.SceneScrolling.y;
            spot *= zoomScale;
            Vector2 offset = new Vector2(UIEditorVariables.DeviceWidth - (UIEditorVariables.DeviceWidth * zoomScale), UIEditorVariables.DeviceHeight - (UIEditorVariables.DeviceHeight * zoomScale));
            spot.x += offset.x / 2;
            spot.y += offset.y / 2;

            localCorners[i] = new Vector3(spot.x, spot.y, 0);
        }

        Vector2 directionFromTopLeft_TopRight = (localCorners[2] - localCorners[1]).normalized;
        Vector2 directionFromTopLeft_BottomLeft = (localCorners[1] - localCorners[0]).normalized;

        //Vector2 topLeft = new Vector2(localCorners[1].x, localCorners[1].y);
        Vector2 boxSize = new Vector2(Vector2.Distance(localCorners[1], localCorners[2]), Vector2.Distance(localCorners[1], localCorners[0]));


        Vector2 center = new Vector2(localCorners[1].x, localCorners[1].y);
        Vector2 offsetX = (directionFromTopLeft_TopRight * boxSize.x * rectTransform.pivot.x);
        Vector2 offsetY = (directionFromTopLeft_BottomLeft * boxSize.y * (1 - rectTransform.pivot.y));

        center += offsetX;
        center -= offsetY;

        return center;
    }

    public void DrawSelectRegion(float zoomScale, UIEditorWindow window)
    {
        SetDefaultMaterial();

        if (window.Input.InputState == InputStateId.SelectRegion)
        {
            //Color color = UIEditorHelpers.Color(100, 149, 237, 16);
            Color color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            DrawRectangleUnscaled(window.Input.SelectionRegionRect, color, UIEditorVariables.SceneScrolling, zoomScale, true);
        }
    }

    private void DrawAnchors(float zoomScale, UIEditorWindow window, RectTransform rectTransform, UIEditorWindow.ObjectData myObjectData)
    {
        Rect parentRect = new Rect(0, 0, UIEditorVariables.DeviceWidth, UIEditorVariables.DeviceHeight);

        if (rectTransform.parent != null)
        {
            RectTransform parentTransform = rectTransform.parent.GetComponent<RectTransform>();
            if (parentTransform != null)
            {
                UIEditorWindow.ObjectData parentObjectData = window.GetObjectData(parentTransform.gameObject);
                if (parentObjectData != null)
                {
                    parentRect = parentObjectData.Rect;
                }
            }
        }

        Color color = UIEditorHelpers.Color(100, 149, 245);


        float pivotX = myObjectData.Rect.x + myObjectData.Rect.width * rectTransform.pivot.x;
        if (rectTransform.sizeDelta.x < 0)
            pivotX = myObjectData.Rect.x + myObjectData.Rect.width * (1 - rectTransform.pivot.x);

        float pivotY = myObjectData.Rect.y + myObjectData.Rect.height * (1 - rectTransform.pivot.y);
        if (rectTransform.sizeDelta.y < 0)
            pivotY = myObjectData.Rect.y + myObjectData.Rect.height * (rectTransform.pivot.y);

        Vector2 anchorMinLocation = new Vector2(rectTransform.anchorMin.x * parentRect.width, (1 - rectTransform.anchorMin.y) * parentRect.height);
        Vector2 anchorMaxLocation = new Vector2(rectTransform.anchorMax.x * parentRect.width, (1 - rectTransform.anchorMax.y) * parentRect.height);

        float size = 1.2f;
        float sizeHalf = size / 2;

        float horizontalDistance = parentRect.width;
        float verticalDistance = parentRect.height;

        if (window.Input.InputState == InputStateId.DraggingSelectedControls || window.Input.InputState == InputStateId.ResizingControl)
        {

            // Stretched Horizontal
            if (rectTransform.anchorMin.x != rectTransform.anchorMax.x)
            {
                // Stretched
                float verticalMidPoint = myObjectData.Rect.y + myObjectData.Rect.height / 2;

                float startX = parentRect.x + rectTransform.anchorMin.x * parentRect.width;
                float widthX = myObjectData.Rect.x - startX;

                float endX = parentRect.x + rectTransform.anchorMax.x * parentRect.width;
                float endWidth = myObjectData.Rect.xMax;


                DrawDotRectangle(true, new Rect(startX, verticalMidPoint - sizeHalf, widthX, size), color, UIEditorVariables.SceneScrolling, zoomScale, true);
                DrawDotRectangle(true, new Rect(endX, verticalMidPoint - sizeHalf, endWidth - endX, size), color, UIEditorVariables.SceneScrolling, zoomScale, true);
            }

            if (rectTransform.anchorMin.y != rectTransform.anchorMax.y)
            {
                float startY = parentRect.y + rectTransform.anchorMin.y * parentRect.height;
                float heightY = myObjectData.Rect.y - startY;

                float endY = parentRect.y + rectTransform.anchorMax.y * parentRect.height;
                float endHeight = myObjectData.Rect.yMax;

                float horizontalMidPoint = myObjectData.Rect.x + myObjectData.Rect.width / 2;

                DrawDotRectangle(false, new Rect(horizontalMidPoint, startY, size, heightY), color, UIEditorVariables.SceneScrolling, zoomScale, true);
                DrawDotRectangle(false, new Rect(horizontalMidPoint - sizeHalf, endY, size, (endHeight - endY)), color, UIEditorVariables.SceneScrolling, zoomScale, true);
            }


            if (rectTransform.anchorMin.x == rectTransform.anchorMax.x)
            {
                Rect positionSize = new Rect(
                    parentRect.x + anchorMinLocation.x,
                    pivotY - sizeHalf,
                    pivotX - anchorMinLocation.x - parentRect.x,
                     size);

                horizontalDistance = Mathf.Abs(positionSize.width);

                DrawDotRectangle(true, positionSize, color, UIEditorVariables.SceneScrolling, zoomScale, true);

                DrawDotRectangle(false, new Rect(
                  (pivotX - sizeHalf) - (pivotX - anchorMinLocation.x - parentRect.x),
                  anchorMinLocation.y + parentRect.y,
                  size,
                  pivotY - anchorMinLocation.y - parentRect.y
                   ),
                  color, UIEditorVariables.SceneScrolling, zoomScale, true);
            }

            if (rectTransform.anchorMin.y == rectTransform.anchorMax.y)
            {
                Rect positionSize = new Rect(
                  pivotX - sizeHalf,
                  anchorMinLocation.y + parentRect.y,
                  size,
                  pivotY - anchorMinLocation.y - parentRect.y
                   );

                verticalDistance = Mathf.Abs(positionSize.height);

                DrawDotRectangle(false, positionSize,
                    color, UIEditorVariables.SceneScrolling, zoomScale, true);

                DrawDotRectangle(true, new Rect(
                    parentRect.x + anchorMinLocation.x,
                     anchorMinLocation.y + parentRect.y,
                    pivotX - anchorMinLocation.x - parentRect.x,
                     size),
                    color, UIEditorVariables.SceneScrolling, zoomScale, true);
            }
        }


        DrawResizeHelpers(zoomScale, window.ZoomScalePositionOffset, myObjectData);
        DrawAnchorBox(rectTransform, zoomScale, window.ZoomScalePositionOffset, parentRect, anchorMinLocation, anchorMaxLocation, horizontalDistance, verticalDistance, window);
    }

    private void DrawDotRectangle(bool isXAxis, Rect position, Color color, Vector2 scrolling, float zoomScale = 1.0f, bool clip = false)
    {
        DrawDotRectangle(false, isXAxis, position, color, scrolling, zoomScale, clip);

    }

    private void DrawDotRectangle(bool useUnscale, bool isXAxis, Rect position, Color color, Vector2 scrolling, float zoomScale = 1.0f, bool clip = false)
    {
        float lineGap = 10;

        if (isXAxis)
        {
            float steps = Mathf.Abs(position.width) / lineGap;

            float min = Mathf.Min(position.xMin, position.xMax);

            for (int i = 0; i < (int)steps; ++i)
            {
                Rect newPosition = position;
                newPosition.x = min + i * lineGap;
                newPosition.width = lineGap / 2;
                if (useUnscale)
                    DrawRectangleUnscaled(newPosition, color, scrolling, zoomScale, clip);
                else
                    DrawRectangle(newPosition, color, scrolling, zoomScale, clip);

            }
        }
        else
        {
            float steps = Mathf.Abs(position.height) / lineGap;

            float minY = Mathf.Min(position.yMin, position.yMax);

            for (int i = 0; i < (int)steps; ++i)
            {
                Rect newPosition = position;
                newPosition.y = minY + i * lineGap;
                newPosition.height = lineGap / 2;

                //if (i == (int)steps - 1)
                //{
                //    newPosition.height = position.height - newPosition.y;
                //}

                if (useUnscale)
                    DrawRectangleUnscaled(newPosition, color, scrolling, zoomScale, clip);

                else
                    DrawRectangle(newPosition, color, scrolling, zoomScale, clip);
            }
        }

    }

    private void DrawResizeHelpers(float zoomScale, Vector2 zoomScaleOffset, UIEditorWindow.ObjectData myObjectData)
    {
        Color color = UIEditorHelpers.Color(100, 149, 245);

        List<Rect> corners = UIEditorHelpers.Get8ScaleBoxesAroundObject(myObjectData, zoomScale, zoomScaleOffset, false);
        for (int b = 0; b < corners.Count; b++)
        {
           // corners[b] = UIEditorHelpers.TransformRectToScreenPixels(corners[b], UIEditorHelpers.GetZoomScaleFactor(), zoomScaleOffset);

            DrawRectangleUnscaled(corners[b], color, UIEditorVariables.SceneScrolling, zoomScale, true);
        }
    }

    private void DrawAnchorBox(RectTransform objectDrawing, float zoomScale, Vector2 zoomScaleOffset, Rect parentRect, Vector2 anchorMinLocation, Vector2 anchorMaxLocation, float horizontalDistance, float verticalDistance, UIEditorWindow window)
    {
        Color color = UIEditorHelpers.Color(100, 149, 245);

        Vector2 anchorPosition = new Vector2(parentRect.x + anchorMinLocation.x, anchorMinLocation.y + parentRect.y);
        anchorPosition = UIEditorHelpers.TransformCoordToScreenPixels(anchorPosition, zoomScale, zoomScaleOffset);


        Vector2 anchorPositionTopLeft = new Vector2(parentRect.x + anchorMinLocation.x, anchorMaxLocation.y + parentRect.y);
        anchorPositionTopLeft = UIEditorHelpers.TransformCoordToScreenPixels(anchorPositionTopLeft, zoomScale, zoomScaleOffset);

        Vector2 anchorPositionTopRight = new Vector2(parentRect.x + anchorMaxLocation.x, anchorMaxLocation.y + parentRect.y);
        anchorPositionTopRight = UIEditorHelpers.TransformCoordToScreenPixels(anchorPositionTopRight, zoomScale, zoomScaleOffset);

        Vector2 anchorPositionBottomLeft = new Vector2(parentRect.x + anchorMinLocation.x, anchorMinLocation.y + parentRect.y);
        anchorPositionBottomLeft = UIEditorHelpers.TransformCoordToScreenPixels(anchorPositionBottomLeft, zoomScale, zoomScaleOffset);

        Vector2 anchorPositionBottomRight = new Vector2(parentRect.x + anchorMaxLocation.x, anchorMinLocation.y + parentRect.y);
        anchorPositionBottomRight = UIEditorHelpers.TransformCoordToScreenPixels(anchorPositionBottomRight, zoomScale, zoomScaleOffset);


        float smallSquareSize = 7;
        float squaresGap = 1.5f;

        DrawRectangleUnscaled(new Rect(anchorPositionTopLeft.x - smallSquareSize - squaresGap, anchorPositionTopLeft.y - smallSquareSize - squaresGap, smallSquareSize, smallSquareSize),
              color, UIEditorVariables.SceneScrolling, zoomScale, true);


        DrawRectangleUnscaled(new Rect(anchorPositionTopRight.x + squaresGap, anchorPositionTopRight.y - smallSquareSize - squaresGap, smallSquareSize, smallSquareSize),
            color, UIEditorVariables.SceneScrolling, zoomScale, true);

        DrawRectangleUnscaled(new Rect(anchorPositionBottomLeft.x - smallSquareSize - squaresGap, anchorPositionBottomLeft.y + squaresGap, smallSquareSize, smallSquareSize),
           color, UIEditorVariables.SceneScrolling, zoomScale, true);


        DrawRectangleUnscaled(new Rect(anchorPositionBottomRight.x + squaresGap, anchorPositionBottomRight.y + squaresGap, smallSquareSize, smallSquareSize),
            color, UIEditorVariables.SceneScrolling, zoomScale, true);

        if (window.Input.InputState == InputStateId.DraggingSelectedControls)
        {

            // DOT BOX LINES

            // DRAW ANCHOR DOTED BOX
            if (objectDrawing.anchorMin.y != objectDrawing.anchorMax.y && objectDrawing.anchorMin.x != objectDrawing.anchorMax.x)
            {
                DrawDotRectangle(true,
                    false,
                    new Rect(anchorPositionTopLeft.x, anchorPositionTopLeft.y, 1, anchorPositionBottomRight.y - anchorPositionTopLeft.y),
                    color, UIEditorVariables.SceneScrolling, zoomScale, true);

                DrawDotRectangle(true,
                   false,
                   new Rect(anchorPositionBottomRight.x, anchorPositionTopLeft.y, 1, anchorPositionBottomRight.y - anchorPositionTopLeft.y),
                   color, UIEditorVariables.SceneScrolling, zoomScale, true);


                DrawDotRectangle(true,
                  true,
                  new Rect(anchorPositionTopLeft.x, anchorPositionTopLeft.y, anchorPositionBottomRight.x - anchorPositionTopLeft.x, 1),
                  color, UIEditorVariables.SceneScrolling, zoomScale, true);

                DrawDotRectangle(true,
                   true,
                   new Rect(anchorPositionTopLeft.x, anchorPositionBottomRight.y, anchorPositionBottomRight.x - anchorPositionTopLeft.x, 1),
                   color, UIEditorVariables.SceneScrolling, zoomScale, true);
            }
        }

        //Vector2 anchorPointScreen = TransformPoint(anchorPosition, UIEditorVariables.SceneScrolling, zoomScale);


        //EditorGUI.LabelField(new Rect(anchorPointScreen.x + (horizontalDistance / 2) - 30, anchorPointScreen.y, 100, 200), Mathf.Abs(horizontalDistance).ToString());

        //Rect drawLabelY = new Rect(anchorPointScreen.x + 14, anchorPointScreen.y + (verticalDistance / 2) - 25, 100, 200);
        //GUIUtility.RotateAroundPivot(90, new Vector2(drawLabelY.x, drawLabelY.y));
        //EditorGUI.LabelField(drawLabelY, Mathf.Abs(verticalDistance).ToString());
        //GUIUtility.RotateAroundPivot(-90, new Vector2(drawLabelY.x, drawLabelY.y));

    }


    public void DrawSelectedEditorHelpers(float zoomScale, UIEditorWindow window)
    {
        SetDefaultMaterial();

        for (int i = 0; i < UIEditorSelectionHelpers.Selected.Count; ++i)
        {
            GameObject selected = UIEditorSelectionHelpers.Selected[i];
            if (!selected.gameObject.activeInHierarchy) continue;
            UIEditorWindow.ObjectData objectData = window.GetObjectData(selected);
            if (objectData == null) continue;

            Rect boundingRect = objectData.Rect;
            RectTransform rectTransform = selected.GetComponent<RectTransform>();
            float translatedX = boundingRect.x;// +UIEditorData22.Singleton.SceneScrolling.x;
            float translatedY = boundingRect.y;// +UIEditorData22.Singleton.SceneScrolling.y;

            Color color = UIEditorHelpers.Color(100, 149, 245);
            
            // DRAW GUIDE LINES ONLY IF WE HAVE 1 SELECTED OBJECT

            if (UIEditorSelectionHelpers.Selected.Count == 1/* && Tools.current == Tool.Move*/)
            {
                DrawAnchors(zoomScale, window, rectTransform, objectData);
            }


            Rect topRect;// = new Rect(translatedX - 3, translatedY - 3, boundingRect.width + 6, 3);
            Rect bottomRect;// = new Rect(translatedX - 3, translatedY + boundingRect.height, boundingRect.width + 6, 3);
            Rect leftRect;// = new Rect(translatedX - 3, translatedY - 3, 3, boundingRect.height + 6);
            Rect rightRect;// = new Rect(translatedX + boundingRect.width, translatedY - 3, 3, boundingRect.height + 6);

            UIEditorHelpers.CreateBoundingGrabRects(out topRect, out bottomRect, out leftRect, out rightRect, 1.0f, 
                new Rect(translatedX, translatedY, boundingRect.width, boundingRect.height));


            Rect newRect = UIEditorHelpers.TransformRectToScreenPixels(objectData.Rect, zoomScale, window.ZoomScalePositionOffset);

            int size = 1;

            // Square
            DrawRectangleUnscaled(new Rect(newRect.x - size, newRect.y - size, newRect.width + size, size), color, UIEditorVariables.SceneScrolling, zoomScale, true);
            DrawRectangleUnscaled(new Rect(newRect.x - size, newRect.y + newRect.height, newRect.width + size, size), color, UIEditorVariables.SceneScrolling, zoomScale, true);
            DrawRectangleUnscaled(new Rect(newRect.x - size, newRect.y, size, newRect.height), color, UIEditorVariables.SceneScrolling, zoomScale, true);
            DrawRectangleUnscaled(new Rect(newRect.x + newRect.width, newRect.y, size, newRect.height), color, UIEditorVariables.SceneScrolling, zoomScale, true);

            // DrawRectangleUnscaled(new Rect(newRect.x - 2, newRect.y,  2, newRect.height), color, UIEditorVariables.SceneScrolling, zoomScale, true);
            //   DrawRectangleUnscaled(new Rect(newRect.x + newRect.width, newRect.y, 2, newRect.height), color, UIEditorVariables.SceneScrolling, zoomScale, true);


            if (Tools.current == Tool.Rect || Tools.current == Tool.View || Tools.current == Tool.None)
            {
                DrawCenterGizmo(objectData, zoomScale, window);
            }

            if (Event.current.control) EditorGUI.LabelField(new Rect(newRect.x - 3, newRect.y - 16, 100, 200), selected.name);

        }

        if (Tools.current == Tool.Scale)
        {
            DrawScaleGizmo(zoomScale, window);
        }
        else if (Tools.current == Tool.Rotate)
        {
            DrawRotateGizmo(zoomScale, window);
        }
        else if (Tools.current == Tool.Rect)
        {
            
        }
        else if(Tools.current == Tool.Move)
        {
            DrawMoveGizmo(zoomScale, window);
        }
    }

    public void DrawVirtualWindow(float zoomScale = 1.0f)
    {
        SetDefaultMaterial();

        float borderSize = 8;
        float extraHeight = 2;

        Color color = new Color(1.0f, 1.0f, 1.0f, 0.2f);

        Vector2 topLeftCorner = new Vector2(0, 0);
        //Vector2 bottomRightCorner = new Vector2(UIEditorData22.Singleton.SceneScrolling.x + resWidth, UIEditorData22.Singleton.SceneScrolling.y + resHeight);

        float localResWidth = UIEditorVariables.DeviceWidth;
        float localResHeight = UIEditorVariables.DeviceHeight;

        DrawRectangle(new Rect(topLeftCorner.x, topLeftCorner.y - borderSize, localResWidth, borderSize), color, UIEditorVariables.SceneScrolling, zoomScale, true);
        DrawRectangle(new Rect(topLeftCorner.x, topLeftCorner.y + localResHeight, localResWidth, borderSize + extraHeight), color, UIEditorVariables.SceneScrolling, zoomScale, true);

        DrawRectangle(new Rect(topLeftCorner.x - borderSize, topLeftCorner.y, borderSize, localResHeight + extraHeight), color, UIEditorVariables.SceneScrolling, zoomScale, true);
        DrawRectangle(new Rect(topLeftCorner.x + localResWidth, topLeftCorner.y, borderSize, localResHeight + extraHeight), color, UIEditorVariables.SceneScrolling, zoomScale, true);


        DrawCircleGL(new Vector3(topLeftCorner.x, topLeftCorner.y, 0), borderSize, CircleQuadrantId.TopRight, color, zoomScale, true);
        DrawCircleGL(new Vector3(topLeftCorner.x + localResWidth, topLeftCorner.y, 0), borderSize, CircleQuadrantId.TopLeft, color, zoomScale, true);

        DrawCircleGL(new Vector3(topLeftCorner.x, topLeftCorner.y + localResHeight + extraHeight, 0), borderSize, CircleQuadrantId.BottomLeft, color, zoomScale, true);
        DrawCircleGL(new Vector3(topLeftCorner.x + localResWidth, topLeftCorner.y + localResHeight + extraHeight, 0), borderSize, CircleQuadrantId.BottomRight, color, zoomScale, true);
    }

    public void DrawGrid(UIEditorWindow window, Canvas activeCanvas)
    {
        SetDefaultMaterial();

        Color color = new Color(0.56f, 0.56f, 0.62f, 0.4f);

        float gridThickness = 1;
        float gridLinesEvery = UIEditorVariables.GridSize;
        if (activeCanvas != null) gridLinesEvery *= activeCanvas.scaleFactor;

        while (gridLinesEvery < 8)
        {
            gridLinesEvery += UIEditorVariables.GridSize;
        }

        Vector2 scrolling = UIEditorVariables.SceneScrolling;

        float deviceWidth = UIEditorVariables.DeviceWidth;
        float deviceHeight = UIEditorVariables.DeviceHeight;

        float gridLinesX = deviceWidth / gridLinesEvery;
        float gridLinesY = deviceHeight / gridLinesEvery;

        float zoomFactor = UIEditorHelpers.GetZoomScaleFactor();

        for (int i = 1; i < gridLinesY; ++i)
        {
            Rect newRect = new Rect(0, i * gridLinesEvery, deviceWidth, gridThickness);
            DrawRectangle(newRect, color, scrolling, zoomFactor, true);          
        }

        for (int i = 1; i < gridLinesX; ++i)
        {
            Rect newRect = new Rect(i * gridLinesEvery, 0, gridThickness, deviceHeight);
            DrawRectangle(newRect, color, scrolling, zoomFactor, true);
        }
    }

    public void DrawRenderTexture(RenderTexture texture, float zoomScale)
    {
        if (EditorMaterial == null) Debug.Log("EDITOR MATERIAL IS NULL");

        EditorMaterial.mainTexture = texture;
        EditorMaterial.color = Color.white;
        EditorMaterial.SetPass(0);

        DrawRectangle(
            new Rect(0,
                0,
                texture.width,
                texture.height),
                Color.white,
                UIEditorVariables.SceneScrolling,
                zoomScale,
                true);
        EditorMaterial.mainTexture = Texture2D.whiteTexture;
    }
}
