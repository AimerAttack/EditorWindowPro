using System.Collections.Generic;
using Amazing.Editor.Library;
using EditorUIMaker.Utility;
using EditorUIMaker.Widgets;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Hierarchy : I_EUM_Drawable
    {
        public EUM_Title Title;
        public HierarchyTreeView TreeView;

        private bool _Inited;

        public EUM_Hierarchy()
        {
            Title = new EUM_Title(new GUIContent("Hierarchy"));
            EUM_Helper.Instance.OnAddItemToWindow += OnAddItemToContainer;
            EUM_Helper.Instance.OnSelectWidgetChange += OnSelectWidgetChanged;
            EUM_Helper.Instance.OnRemoveItemFromWindow += OnRemoveItemFromWindow;
            EUM_Helper.Instance.OnItemRename += OnItemRename;
            EUM_Helper.Instance.OnClearData += ClearData;
            EUM_Helper.Instance.OnBeforeReloadDomain += OnBeforeReloadDomain;
            EUM_Helper.Instance.OnAfterReloadDomain += OnAfterReloadDomain;
        }

        public IList<int> expendIds;

        void OnBeforeReloadDomain()
        {
            expendIds = TreeView.GetExpanded();
        }

        void OnAfterReloadDomain()
        {
            _Inited = false;
            InitIfNeed(); 
        }

        void ClearData()
        {
            if (expendIds != null)
                expendIds = new List<int>();
            _Inited = false;
            InitIfNeed();
        }

        void OnItemRename(EUM_BaseWidget widget)
        {
            TreeView.SetName(widget.ID,widget.Info.DisplayName);
        }

        void InitIfNeed()
        {
            if(_Inited)
                return;
            _Inited = true;
            
            TreeView = HierarchyTreeView.Create(new GUIContent("Controls"), 70);
            EUM_Helper.Instance.TreeView = TreeView;
            
            TreeView.OnDragItemToContainer += OnDragItemToContainer;
            TreeView.OnInsertToParent += OnInsertToParent;
            RefreshTreeView();
            OnSelectWidgetChanged();
            
            if(expendIds != null)
                TreeView.SetExpanded(expendIds);
        }
        
        
        public void Draw(ref Rect rect)
        {
            Title.Draw(ref rect);
            
            GUILib.Area(rect, () =>
            {
                GUILib.VerticelRect(() =>
                {
                    InitIfNeed();
                    TreeView.Draw();
                });
            });
        }

        void OnRemoveItemFromWindow(EUM_BaseWidget widget)
        {
            var queue = new Queue<EUM_BaseWidget>();
            queue.Enqueue(widget);

            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                if (item is EUM_Container container)
                {
                    foreach (var child in container.Widgets)
                    {
                        queue.Enqueue(child);
                    }

                    EUM_Helper.Instance.Containers.Remove(container);
                }

                EUM_Helper.Instance.Widgets.Remove(item.ID);
            }
            
            RefreshTreeView();
        }
        
        void OnAddItemToContainer(EUM_BaseWidget widget)
        {
            EUM_Helper.Instance.Widgets.Add(widget.ID, widget);
            RefreshTreeView();
        }

        void RefreshTreeView()
        {
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

            TreeView.SetData(nodes);
        }
        
        private void OnSelectWidgetChanged()
        {
            EUM_Helper.Instance.ClearFocus();
            
            var selectWidget = EUM_Helper.Instance.SelectWidget;
            if (selectWidget == null)
            {
                TreeView.SetSelection(new List<int>());
            }
            else
            {
                TreeView.SetSelection(new List<int>() {selectWidget.ID},TreeViewSelectionOptions.FireSelectionChanged | TreeViewSelectionOptions.RevealAndFrame);
            }
        }


        void OnInsertToParent(EUM_BaseWidget widget, int parentID, int index)
        {
            var container = EUM_Helper.Instance.Widgets[parentID];
            
            EUM_Helper.Instance.AddToContainer(widget,container as EUM_Container,index);

            EUM_Helper.Instance.SelectWidget = widget;
            EUM_Helper.Instance.OnSelectWidgetChange?.Invoke(); 
            
            EUM_Helper.Instance.DraggingWidget = null;
        }

        void OnDragItemToContainer(int itemID, int parentID, int index)
        {
            var parentWidget = EUM_Helper.Instance.Widgets[parentID];
            if(parentWidget is not EUM_Container container)
                return;
            var widget = EUM_Helper.Instance.Widgets[itemID];
            if (widget.Parent == container)
            {
                //同容器之间，切换index
                var curIndex = widget.Parent.Widgets.IndexOf(widget);
                if (curIndex != index)
                {
                    if (curIndex > index)
                    {
                        //往前移动
                        widget.Parent.Widgets.RemoveAt(curIndex);
                        widget.Parent.Widgets.Insert(index,widget);
                    }
                    else
                    {
                        //往后移动
                        widget.Parent.Widgets.RemoveAt(curIndex);
                        widget.Parent.Widgets.Insert(index - 1,widget);
                    }
                }
            }
            else
            {
                //不同容器之间，移动
                widget.Parent.Widgets.Remove(widget);
                container.Widgets.Insert(index,widget);
                widget.OnAddToContainer(container);
            }
            RefreshTreeView();
            EUM_Helper.Instance.OnItemIndexChange?.Invoke();
        }
    }
}