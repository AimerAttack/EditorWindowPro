using System;
using System.Collections;
using System.Collections.Generic;
using Amazing.Editor.Library;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestEditor : EditorWindow, IEditorWindow
{
    [MenuItem("Tools/组件展示窗口")]
    static TestEditor OpenWindow()
    {
        var window = GetWindow<TestEditor>();
        window.Focus();
        window.Repaint();
        return window;
    }

    enum MyEnum
    {
        A,
        B,
        C
    }

    public bool NeedRepaint { get; set; }

    private ATabList _TabList;
    private ASplitView _SplitView1;
    private ASplitView _SplitView2;
    private bool _Toggle1;
    private MyEnum _MyEnum;
    private string _SelectStr = "1";
    private int _SliderInt = 0;
    private float _SliderFloat = 0;
    private float _MinSliderFloat = 0;
    private float _MaxSliderFloat = 0;
    private int _MinSliderInt = 0;
    private int _MaxSliderInt = 0;
    private Color _Color;
    private string _SearchString;
    private GameObject _Go;
    private Vector2 _ScrollPosition;
    private bool _Foldout;
    private SimpleTreeView _TreeView;
    private MultiColumnTreeView _MultiColumnTreeView;

    IList<TreeElement> GetData()
    {
        return GenerateRandomTree(130);
    }


    private void OnGUI()
    {
        GUILib.ScrollView(ref _ScrollPosition, () =>
        {
            if (_TabList == null)
                _TabList = ATabList.Create(this,
                    new List<object>() {"Uses", "Used By", "Duplicate", "GUIDs", "Unused Assets", "Uses in Build"});
            if (_SplitView1 == null)
            {
                _SplitView1 = new ASplitView(this)
                {
                    isHorz = false,
                    splits = new List<ASplitView.Info>()
                    {
                        new ASplitView.Info()
                        {
                            title = new GUIContent("Scene", GUIIconLib.Scene.image),
                            draw = (r) => { GUILayout.Button("test button 1"); }
                        },
                        new ASplitView.Info()
                        {
                            title = new GUIContent("Assets", GUIIconLib.Asset.image),
                            draw = (r) => { _SplitView2.Draw(); }
                        },
                    }
                };
                _SplitView1.CalculateWeight();
            }

            if (_SplitView2 == null)
            {
                _SplitView2 = new ASplitView(this)
                {
                    isHorz = true,
                    splits = new List<ASplitView.Info>()
                    {
                        new ASplitView.Info()
                        {
                            title = new GUIContent("Scene", GUIIconLib.Scene.image),
                            draw = (r) => { GUILayout.Button("test button 3"); }
                        },
                        new ASplitView.Info()
                        {
                            title = new GUIContent("Assets", GUIIconLib.Asset.image),
                            draw = (r) => { GUILayout.Button("test button 4"); }
                        },
                    }
                };
                _SplitView2.CalculateWeight();
            }

            if (_TreeView == null)
            {
                _TreeView = SimpleTreeView.Create(new GUIContent("test title"), 70);
                _TreeView.SetData(new List<TreeViewItem>()
                {
                    new TreeViewItem {id = 1, depth = 0, displayName = "Animals"},
                    new TreeViewItem {id = 2, depth = 1, displayName = "Mammals"},
                    new TreeViewItem {id = 3, depth = 2, displayName = "Tiger"},
                    new TreeViewItem {id = 4, depth = 2, displayName = "Elephant"},
                    new TreeViewItem {id = 5, depth = 2, displayName = "Okapi"},
                    new TreeViewItem {id = 6, depth = 2, displayName = "Armadillo"},
                    new TreeViewItem {id = 7, depth = 1, displayName = "Reptiles"},
                    new TreeViewItem {id = 8, depth = 2, displayName = "Crocodile"},
                    new TreeViewItem {id = 9, depth = 2, displayName = "Lizard"},
                });
            }

            if (_MultiColumnTreeView == null)
            {
                var headerState = MultiColumnTreeView.CreateDefaultMultiColumnHeaderState();
                var multiColumnHeader = new MultiColumnHeader(headerState);
                multiColumnHeader.ResizeToFit();
                var treeModel = new TreeModel<TreeElement>(GetData());
                _MultiColumnTreeView = new MultiColumnTreeView(new TreeViewState(), multiColumnHeader, treeModel, 300);
            }

           
            // _MultiColumnTreeView.Draw();

            GUILayout.Space(10);

            GUILib.Line();
            GUILib.HorizontalRect(() => { _TabList.Draw(); });
            GUILib.Line();

            // _TreeView.Draw();

            if (GUILib.Button("test button"))
            {
                Debug.Log("click test button");
            }

            if (GUILib.EnumPopup(ref _MyEnum, EditorGUIUtility.IconContent("ShurikenCheckMarkMixed"),
                    GUILayout.Width(50)))
            {
                Debug.Log(_MyEnum);
            }

            if (GUILib.Popup(ref _SelectStr, new string[] {"1", "4", "7"},
                    EditorGUIUtility.IconContent("ShurikenCheckMarkMixed"), GUILayout.Width(50)))
            {
                Debug.Log(_SelectStr);
            }

            GUILib.HelpBox("Test", MessageType.Info);

            GUILib.IntSlider("int slider", ref _SliderInt, 0, 10);

            GUILib.Slider("float slider", ref _SliderFloat, 0, 10);

            GUILib.MinMaxSlider("min max slider", ref _MinSliderFloat, ref _MaxSliderFloat, 0, 10);
            GUILib.MinMaxSlider("min max slider readonly", ref _MinSliderFloat, ref _MaxSliderFloat, 0, 10, true);

            GUILib.IntMinMaxSlider("int min max slider", ref _MinSliderInt, ref _MaxSliderInt, 0, 10);
            GUILib.IntMinMaxSlider("int min max slider readonly", ref _MinSliderInt, ref _MaxSliderInt, 0, 10, true);

            GUILib.Foldout("foldout", ref _Foldout);

            GUILib.Color("Color", ref _Color);

            GUILib.Toggle(ref _Toggle1, new GUIContent("Test toggle"));

            GUILib.HorizontalRect(() =>
            {
                GUILib.Label(new GUIContent("Search:"), GUILayout.ExpandWidth(false));
                GUILib.SearchBar(ref _SearchString);
            });

          
            Repaint();
            _SplitView1.Draw(); 


            GUILib.ObjectField("test go", ref _Go);

            GUILib.HorizontalRect(() =>
            {
                GUILib.Icon(GUIIconLib.Scene, 30, 30);
                GUILib.Icon(GUIIconLib.Folder, 30, 30);
                GUILib.Icon(GUIIconLib.Hierarchy, 30, 30);
                GUILib.Icon(GUIIconLib.Material, 30, 30);
                GUILib.Icon(GUIIconLib.Group, 30, 30);
                GUILib.Icon(GUIIconLib.Delete, 30, 30);
                GUILib.Icon(GUIIconLib.Split, 30, 30);
                GUILib.Icon(GUIIconLib.Prefab, 30, 30);
                GUILib.Icon(GUIIconLib.Favorite, 30, 30);
                GUILib.Icon(GUIIconLib.Setting, 30, 30);
                GUILib.Icon(GUIIconLib.Ignore, 30, 30);
                GUILib.Icon(GUIIconLib.Plus, 30, 30);
            });

            GUILib.HorizontalRect(() =>
            {
                GUILib.Icon(GUIIconLib.Asset, 30, 30);
                GUILib.Icon(GUIIconLib.Filesize, 30, 30);
                GUILib.Icon(GUIIconLib.AssetBundle, 30, 30);
                GUILib.Icon(GUIIconLib.Script, 30, 30);
                GUILib.Icon(GUIIconLib.Filter, 30, 30);
                GUILib.Icon(GUIIconLib.Visibility, 30, 30);
                GUILib.Icon(GUIIconLib.Panel, 30, 30);
                GUILib.Icon(GUIIconLib.Layout, 30, 30);
                GUILib.Icon(GUIIconLib.Sort, 30, 30);
                GUILib.Icon(GUIIconLib.Lock, 30, 30);
                GUILib.Icon(GUIIconLib.Unlock, 30, 30);
                GUILib.Icon(GUIIconLib.Refresh, 30, 30);
                GUILib.Icon(GUIIconLib.Selection, 30, 30);
            });

            if (NeedRepaint)
                Repaint();
        });
    }

    static int IDCounter = 0;

    public static List<TreeElement> GenerateRandomTree(int numTotalElements)
    {
        int numRootChildren = numTotalElements / 4;
        IDCounter = 0;
        var treeElements = new List<TreeElement>(numTotalElements);

        var root = new TreeElement("Root", -1, IDCounter);
        treeElements.Add(root);
        for (int i = 0; i < numRootChildren; ++i)
        {
            int allowedDepth = 6;
            AddChildrenRecursive(root, Random.Range(5, 10), true, numTotalElements, ref allowedDepth, treeElements);
        }

        return treeElements;
    }

    static void AddChildrenRecursive(TreeElement element, int numChildren, bool force, int numTotalElements,
        ref int allowedDepth, List<TreeElement> treeElements)
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

            var child = new TreeElement("Element " + IDCounter, element.depth + 1, ++IDCounter);
            treeElements.Add(child);

            if (!force && Random.value < 0.5f)
                continue;

            AddChildrenRecursive(child, Random.Range(5, 10), false, numTotalElements, ref allowedDepth, treeElements);
        }
    }
}