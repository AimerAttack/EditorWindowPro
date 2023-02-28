using System.Collections.Generic;
using Amazing.Editor.Library;
using EditorUIMaker.Utility;
using EditorUIMaker.Widgets;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Library : I_EUM_Drawable
    {
        public EUM_Title Title;
        public Vector2 ScrollPos;
        public bool ShowBuildIn;
        public bool ShowCustom;
        public bool ShowContainers;
        public bool ShowControls;
        public bool ShowNumericFields;

        public List<EUM_BaseWidget> Containers = new List<EUM_BaseWidget>();
        public List<EUM_BaseWidget> Controls = new List<EUM_BaseWidget>();
        public List<EUM_BaseWidget> NumericFields = new List<EUM_BaseWidget>();

        public EUM_Library()
        {
            Title = new EUM_Title(new GUIContent("Library"));
            ShowBuildIn = true;
            
            Containers.Add(new EUM_Horizontal());
            Containers.Add(new EUM_Vertical());
            Containers.Add(new EUM_ScrollView());

            Controls.Add(new EUM_Space());
            Controls.Add(new EUM_FlexibleSpace());
            Controls.Add(new EUM_Button());
            Controls.Add(new EUM_Label());
            Controls.Add(new EUM_TextField());
            Controls.Add(new EUM_Slider());
            Controls.Add(new EUM_IntSlider());
            
            NumericFields.Add(new EUM_IntField());
            NumericFields.Add(new EUM_FloatField());
        }

        public void Draw(ref Rect rect)
        {
            EUM_Helper.Instance.MouseRects.Add(rect);
            
            Title.Draw(ref rect);

            GUILayout.BeginArea(rect);

            GUILayout.Space(10);
            GUILib.HorizontalRect(DrawToggleTab);

            GUILib.ScrollView(ref ScrollPos, DrawControls);
            GUILayout.EndArea();
        }

        void DrawToggleTab()
        {
            GUILayout.FlexibleSpace();
            if (GUILib.Toggle(ref ShowBuildIn, new GUIContent("BuildIn"), new GUIStyle("Button")))
            {
                if (ShowBuildIn)
                    ShowCustom = false;
                else
                    ShowBuildIn = true;
            }

            if (GUILib.Toggle(ref ShowCustom, new GUIContent("Custom"), new GUIStyle("Button")))
            {
                if (ShowCustom)
                    ShowBuildIn = false;
                else
                {
                    ShowCustom = true;
                }
            }

            GUILayout.FlexibleSpace();
        }

        void DrawControls()
        {
            if (ShowBuildIn)
                DrawBuildIn();
            if (ShowCustom)
                DrawCustom();
        }

        void DrawBuildIn()
        {
            GUILib.Toggle(ref ShowContainers, new GUIContent("Containers"), new GUIStyle("FoldoutHeader"));
            if (ShowContainers)
                DrawContainers();

            GUILib.Toggle(ref ShowControls, new GUIContent("Controls"), new GUIStyle("FoldoutHeader"));
            if (ShowControls)
                DrawBaseControls();

            GUILib.Toggle(ref ShowNumericFields, new GUIContent("NumericFields"), new GUIStyle("FoldoutHeader"));
            if (ShowNumericFields)
                DrawNumericFields();
        }

        void DrawContainers()
        {
            foreach (var control in Containers)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label(control.TypeName);
                GUILayout.EndHorizontal();

                if (!EUM_Helper.Instance.Preview)
                {
                    var lastRect = GUILayoutUtility.GetLastRect();
                    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                    {
                        if (lastRect.Contains(Event.current.mousePosition))
                        {
                            DragAndDrop.PrepareStartDrag();
                            DragAndDrop.SetGenericData("dragflag", "");
                            DragAndDrop.StartDrag("");
                            Event.current.Use();

                            EUM_Helper.Instance.DraggingWidget = control.Clone();
                        }
                    }
                }
            }
        }

        void DrawBaseControls()
        {
            foreach (var control in Controls)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label(control.TypeName);
                GUILayout.EndHorizontal();

                var lastRect = GUILayoutUtility.GetLastRect();
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    if (lastRect.Contains(Event.current.mousePosition))
                    {
                        DragAndDrop.PrepareStartDrag();
                        DragAndDrop.SetGenericData("dragflag", "");
                        DragAndDrop.StartDrag("");
                        Event.current.Use();

                        EUM_Helper.Instance.DraggingWidget = control.Clone();
                    }
                }
            }
        }

        void DrawNumericFields()
        {
            foreach (var control in NumericFields)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label(control.TypeName);
                GUILayout.EndHorizontal();

                var lastRect = GUILayoutUtility.GetLastRect();
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    if (lastRect.Contains(Event.current.mousePosition))
                    {
                        DragAndDrop.PrepareStartDrag();
                        DragAndDrop.SetGenericData("dragflag", "");
                        DragAndDrop.StartDrag("");
                        Event.current.Use();

                        EUM_Helper.Instance.DraggingWidget = control.Clone();
                    }
                }
            }
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
                                EUM_Helper.Instance.AddToContainer(EUM_Helper.Instance.DraggingWidget,EUM_Helper.Instance.DraggingOverContainer);

                                EUM_Helper.Instance.SelectWidget = EUM_Helper.Instance.DraggingWidget;
                                EUM_Helper.Instance.OnSelectWidgetChange?.Invoke();
                                
                            }
                        }
                        else
                        {
                            var treeView = EUM_Helper.Instance.TreeView;
                            if (treeView.ParentItem != null)
                            {
                                treeView.InsertToParent(EUM_Helper.Instance.DraggingWidget,treeView.ParentItem,treeView.InsertIndex);
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