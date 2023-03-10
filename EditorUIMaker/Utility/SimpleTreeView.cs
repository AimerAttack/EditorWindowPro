using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace EditorUIMaker
{
    public class SimpleTreeView : TreeView
    {
        public SearchField m_SearchField;
        public List<TreeViewItem> elements = new List<TreeViewItem>();
        public string Content;
        public float MinHeight;
        
        private SimpleTreeView(string content,TreeViewState treeViewState,float minHeight)
            : base(treeViewState)
        {
            MinHeight = minHeight;
            Content = content;
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

        public static SimpleTreeView Create(string content,float minHeight = 0)
        {
            var tree = new SimpleTreeView(content,new TreeViewState(),minHeight);
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
            GUILayout.Label(Content);
            GUILayout.FlexibleSpace();
            searchString = m_SearchField.OnToolbarGUI(searchString);
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
			
            SetupParentsAndChildrenFromDepths (root, elements);
			
            return root;
        }
        
        public Action OnSelectionChanged;
        protected override void SelectionChanged(IList<int> selectedIds)
        {
            base.SelectionChanged(selectedIds);
            OnSelectionChanged?.Invoke();
        }
    }
}