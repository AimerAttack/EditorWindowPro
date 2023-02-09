//-----------------------------------------------------------------------------------------
// UI Framework
// Copyright © Argiris Baltzis - All Rights Reserved
//-----------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.Reflection;
using System.IO;

[InitializeOnLoad]
public class UIEditorStartup
{
    public static List<GameObject> Prefabs = new List<GameObject>();
    public static List<string> NewPrefabsToLoad = new List<string>();

    private static FileSystemWatcher prefabWatcher;

    static UIEditorStartup()
    {
//        BuildPrefabControls();
        MonitorDataPath();
    }

    static void MonitorDataPath()
    {
        if (prefabWatcher != null)
        {
            Debug.Log("prefabWatcher is not null");
        }

        prefabWatcher = new FileSystemWatcher(Application.dataPath, "*.prefab");
        prefabWatcher.IncludeSubdirectories = true;
        prefabWatcher.EnableRaisingEvents = true;

        prefabWatcher.Created += new FileSystemEventHandler(OnFile_Created);
    }

    static void OnFile_Created(object sender, FileSystemEventArgs e)
    {
        NewPrefabsToLoad.Add(e.FullPath);
    }

    public static void LoadNewPrefabs()
    {
      //  DateTime startTime = DateTime.Now;

        for (int i = 0; i < NewPrefabsToLoad.Count; ++i)
        {
            if (NewPrefabsToLoad[i] == null) continue;

            string newPath = NewPrefabsToLoad[i].Replace(@"\", "/").Replace(Application.dataPath, "Assets");

            UIEditorLibraryControl libraryControl = AssetDatabase.LoadAssetAtPath(newPath, typeof(UIEditorLibraryControl)) as UIEditorLibraryControl;
            if (libraryControl != null)
            {
                if (libraryControl.GetComponent<RectTransform>() == null) continue;

                if (!Prefabs.Contains(libraryControl.gameObject))
                {
                    Prefabs.Add(libraryControl.gameObject);
                    UIEditorLibraryControl.RequiresToolboxRebuild = true;
                }
            }
        }

        NewPrefabsToLoad.Clear();

       // Debug.Log("LoadNewPrefabs: " + (DateTime.Now - startTime).TotalSeconds.ToString());

    }

    public static void BuildPrefabControls()
    {
        DateTime startTime = DateTime.Now;

        Prefabs.Clear();

        DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
        FileInfo[] goFileInfo = directory.GetFiles("*.prefab", SearchOption.AllDirectories);

        for (int i = 0; i < goFileInfo.Length; i++)
        {
            if (goFileInfo[i] == null)
                continue;

            string tempFilePath = goFileInfo[i].FullName;
            tempFilePath = tempFilePath.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
            UIEditorLibraryControl tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, typeof(UIEditorLibraryControl)) as UIEditorLibraryControl;
            if (tempGO != null)
            {
                if (tempGO.GetComponent<RectTransform>() == null) continue;

                Prefabs.Add(tempGO.gameObject);
            }
        }

        Debug.Log("BuildPrefabControls: " + (DateTime.Now - startTime).TotalSeconds.ToString());
    }
}
