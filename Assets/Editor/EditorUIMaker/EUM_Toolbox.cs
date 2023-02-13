using System.Collections.Generic;
using Amazing.Editor.Library;
using EditorUIMaker.Widgets;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Toolbox : I_EUM_Drawable
    {
        private EUM_Title _Title;
        private Vector2 _ScrollPos;
        private bool _ShowBuildIn;
        private bool _ShowCustom;
        private bool _ShowContainers;
        private bool _ShowControls;
        private bool _ShowNumericFields;

        private List<EUM_Widget> _Controls = new List<EUM_Widget>();

        public EUM_Toolbox()
        {
            _Title = new EUM_Title(new GUIContent("Toolbox"));
            _ShowBuildIn = true;

            _Controls.Add(new EUM_Space());
            _Controls.Add(new EUM_Button());
            _Controls.Add(new EUM_Label());
        }

        public void Draw(ref Rect rect)
        {
            GUILib.Rect(rect, GUILib.s_DefaultColor, 1f);

            _Title.Draw(ref rect);

            GUILayout.BeginArea(rect);

            GUILayout.Space(10);
            GUILib.HorizontalRect(DrawToggleTab);

            GUILib.ScrollView(ref _ScrollPos, DrawControls);
            GUILayout.EndArea();
        }

        void DrawToggleTab()
        {
            GUILayout.FlexibleSpace();
            if (GUILib.Toggle(ref _ShowBuildIn, new GUIContent("BuildIn"), new GUIStyle("Button")))
            {
                if (_ShowBuildIn)
                    _ShowCustom = false;
            }

            if (GUILib.Toggle(ref _ShowCustom, new GUIContent("Custom"), new GUIStyle("Button")))
            {
                if (_ShowCustom)
                    _ShowBuildIn = false;
            }

            GUILayout.FlexibleSpace();
        }

        void DrawControls()
        {
            if (_ShowBuildIn)
                DrawBuildIn();
            if (_ShowCustom)
                DrawCustom();
        }

        void DrawBuildIn()
        {
            GUILib.Toggle(ref _ShowContainers, new GUIContent("Containers"), new GUIStyle("FoldoutHeader"));
            if (_ShowContainers)
                DrawContainers();

            GUILib.Toggle(ref _ShowControls, new GUIContent("Controls"), new GUIStyle("FoldoutHeader"));
            if (_ShowControls)
                DrawBaseControls();

            GUILib.Toggle(ref _ShowNumericFields, new GUIContent("NumericFields"), new GUIStyle("FoldoutHeader"));
            if (_ShowNumericFields)
                DrawNumericFields();
        }

        void DrawContainers()
        {
        }

        void DrawBaseControls()
        {
            foreach (var control in _Controls)
            {
                GUILayout.Label(control.TypeName);

                var lastRect = GUILayoutUtility.GetLastRect();
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    if (lastRect.Contains(Event.current.mousePosition))
                    {
                        DragAndDrop.PrepareStartDrag();
                        DragAndDrop.StartDrag("Create a new control");
                        Event.current.Use();

                        EUM_Helper.Instance.DraggingWidget = control.Clone();
                    }
                }
            }
        }

        void DrawNumericFields()
        {
        }

        void DrawCustom()
        {
        }

        public void HandleDrag()
        {
            switch (Event.current.type)
            {
                case EventType.DragPerform:
                {
                    if (EUM_Helper.Instance.DraggingWidget != null)
                    {
                        DragAndDrop.AcceptDrag();
                        Event.current.Use();

                        if (EUM_Helper.Instance.VitualWindowRect.Contains(Event.current.mousePosition) &&
                            EUM_Helper.Instance.ViewportRect.Contains(Event.current.mousePosition)
                           )
                        {
                            //in window
                            if (EUM_Helper.Instance.DraggingOverContainer != null)
                            {
                                //in container
                                EUM_Helper.Instance.DraggingOverContainer.Widgets.Add(EUM_Helper.Instance.DraggingWidget);
                            }
                        }
                    }
                    EUM_Helper.Instance.DraggingWidget = null;
 
                    break;
                }
                case EventType.DragUpdated:
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                 
                    break;
                }
            }
        }
    }
}