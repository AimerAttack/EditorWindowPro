using System;
using System.Collections.Generic;
using System.Linq;
using Amazing.Editor.Library;
using EditorUIMaker.Widgets;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace EditorUIMaker.Utility
{
    public class HierarchyTreeView : TreeView
    {
        public const string k_GenericDragID = "GenericDragColumnDragging";

        public SearchField SearchField;
        public List<TreeViewItem> Elements = new List<TreeViewItem>();
        public GUIContent Content;
        public float MinHeight;
        public bool Dragging = false;

        public TreeViewItem HoverItem
        {
            get
            {
                if (Dragging)
                    return null;
                var windowID = EUM_Helper.Instance.Window.ID;
                if (hoveredItem != null)
                {
                    var selectWidget = EUM_Helper.Instance.SelectWidget;
                    if (selectWidget != null)
                    {
                        var selectID = selectWidget.ID;
                        if (hoveredItem.id == selectID)
                            return null;
                        else
                        {
                            return hoveredItem.id != windowID ? hoveredItem : null;
                        }
                    }
                    else
                    {
                        return hoveredItem.id != windowID ? hoveredItem : null;
                    }
                }
                else
                {
                    //检查有没有hover的
                    var hoverWidget = EUM_Helper.Instance.HoverWidget;
                    if (hoverWidget != null)
                    {
                        var selectWidget = EUM_Helper.Instance.SelectWidget;
                        if (selectWidget != null)
                        {
                            var selectID = selectWidget.ID;
                            if (hoverWidget.ID == selectID)
                                return null;
                            else
                            {
                                var item = FindItem(hoverWidget.ID, rootItem);
                                if (item != null)
                                    return item.id != windowID ? item : null;
                                else
                                {
                                    return null;
                                }
                            }
                        }
                        else
                        {
                            var item = FindItem(hoverWidget.ID, rootItem);
                            if (item != null)
                                return item.id != windowID ? item : null;
                            else
                            {
                                return null;
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }
                }

                return null;
            }
        }

        public void SetName(int id, string name)
        {
            var item = FindItem(id, rootItem);
            item.displayName = name;
        }

        private HierarchyTreeView(GUIContent content, TreeViewState treeViewState, float minHeight)
            : base(treeViewState)
        {
            MinHeight = minHeight;
            Content = content;
            enableItemHovering = true;
            Reload();
        }

        public void SetData(List<TreeViewItem> nodes)
        {
            Elements.Clear();
            foreach (var node in nodes)
            {
                Elements.Add(node);
            }

            Reload();
        }


        public static HierarchyTreeView Create(GUIContent content, float minHeight = 0)
        {
            var tree = new HierarchyTreeView(content, new TreeViewState(), minHeight);
            tree.SearchField = new SearchField();
            tree.SearchField.downOrUpArrowKeyPressed += tree.SetFocusAndEnsureSelectedItem;
            return tree;
        }

        public void Draw()
        {
            DoToolbar();
            DoTreeView();
        }

        void DoToolbar()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label(Content);
            GUILayout.FlexibleSpace();
            searchString = SearchField.OnToolbarGUI(searchString);
            GUILayout.EndHorizontal();
        }

        void DoTreeView()
        {
            Rect rect = GUILayoutUtility.GetRect(0, 100000, MinHeight, 100000);
            OnGUI(rect);
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem {id = 0, depth = -1, displayName = "Root"};

            SetupParentsAndChildrenFromDepths(root, Elements);

            return root;
        }

        protected override bool CanRename(TreeViewItem item)
        {
            var windowID = EUM_Helper.Instance.Window.ID;
            if (item.id == windowID)
                return false;
            return true;
        }

        protected override void RenameEnded(RenameEndedArgs args)
        {
            var newName = args.newName;
            var targetWidget = EUM_Helper.Instance.Widgets[args.itemID];
            var nameValid = EUM_Helper.Instance.NameValid(targetWidget,newName);
            if(!nameValid)
                return;
            
            EUM_Helper.Instance.Widgets[args.itemID].Info.Name = args.newName;
            EUM_Helper.Instance.Modified = true;
            var nodes = new List<TreeViewItem>();


            Stack<EUM_BaseWidget> bucket = new Stack<EUM_BaseWidget>();
            bucket.Push(EUM_Helper.Instance.Window);
            while (bucket.Count > 0)
            {
                var widget = bucket.Pop();

                if (widget is EUM_Container container)
                {
                    for (int i = container.Widgets.Count - 1; i >= 0; i--)
                    {
                        var child = container.Widgets[i];
                        bucket.Push(child);
                    }
                }

                var node = new TreeViewItem(widget.ID, widget.Depth, widget.Info.Name);
                nodes.Add(node);
            }

            SetData(nodes);
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            base.SelectionChanged(selectedIds);
            foreach (var selectedId in selectedIds)
            {
                if (EUM_Helper.Instance.Widgets.ContainsKey(selectedId))
                {
                    var widget = EUM_Helper.Instance.Widgets[selectedId];
                    EUM_Helper.Instance.SelectWidget = widget;
                }
                else
                {
                    EUM_Helper.Instance.SelectWidget = null;
                }
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            base.RowGUI(args);
        }

        protected override void AfterRowsGUI()
        {
            base.AfterRowsGUI();
            if (EUM_Helper.Instance.DraggingWidget == null)
            {
                if (HoverItem != null)
                {
                    var row = FindRowOfItem(HoverItem);
                    var rect = GetRowRect(row);
                    GUILib.Frame(rect, Color.cyan);
                }
            }
        }

        protected override bool CanStartDrag(CanStartDragArgs args)
        {
            return true;
        }

        protected override void SetupDragAndDrop(SetupDragAndDropArgs args)
        {
            if (hasSearch)
                return;
            DragAndDrop.PrepareStartDrag();
            var draggedRows = GetRows().Where(item => args.draggedItemIDs.Contains(item.id)).ToList();
            DragAndDrop.SetGenericData(k_GenericDragID, draggedRows);
            DragAndDrop.objectReferences = new UnityEngine.Object[] { }; // this IS required for dragging to work
            string title = draggedRows.Count == 1 ? draggedRows[0].displayName : "< Multiple >";
            DragAndDrop.StartDrag(title);
            Dragging = true;
        }

        public TreeViewItem ParentItem;
        public int InsertIndex;

        protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args)
        {
            // Check if we can handle the current drag data (could be dragged in from other areas/windows in the editor)
            var draggedRows = DragAndDrop.GetGenericData(k_GenericDragID) as List<TreeViewItem>;
            if (draggedRows == null)
            {
                if (EUM_Helper.Instance.DraggingWidget != null)
                {
                    switch (args.dragAndDropPosition)
                    {
                        case DragAndDropPosition.UponItem:
                        {
                            InsertIndex = -1;
                            ParentItem = args.parentItem;
                            return DragAndDropVisualMode.Move;
                        }
                        case DragAndDropPosition.BetweenItems:
                        {
                            InsertIndex = args.insertAtIndex;
                            ParentItem = args.parentItem;
                            return DragAndDropVisualMode.Move;
                        }
                        case DragAndDropPosition.OutsideItems:
                        {
                            InsertIndex = -1;
                            ParentItem = null;
                            return DragAndDropVisualMode.Move;
                        }
                        default:
                            Debug.LogError("Unhandled enum " + args.dragAndDropPosition);
                            return DragAndDropVisualMode.None;
                    }
                }
            }
            else
            {
                // Parent item is null when dragging outside any tree view items.
                switch (args.dragAndDropPosition)
                {
                    case DragAndDropPosition.UponItem:
                    case DragAndDropPosition.BetweenItems:
                    {
                        bool validDrag = ValidDrag(args.parentItem, draggedRows);
                        if (args.performDrop && validDrag)
                        {
                            OnDropDraggedElementsAtIndex(draggedRows, args.parentItem,
                                args.insertAtIndex == -1 ? 0 : args.insertAtIndex);
                            Dragging = false;
                        }

                        return validDrag ? DragAndDropVisualMode.Move : DragAndDropVisualMode.None;
                    }

                    case DragAndDropPosition.OutsideItems:
                    {
                        if (args.performDrop)
                        {
                            Dragging = false;
                        }

                        return DragAndDropVisualMode.Move;
                    }
                    default:
                        Debug.LogError("Unhandled enum " + args.dragAndDropPosition);
                        return DragAndDropVisualMode.None;
                }
            }

            return DragAndDropVisualMode.None;
        }

        public Action<int, int, int> OnDragItemToContainer;

        public virtual void OnDropDraggedElementsAtIndex(List<TreeViewItem> draggedRows, TreeViewItem parent,
            int insertIndex)
        {
            OnDragItemToContainer?.Invoke(draggedRows[0].id, parent.id, insertIndex);
        }

        public Action<EUM_BaseWidget, int, int> OnInsertToParent;
        public void InsertToParent(EUM_BaseWidget widget, TreeViewItem parent, int index)
        {
            OnInsertToParent?.Invoke(widget, parent.id, index);
        }

        public TreeViewItem FindItem(int id)
        {
            return FindItem(id, rootItem);
        }


        bool ValidDrag(TreeViewItem parent, List<TreeViewItem> draggedItems)
        {
            TreeViewItem currentParent = parent;
            while (currentParent != null)
            {
                if (draggedItems.Contains(currentParent))
                    return false;
                currentParent = currentParent.parent;
            }

            return true;
        }
    }
}