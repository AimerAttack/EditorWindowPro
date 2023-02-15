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
        public HierarchyTreeView TreeView => _TreeView;
        
        private EUM_Title _Title;
        private HierarchyTreeView _TreeView;

        public EUM_Hierarchy()
        {
            _Title = new EUM_Title(new GUIContent("Hierarchy"));
            EUM_Helper.Instance.OnAddItemToWindow += OnAddItemToContainer;
            EUM_Helper.Instance.OnSelectWidgetChange += OnSelectWidgetChanged;
        }

        public void Draw(ref Rect rect)
        {
            _Title.Draw(ref rect);
            GUILayout.BeginArea(rect);
            GUILayout.BeginVertical();
            if (_TreeView == null)
            {
                _TreeView = HierarchyTreeView.Create(new GUIContent("Controls"), 70);
                _TreeView.OnDragItemToContainer += OnDragItemToContainer;
                _TreeView.SetData(new List<TreeViewItem>());
            }

            _TreeView.Draw();
            GUILayout.EndVertical();
            GUILayout.EndArea();
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

                var node = new TreeViewItem(widget.ID, widget.Depth, widget.Name);
                nodes.Add(node);
            }

            _TreeView.SetData(nodes);
        }
        
        private void OnSelectWidgetChanged()
        {
            var selectWidget = EUM_Helper.Instance.SelectWidget;
            if (selectWidget == null)
            {
                _TreeView.SetSelection(new List<int>());
            }
            else
            {
                _TreeView.SetSelection(new List<int>() {selectWidget.ID},TreeViewSelectionOptions.FireSelectionChanged | TreeViewSelectionOptions.RevealAndFrame);
            }
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
        }
    }
}