using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Amazing.Editor.Library
{
    class SimpleTreeView : TreeView
    {
        SearchField m_SearchField;
        private List<TreeViewItem> elements = new List<TreeViewItem>();
        private GUIContent m_Content;
        private float _MinHeight;

        private SimpleTreeView(GUIContent content,TreeViewState treeViewState,float minHeight)
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

        public static SimpleTreeView Create(GUIContent content,float minHeight = 0)
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
    }
}