using System;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EditorUIMaker : EditorWindow
    {
        [MenuItem("Tools/EditorUIMaker")]
        static void OpenWindow()
        {
            var window = GetWindow<EditorUIMaker>();
            window.titleContent = new GUIContent("EditorUIMaker");
            window.Show();
            window.Focus();
        }

        private EUM_Paper2 _Paper2;
        private EUM_Toolbar2 _Toolbar2;
        private EUM_ToolboxControls2 _ToolboxControls2;
        
        private EUM_Toolbox _Toolbox;
        private EUM_Viewport _Viewport;
        private EUM_Inspector _Inspector;

        public EditorUIMaker()
        {
            Init();
        }

        void Init()
        {
            // _Paper = new EUM_Paper();
            // _Toolbar = new EUM_Toolbar();
            // _ToolboxControls = new EUM_ToolboxControls();

            _Toolbox = new EUM_Toolbox();
            _Viewport = new EUM_Viewport();
            _Inspector = new EUM_Inspector();
        }

        private void OnLostFocus()
        {
        }

        private void OnFocus()
        {
        }

        private void OnGUI()
        {
            var windowRect = new Rect(0, 0, position.width, position.height);
            
            var viewportRect = new Rect(position.width/3, 0, position.width/3, position.height);
            _Viewport.Draw(ref viewportRect);
              
            var toolboxRect = new Rect(0, 0, position.width/3, position.height);
            _Toolbox.Draw(ref toolboxRect);
            
            var inspectorRect = new Rect(position.width*2/3, 0, position.width/3, position.height);
            _Inspector.Draw(ref inspectorRect);
            
            // _Paper.Draw(position.width, position.height);
            // _Paper.DrawEditorGrid(position);
            // _Paper.DrawVirtualWindow(UIEditorHelpers.GetZoomScaleFactor());
            // _Toolbar.Draw();
            // _ToolboxControls.Draw(position);
            //
            // _ToolboxControls.HandleToolboxDrags();
            //
            // ProcessMouseMove();
            //
            // ProcessScrollWheel();
            
            Repaint();
        }

        void ProcessScrollWheel()
        {
            if (Event.current.type == EventType.ScrollWheel)
            {
                var zoomDelta = UIEditorVariables.ZoomIndex;
                if (Event.current.delta.y < 0)
                    zoomDelta++;
                else
                {
                    zoomDelta--;
                }

                if (zoomDelta < 0)
                    zoomDelta = 0;
                if(zoomDelta >= UIEditorHelpers.ZoomScales.Length)
                    zoomDelta = UIEditorHelpers.ZoomScales.Length - 1;

                UIEditorVariables.ZoomIndex = zoomDelta;
            }
        }

        void ProcessMouseMove()
        {
            if (Event.current.isMouse)
            {
                if (Event.current.button == 2)
                {
                    if (Event.current.type == EventType.MouseDown)
                    {
                       
                    }
                }
            }
        }
    }
}