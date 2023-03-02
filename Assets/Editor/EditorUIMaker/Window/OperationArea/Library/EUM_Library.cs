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
        public bool ShowCustomContainer;
        public bool ShowCustomControl;

        public List<EUM_BaseWidget> Containers = new List<EUM_BaseWidget>();
        public List<EUM_BaseWidget> Controls = new List<EUM_BaseWidget>();
        public List<EUM_BaseWidget> NumericFields = new List<EUM_BaseWidget>();

        public List<EUM_BaseWidget> CustomContainers = new List<EUM_BaseWidget>();
        public List<EUM_BaseWidget> CustomControls = new List<EUM_BaseWidget>();

        public EUM_Library()
        {
            Title = new EUM_Title(new GUIContent("Library"));
            ShowBuildIn = true;

            Containers.Add(new EUM_Horizontal());
            Containers.Add(new EUM_Vertical());
            Containers.Add(new EUM_ScrollView());
            Containers.Add(new EUM_Foldout());

            Controls.Add(new EUM_Space());
            Controls.Add(new EUM_FlexibleSpace());
            Controls.Add(new EUM_Button());
            Controls.Add(new EUM_Label());
            Controls.Add(new EUM_TextField());
            Controls.Add(new EUM_Toggle());
            Controls.Add(new EUM_Dropdown());
            Controls.Add(new EUM_TreeView());

            NumericFields.Add(new EUM_Int());
            NumericFields.Add(new EUM_Slider());
            NumericFields.Add(new EUM_IntSlider());
            NumericFields.Add(new EUM_Float());
            NumericFields.Add(new EUM_Long());
            NumericFields.Add(new EUM_Double());
            NumericFields.Add(new EUM_Vector2());
            NumericFields.Add(new EUM_Vector2Int());
            NumericFields.Add(new EUM_Vector3());
            NumericFields.Add(new EUM_Vector3Int());
            NumericFields.Add(new EUM_MinMaxSlider());
            NumericFields.Add(new EUM_MinMaxIntSlider());

            var setting = EUM_Helper.Instance.GetSetting();
            if (setting.Containers != null)
            {
                foreach (var container in setting.Containers)
                {
                    CustomContainers.Add(container);
                }
            }

            if (setting.Widgets != null)
            {
                foreach (var widget in setting.Widgets)
                {
                    CustomControls.Add(widget);
                }
            }
        }

        public void Draw(ref Rect rect)
        {
            EUM_Helper.Instance.MouseRects.Add(rect);

            Title.Draw(ref rect);

            GUILib.Area(rect, () =>
            {
                GUILib.Space(10);
                GUILib.HorizontalRect(DrawToggleTab);
                GUILib.ScrollView(ref ScrollPos, DrawControls);
            });
        }

        void DrawToggleTab()
        {
            GUILib.FlexibleSpace();
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
            GUILib.FlexibleSpace();
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
                GUILib.HorizontalRect(() =>
                {
                    GUILib.Space(20);
                    GUILib.Label(control.TypeName);
                });

                if (!EUM_Helper.Instance.Preview)
                {
                    var lastRect = GUILib.GetLastRect();
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
                GUILib.HorizontalRect(() =>
                {
                    GUILib.Space(20);
                    GUILib.Label(control.TypeName);
                });

                var lastRect = GUILib.GetLastRect();
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
                GUILib.HorizontalRect(() =>
                {
                    GUILib.Space(20);
                    GUILib.Label(control.TypeName);
                });

                var lastRect = GUILib.GetLastRect();
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
            GUILib.Toggle(ref ShowCustomContainer, new GUIContent("Containers"), new GUIStyle("FoldoutHeader"));
            if (ShowCustomContainer)
                DrawCustomContainer();

            GUILib.Toggle(ref ShowCustomControl, new GUIContent("Controls"), new GUIStyle("FoldoutHeader"));
            if (ShowCustomControl)
                DrawCustomControl();
        }

        void DrawCustomContainer()
        {
            foreach (var control in CustomContainers)
            {
                GUILib.HorizontalRect(() =>
                {
                    GUILib.Space(20);
                    GUILib.Label(control.TypeName);
                });

                var lastRect = GUILib.GetLastRect();
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

        void DrawCustomControl()
        {
            foreach (var control in CustomControls)
            {
                GUILib.HorizontalRect(() =>
                {
                    GUILib.Space(20);
                    GUILib.Label(control.TypeName);
                });

                var lastRect = GUILib.GetLastRect();
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
                                EUM_Helper.Instance.AddToContainer(EUM_Helper.Instance.DraggingWidget,
                                    EUM_Helper.Instance.DraggingOverContainer);

                                EUM_Helper.Instance.SelectWidget = EUM_Helper.Instance.DraggingWidget;
                                EUM_Helper.Instance.OnSelectWidgetChange?.Invoke();
                            }
                        }
                        else
                        {
                            var treeView = EUM_Helper.Instance.TreeView;
                            if (treeView.ParentItem != null)
                            {
                                treeView.InsertToParent(EUM_Helper.Instance.DraggingWidget, treeView.ParentItem,
                                    treeView.InsertIndex);
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