using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Paper2
    {
        private const int s_ChekerGridSize = 30;

        private float editorWindowWidth;
        private float editorWindowHeight;
        private Material _EditorMaterial;
        private Material _CheckerGridMaterial;
        private Texture2D _CheckerGridTexture;

        private enum CircleQuadrantId
        {
            TopRight = 0,
            TopLeft,
            BottomLeft,
            BottomRight,
            All,
        }

        public void Draw(float windowWidth, float windowHeight)
        {
            editorWindowWidth = windowWidth;
            editorWindowHeight = windowHeight;

            if (_EditorMaterial == null)
            {
                _EditorMaterial = new Material(Shader.Find("UIEDITOR/DefaultShader"));
                _CheckerGridMaterial = new Material(Shader.Find("UIEDITOR/DefaultShader"));
            }

            if (_CheckerGridTexture == null)
            {
                Color color1 = new Color(45 / 255.0f, 45 / 255.0f, 45 / 255.0f, 1.0f);
                Color color2 = new Color(46 / 255.0f, 46 / 255.0f, 46 / 255.0f, 1.0f);

                _CheckerGridTexture = new Texture2D(s_ChekerGridSize, s_ChekerGridSize, TextureFormat.ARGB32, false);
                for (int i = 0; i < _CheckerGridTexture.height; ++i)
                {
                    for (int j = 0; j < _CheckerGridTexture.width; ++j)
                    {
                        Color color = color1;
                        if (i > (_CheckerGridTexture.height / 2) && j > (_CheckerGridTexture.width / 2))
                        {
                            color = color2;
                        }
                        else if (i < (_CheckerGridTexture.height / 2) && j < (_CheckerGridTexture.width / 2))
                        {
                            color = color2;
                        }

                        _CheckerGridTexture.SetPixel(j, i, color);
                    }
                }

                _CheckerGridTexture.wrapMode = TextureWrapMode.Repeat;
                _CheckerGridTexture.Apply();
            }
        }

        private void SetDefaultMaterial()
        {
            _EditorMaterial.mainTexture = Texture2D.whiteTexture;
            _EditorMaterial.color = Color.white;
            _EditorMaterial.SetPass(0);
        }

        public void DrawVirtualWindow(float zoomScale = 1.0f)
        {
            SetDefaultMaterial();

            float borderSize = 8;
            float extraHeight = 2;

            Color color = new Color(1.0f, 1.0f, 1.0f, 0.2f);

            Vector2 topLeftCorner = new Vector2(0, 0);

            float localResWidth = UIEditorVariables.DeviceWidth;
            float localResHeight = UIEditorVariables.DeviceHeight;

            DrawRectangle(new Rect(topLeftCorner.x, topLeftCorner.y - borderSize, localResWidth, borderSize), color,
                UIEditorVariables.SceneScrolling, zoomScale, true);
            DrawRectangle(
                new Rect(topLeftCorner.x, topLeftCorner.y + localResHeight, localResWidth, borderSize + extraHeight),
                color, UIEditorVariables.SceneScrolling, zoomScale, true);

            DrawRectangle(
                new Rect(topLeftCorner.x - borderSize, topLeftCorner.y, borderSize, localResHeight + extraHeight),
                color, UIEditorVariables.SceneScrolling, zoomScale, true);
            DrawRectangle(
                new Rect(topLeftCorner.x + localResWidth, topLeftCorner.y, borderSize, localResHeight + extraHeight),
                color, UIEditorVariables.SceneScrolling, zoomScale, true);
            
            DrawRectangle(new Rect(topLeftCorner.x,topLeftCorner.y,localResWidth,localResHeight),color,UIEditorVariables.SceneScrolling,zoomScale,true);


            DrawCircleGL(new Vector3(topLeftCorner.x, topLeftCorner.y, 0), borderSize, CircleQuadrantId.TopRight, color,
            zoomScale, true);
            DrawCircleGL(new Vector3(topLeftCorner.x + localResWidth, topLeftCorner.y, 0), borderSize,
                CircleQuadrantId.TopLeft, color, zoomScale, true);

            DrawCircleGL(new Vector3(topLeftCorner.x, topLeftCorner.y + localResHeight + extraHeight, 0), borderSize,
                CircleQuadrantId.BottomLeft, color, zoomScale, true);
            DrawCircleGL(
                new Vector3(topLeftCorner.x + localResWidth, topLeftCorner.y + localResHeight + extraHeight, 0),
                borderSize, CircleQuadrantId.BottomRight, color, zoomScale, true);
        }

        private void DrawCircleGL(Vector3 center, float size, CircleQuadrantId quadrant, Color color, float zoomScale,
            bool clip)
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

        public void DrawEditorGrid(Rect windowRect)
        {
            float squaresonX = (windowRect.width / (float) s_ChekerGridSize) + 2;
            float squaresonY = (windowRect.height / (float) s_ChekerGridSize) + 1;

            _CheckerGridMaterial.color = Color.white;
            _CheckerGridMaterial.mainTexture = _CheckerGridTexture;
            _CheckerGridMaterial.mainTextureScale = new Vector2(squaresonX, squaresonY);
            _CheckerGridMaterial.SetPass(0);


            DrawRectangle(new Rect(0, 0, windowRect.width, windowRect.height), Color.white, Vector2.zero);
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
    }
}