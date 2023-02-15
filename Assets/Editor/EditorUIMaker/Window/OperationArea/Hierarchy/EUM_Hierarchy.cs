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

            Queue<EUM_BaseWidget> queue = new Queue<EUM_BaseWidget>();
            queue.Enqueue(EUM_Helper.Instance.Window);
            while (queue.Count > 0)
            {
                var widget = queue.Dequeue();

                if (widget is EUM_Container container)
                {
                    foreach (var child in container.Widgets)
                    {
                        queue.Enqueue(child);
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
    }
}