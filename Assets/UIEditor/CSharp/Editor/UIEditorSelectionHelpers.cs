//-----------------------------------------------------------------------------------------
// UI Editor
// Copyright © Argiris Baltzis - All Rights Reserved
//
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
//-----------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class UIEditorSelectionHelpers
{
    public static UIEditorWindow.ObjectData MouseOverObject;
    public static UIEditorWindow.ObjectData MouseOverHighlightedObject;
    public static UIEditorWindow.ObjectData HighlightedObjectOnContextMenu;
    public static UIEditorWindow.ObjectData TemporaryParentChange;

    private static List<GameObject> LocalSelectionHelper = new List<GameObject>();


    public static List<GameObject> Selected
    {
        get
        {
            LocalSelectionHelper.Clear();
            for (int i = 0; i < Selection.objects.Length; ++i)
            {
                GameObject add = Selection.objects[i] as GameObject;
                if (add != null)
                {
                    RectTransform ui = add.GetComponent<RectTransform>();
                    if (ui != null)
                    {
                        LocalSelectionHelper.Add(ui.gameObject);
                    }
                }
            }

            return LocalSelectionHelper;
        }
    }

    public static bool SelectionContains(GameObject uiObject)
    {
        for (int i = 0; i < Selection.objects.Length; ++i)
        {
            if (Selection.objects[i] == uiObject)
            {
                return true;
            }
        }

        return false;
    }

    public static void ClearAllSelected()
    {
        Selection.activeObject = null;
    }

    public static void Select(GameObject uiObject)
    {
        Selection.activeObject = uiObject.gameObject;
    }

    public static void Select(List<GameObject> uiObjects)
    {
        UnityEngine.Object[] newObjects = new Object[uiObjects.Count];
        for (int i = 0; i < uiObjects.Count; ++i)
        {
            newObjects[i] = uiObjects[i].gameObject;
        }

        Selection.activeObject = null;
        Selection.objects = newObjects;
    }

    public static void AddToSelection(GameObject uiObject)
    {
        UnityEngine.Object[] newObjects = new Object[Selection.objects.Length + 1];
        for (int i = 0; i < Selection.objects.Length; ++i)
        {
            newObjects[i] = Selection.objects[i];
        }
        newObjects[newObjects.Length - 1] = uiObject.gameObject;

        Selection.activeObject = null;
        Selection.objects = newObjects;
    }

    public static void RemoveFromSelection(GameObject uiObject)
    {
        int counter = 0;
        UnityEngine.Object[] newObjects = new Object[Selection.objects.Length - 1];
        for (int i = 0; i < Selection.objects.Length; ++i)
        {
            if (uiObject == Selection.objects[i]) continue;

            newObjects[counter++] = Selection.objects[i];
        }

        Selection.activeObject = null;
        Selection.objects = newObjects;
    }
}


