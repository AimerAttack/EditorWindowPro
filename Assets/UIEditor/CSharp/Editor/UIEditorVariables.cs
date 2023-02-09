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

public enum EditorPrefId
{
    EditingPrefabPath,
    LastOpenSavePanelPath,
    SceneScrollingX,
    SceneScrollingY,
    AutoLoadLastEditingScene,
    DeviceWidth,
    DeviceHeight,
    GridSnap,
    Zoom,
    SceneLayerVisible,
    BackgroundColor,
    GridSize,
    GridVisible,
    ToolboxVisible,
    BackgroundRender,
    ZoomIndex,
    EdgeParentSnap,
    NeighborEdgeSnap,
    DisableAllSnap,
}

public static class UIEditorVariables
{
    //private static BackgroundRenderId cachedBackgroundRender;
    //private static bool cachedBackgroundRenderModified = true;

    //public static BackgroundRenderId BackgroundRender
    //{
    //    get
    //    {
    //        if (cachedBackgroundRenderModified)
    //        {
    //            cachedBackgroundRender = (BackgroundRenderId)EditorPrefs.GetInt(EditorPrefId.BackgroundRender.ToString(), (int)BackgroundRenderId.Color);
    //            cachedBackgroundRenderModified = false;
    //        }

    //        return cachedBackgroundRender;
    //    }

    //    set
    //    {
    //        EditorPrefs.SetInt(EditorPrefId.BackgroundRender.ToString(), (int)value);
    //        cachedBackgroundRenderModified = true;
    //    }
    //}

    private static int cachedGridSize;
    private static bool cachedGridSizeModified = true;

    public static int GridSize
    {
        get
        {
            if (cachedGridSizeModified)
            {
                cachedGridSize = EditorPrefs.GetInt(EditorPrefId.GridSize.ToString(), 2);
                cachedGridSizeModified = false;
            }

            return cachedGridSize;
        }

        set
        {
            EditorPrefs.SetInt(EditorPrefId.GridSize.ToString(), value);
            cachedGridSizeModified = true;
        }
    }

    //private static Color cachedBackgroundColor;
    //private static bool cachedBackgroundColorModified = true;

    //public static Color BackgroundColor
    //{
    //    get
    //    {
    //        if (cachedBackgroundColorModified)
    //        {
    //            Color darkGray = new Color(0.25f, 0.25f, 0.25f, 1);

    //            cachedBackgroundColor = UIEditorHelpers.ParseColor(
    //                EditorPrefs.GetString(
    //                EditorPrefId.BackgroundColor.ToString(),
    //                 UIEditorHelpers.ColorToString(darkGray)));
    //            cachedBackgroundColorModified = false;
    //        }

    //        return cachedBackgroundColor;
    //    }

    //    set
    //    {
    //        EditorPrefs.SetString(EditorPrefId.BackgroundColor.ToString(), UIEditorHelpers.ColorToString(value));
    //        cachedBackgroundColorModified = true;
    //    }
    //}

    private static bool cachedSceneLayerVisible;
    private static bool cachedSceneLayerVisibleModified = true;

    public static bool HasSceneLayerVisible
    {
        get
        {
            return EditorPrefs.HasKey(EditorPrefId.SceneLayerVisible.ToString());
        }
    }

    public static bool SceneLayerVisible
    {
        get
        {
            if (cachedSceneLayerVisibleModified)
            {
                cachedSceneLayerVisible = EditorPrefs.GetBool(EditorPrefId.SceneLayerVisible.ToString(), false);
                cachedSceneLayerVisibleModified = false;
            }

            return cachedSceneLayerVisible;
        }

        set
        {
            EditorPrefs.SetBool(EditorPrefId.SceneLayerVisible.ToString(), value);
            cachedSceneLayerVisibleModified = true;
        }
    }

    private static bool cachedGridVisible;
    private static bool cachedGridVisibleModified = true;

    public static bool GridVisible
    {
        get
        {
            if (cachedGridVisibleModified)
            {
                cachedGridVisible = EditorPrefs.GetBool(EditorPrefId.GridVisible.ToString(), false);
                cachedGridVisibleModified = false;
            }

            return cachedGridVisible;
        }

        set
        {
            EditorPrefs.SetBool(EditorPrefId.GridVisible.ToString(), value);
            cachedGridVisibleModified = true;
        }
    }


    private static bool cachedToolboxVisible;
    private static bool cachedToolboxVisibleModified = true;

    public static bool ToolboxVisible
    {
        get
        {
            if (cachedToolboxVisibleModified)
            {
                cachedToolboxVisible = EditorPrefs.GetBool(EditorPrefId.ToolboxVisible.ToString(), true);
                cachedToolboxVisibleModified = false;
            }

            return cachedToolboxVisible;
        }

        set
        {
            EditorPrefs.SetBool(EditorPrefId.ToolboxVisible.ToString(), value);
            cachedToolboxVisibleModified = true;
        }
    }

    //private static float cachedZoom;
    //private static bool cachedZoomModified = true;

    //public static float Zoom
    //{
    //    get
    //    {
    //        if (cachedZoomModified)
    //        {
    //            cachedZoom = EditorPrefs.GetFloat(EditorPrefId.Zoom.ToString(), 1);
    //            cachedZoomModified = false;
    //        }

    //        return cachedZoom;
    //    }

    //    set
    //    {
    //        EditorPrefs.SetFloat(EditorPrefId.Zoom.ToString(), value);
    //        cachedZoomModified = true;
    //    }
    //}


    private static int cachedZoomIndex;
    private static bool cachedZoomIndexModified = true;

    public static int ZoomIndex
    {
        get
        {
            if (cachedZoomIndexModified)
            {
                cachedZoomIndex = EditorPrefs.GetInt(EditorPrefId.ZoomIndex.ToString(), UIEditorHelpers.DefaultZoomIndex());
                cachedZoomIndexModified = false;
            }

         //  // if (cachedZoomIndex < 0) cachedZoomIndex = 0;
         //   if (cachedZoomIndex >= UIEditorHelpers.ZoomScales.Length) cachedZoomIndex = UIEditorHelpers.ZoomScales.Length - 1;

            return cachedZoomIndex;
        }

        set
        {
            EditorPrefs.SetInt(EditorPrefId.ZoomIndex.ToString(), value);
            cachedZoomIndexModified = true;
        }
    }

    private static bool cachedGridSnap;
    private static bool cachedGridSnapModified = true;

    public static bool GridSnap
    {
        get
        {
            if (cachedGridSnapModified)
            {
                cachedGridSnap = EditorPrefs.GetBool(EditorPrefId.GridSnap.ToString(), true);
                cachedGridSnapModified = false;
            }

            return cachedGridSnap;
        }

        set
        {
            EditorPrefs.SetBool(EditorPrefId.GridSnap.ToString(), value);
            cachedGridSnapModified = true;
        }
    }

    private static bool cachedEdgeParentSnap;
    private static bool cachedEdgeParentSnapModified = true;

    public static bool EdgeParentSnap
    {
        get
        {
            if (cachedEdgeParentSnapModified)
            {
                cachedEdgeParentSnap = EditorPrefs.GetBool(EditorPrefId.EdgeParentSnap.ToString(), true);
                cachedEdgeParentSnapModified = false;
            }

            return cachedEdgeParentSnap;
        }

        set
        {
            EditorPrefs.SetBool(EditorPrefId.EdgeParentSnap.ToString(), value);
            cachedEdgeParentSnapModified = true;
        }
    }

    private static bool cachedNeighborEdgeSnap;
    private static bool cachedNeighborEdgeSnapModified = true;

    public static bool NeighborEdgeSnap
    {
        get
        {
            if (cachedNeighborEdgeSnapModified)
            {
                cachedNeighborEdgeSnap = EditorPrefs.GetBool(EditorPrefId.NeighborEdgeSnap.ToString(), true);
                cachedNeighborEdgeSnapModified = false;
            }

            return cachedNeighborEdgeSnap;
        }

        set
        {
            EditorPrefs.SetBool(EditorPrefId.NeighborEdgeSnap.ToString(), value);
            cachedNeighborEdgeSnapModified = true;
        }
    }


    private static bool cachedDisableAllSnap;
    private static bool cachedDisableAllSnapModified = true;

    public static bool DisableAllSnap
    {
        get
        {
            if (cachedDisableAllSnapModified)
            {
                cachedDisableAllSnap = EditorPrefs.GetBool(EditorPrefId.DisableAllSnap.ToString(), false);
                cachedDisableAllSnapModified = false;
            }

            return cachedDisableAllSnap;
        }

        set
        {
            EditorPrefs.SetBool(EditorPrefId.DisableAllSnap.ToString(), value);
            cachedDisableAllSnapModified = true;
        }
    }

    private static int cachedDeviceWidth;
    //private static bool cachedDeviceWidthModified = true;

    public static int DeviceWidth
    {
        get
        {
            return (int)Handles.GetMainGameViewSize().x;
          
            //if (cachedDeviceWidthModified)
            //{
            //    cachedDeviceWidth = EditorPrefs.GetInt(EditorPrefId.DeviceWidth.ToString(), 1280);
            //    cachedDeviceWidthModified = false;
            //}

            //return cachedDeviceWidth;
        }

        //set
        //{
        //    EditorPrefs.SetInt(EditorPrefId.DeviceWidth.ToString(), value);
        //    cachedDeviceWidthModified = true;
        //}
    }

    private static int cachedDeviceHeight;
    //private static  bool cachedDeviceHeightModified = true;

    public static int DeviceHeight
    {
        get
        {
            return (int)Handles.GetMainGameViewSize().y;

            //if (cachedDeviceHeightModified)
            //{
            //    cachedDeviceHeight = EditorPrefs.GetInt(EditorPrefId.DeviceHeight.ToString(), 720);
            //    cachedDeviceHeightModified = false;
            //}

            //return cachedDeviceHeight;
        }
       
        //set
        //{
        //    EditorPrefs.SetInt(EditorPrefId.DeviceHeight.ToString(), value);
        //    cachedDeviceHeightModified = true;
        //}
    }

    public static Vector2 SceneScrolling
    {
        get
        {
            return new Vector2(EditorPrefs.GetFloat(EditorPrefId.SceneScrollingX.ToString()), EditorPrefs.GetFloat(EditorPrefId.SceneScrollingY.ToString()));
        }
        set
        {
            EditorPrefs.SetFloat(EditorPrefId.SceneScrollingX.ToString(), value.x);
            EditorPrefs.SetFloat(EditorPrefId.SceneScrollingY.ToString(), value.y);
        }
    }
    
    public static void SetVariable(EditorPrefId id, string path)
    {
        EditorPrefs.SetString(id.ToString(), path);
    }

    public static string GetVariable(EditorPrefId id)
    {
        return EditorPrefs.GetString(id.ToString());
    }
}


