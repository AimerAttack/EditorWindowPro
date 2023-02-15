using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace EditorUIMaker.Utility
{
    public class HierarchyTreeView : TreeView
    {
        SearchField m_SearchField;
        private List<TreeViewItem> elements = new List<TreeViewItem>();
        private GUIContent m_Content;
        private float _MinHeight;

        private HierarchyTreeView(GUIContent content,TreeViewState treeViewState,float minHeight)
            : base(treeViewState)
        {
            _MinHeight = minHeight;
            m_Content = content;
            Reload();
        }

        public void SetData(List<TreeViewItem> nodes)
        {
            elements.Clear();
            foreach (var node in nodes)
            {
                elements.Add(node);
            }
            Reload();
        }

        public static HierarchyTreeView Create(GUIContent content,float minHeight = 0)
        {
            var tree = new HierarchyTreeView(content,new TreeViewState(),minHeight);
            tree.m_SearchField = new SearchField();
            tree.m_SearchField.downOrUpArrowKeyPressed += tree.SetFocusAndEnsureSelectedItem;
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
            GUILayout.Label(m_Content);
            GUILayout.FlexibleSpace();
            searchString = m_SearchField.OnToolbarGUI(searchString);
            GUILayout.EndHorizontal();
        }

        void DoTreeView()
        {
            Rect rect = GUILayoutUtility.GetRect(0, 100000, _MinHeight, 100000);
            OnGUI(rect);
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem {id = 0, depth = -1, displayName = "Root"};
			
            SetupParentsAndChildrenFromDepths (root, elements);
			
            return root;
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
    }
}