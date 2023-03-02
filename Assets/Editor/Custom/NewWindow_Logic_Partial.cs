using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public partial class NewWindow_Logic
{
    int IDCounter = 0;

    List<TreeViewItem> GenerateRandomTree(int numTotalElements)
    {
        int numRootChildren = numTotalElements / 4;
        IDCounter = 0;
        var treeElements = new List<TreeViewItem>(numTotalElements);

        var root = new TreeViewItem(IDCounter,1,"Root");

        for (int i = 0; i < numRootChildren; ++i)
        {
            int allowedDepth = 6;
            AddChildrenRecursive(root, Random.Range(5, 10), true, numTotalElements, ref allowedDepth,
                treeElements);
        }

        return treeElements;
    }

    void AddChildrenRecursive(TreeViewItem element, int numChildren, bool force, int numTotalElements,
        ref int allowedDepth, List<TreeViewItem> treeElements)
    {
        if (element.depth >= allowedDepth)
        {
            allowedDepth = 0;
            return;
        }

        for (int i = 0; i < numChildren; ++i)
        {
            if (IDCounter > numTotalElements)
                return;

            var child = new TreeViewItem( IDCounter++,element.depth + 1,"Element " + IDCounter);
            treeElements.Add(child);

            if (!force && Random.value < 0.5f)
                continue;

            AddChildrenRecursive(child, Random.Range(5, 10), false, numTotalElements, ref allowedDepth,
                treeElements);
        }
    }

    public List<TreeViewItem> data = new List<TreeViewItem>();
    void DoInit()
    {
        
        data = GenerateRandomTree(10);
    }

    void OnBeforeReloadDoMain()
    {
        
    }
    
    
    void OnAfterReloadDoMain()
    {
        data = GenerateRandomTree(10);
    }
}