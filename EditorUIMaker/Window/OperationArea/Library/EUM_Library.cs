using System;
using System.Collections.Generic;
using EditorUIMaker.Utility;
using EditorUIMaker.Widgets;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

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

        private float _WindowWidth;
        private GUIStyle _Style;
        private const float s_GUIMargin = 5;
        private const float itemSize = 60;

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
            Controls.Add(new EUM_Color());
            Controls.Add(new EUM_GameObject());
            Controls.Add(new EUM_Material());
            Controls.Add(new EUM_HelpBox());
            Controls.Add(new EUM_ProgressBar());

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
        }

        public void Draw(ref Rect rect)
        {
            EUM_Helper.Instance.MouseRects.Add(rect);

            Title.Draw(ref rect);

            _WindowWidth = rect.width;

            _Style = new GUIStyle(GUI.skin.label);
            _Style.alignment = TextAnchor.LowerCenter;
            _Style.wordWrap = true;
            _Style.fontSize = 11;

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

        private int _ContainerLineCount;
        private int _ContainerPerLineItemCount;
        private float _ContainerSpace;
        void DrawContainers()
        {
            DrawItems(Containers,ref _ContainerLineCount,ref _ContainerPerLineItemCount,ref _ContainerSpace);
        }

        private int _BaseControlLineCount;
        private int _BaseControlPerLineItemCount;
        private float _BaseControlSpace;
        void DrawBaseControls()
        {
            DrawItems(Controls,ref _BaseControlLineCount,ref _BaseControlPerLineItemCount,ref _BaseControlSpace);
        }

        private int _NumbericLineCount;
        private int _NumbericPerLineItemCount;
        private float _NumbericSpace;

        void DrawNumericFields()
        {
            DrawItems(NumericFields,ref _NumbericLineCount,ref _NumbericPerLineItemCount,ref _NumbericSpace);
        }

        void DrawItems(List<EUM_BaseWidget> items, ref int lineCount, ref int perLineItemCount, ref float space)
        {
            if (Event.current.type == EventType.Layout)
            {
                var itemCount = items.Count;
                perLineItemCount = Mathf.FloorToInt(_WindowWidth / itemSize);
                perLineItemCount = Mathf.Max(perLineItemCount, 1);
                lineCount = Mathf.CeilToInt(itemCount * 1f / perLineItemCount);
                space = (_WindowWidth - perLineItemCount * itemSize - s_GUIMargin * (perLineItemCount - 1)) /
                        (perLineItemCount + 1);
            }

            if (lineCount == 1)
            {
                GUILayout.BeginHorizontal();
                for (int i = 0; i < items.Count; i++)
                {
                    GUILayout.Label(items[i].TypeName, _Style, GUILayout.Width(itemSize), GUILayout.Height(itemSize));
                    var lastRect = GUILib.GetLastRect();
                    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                    {
                        if (lastRect.Contains(Event.current.mousePosition))
                        {
                            DragAndDrop.PrepareStartDrag();
                            DragAndDrop.SetGenericData("dragflag", "");
                            DragAndDrop.StartDrag("");
                            Event.current.Use();

                            EUM_Helper.Instance.DraggingWidget = items[i].Clone();
                        }
                    }

                    Rect rect = new Rect();
                    if (Event.current.type == EventType.Repaint)
                    {
                        rect = GUILayoutUtility.GetLastRect();
                        var height = _Style.CalcHeight(new GUIContent(items[i].TypeName), rect.width);
                        rect.yMax -= height;
                    }

                    GUI.DrawTexture(rect, Utility.Utility.GetIcon(items[i].IconType), ScaleMode.ScaleToFit);
                }

                GUILayout.EndHorizontal();
            }
            else if (lineCount > 1)
            {
                for (int lineIndex = 0; lineIndex < lineCount; lineIndex++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(_NumbericSpace);
                    for (int i = 0; i < perLineItemCount; i++)
                    {
                        var itemIndex = lineIndex * perLineItemCount + i;
                        if (itemIndex >= items.Count)
                            continue;
                        var item = items[itemIndex];
                        GUILayout.Label(item.TypeName, _Style, GUILayout.Width(itemSize), GUILayout.Height(itemSize));

                        var lastRect = GUILib.GetLastRect();
                        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                        {
                            if (lastRect.Contains(Event.current.mousePosition))
                            {
                                DragAndDrop.PrepareStartDrag();
                                DragAndDrop.SetGenericData("dragflag", "");
                                DragAndDrop.StartDrag("");
                                Event.current.Use();

                                EUM_Helper.Instance.DraggingWidget = item.Clone();
                            }
                        }

                        Rect rect = new Rect();
                        if (Event.current.type == EventType.Repaint)
                        {
                            rect = GUILayoutUtility.GetLastRect();
                            var height = _Style.CalcHeight(new GUIContent(item.TypeName), rect.width);
                            rect.yMax -= height;
                        }

                        GUILayout.Space(_NumbericSpace);

                        GUI.DrawTexture(rect, Utility.Utility.GetIcon(item.IconType), ScaleMode.ScaleToFit);
                    }

                    GUILayout.EndHorizontal();
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
            foreach (var control in EUM_Helper.Instance.CustomContainer)
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
            foreach (var control in EUM_Helper.Instance.CustomWidget)
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
                                var parentID = treeView.ParentItem.id;
                                var parentWidget = EUM_Helper.Instance.Widgets[parentID];
                                if (parentWidget is EUM_Container)
                                {
                                    treeView.InsertToParent(EUM_Helper.Instance.DraggingWidget, treeView.ParentItem,
                                        treeView.InsertIndex);
                                }
                                else
                                {
                                    var targetItemID = parentWidget.Parent.ID;
                                    var targetItem = treeView.FindItem(targetItemID);
                                    treeView.InsertToParent(EUM_Helper.Instance.DraggingWidget, targetItem,
                                        -1);
                                }
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