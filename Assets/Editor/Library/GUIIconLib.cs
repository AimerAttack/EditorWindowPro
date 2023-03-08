using System.Collections.Generic;
using EditorUIMaker;
using UnityEditor;
using UnityEngine;

namespace Amazing.Editor.Library
{
    public static class GUIIconLib
    {
        private static readonly Dictionary<string, GUIContent> _cache = new Dictionary<string, GUIContent>();

        public static GUIContent TryGet(string id)
        {
            GUIContent result;
            if (_cache.TryGetValue(id, out result)) return result ?? GUIContent.none;
            GUIContent icon = null;
            if (string.IsNullOrEmpty(id))
            {
                var texture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/EditorUIMaker/Icon/default_icon.png");
                icon = new GUIContent(texture);
            }
            else
            {
                icon = EditorGUIUtility.IconContent(id) ?? new GUIContent(Texture2D.whiteTexture);
            }
            _cache.Add(id, icon);
            return icon;
        }
        
        public static GUIContent Horizontal
        {
            get { return TryGet("d_HorizontalLayoutGroup Icon"); }
        }
        public static GUIContent Scene { get { return TryGet("SceneAsset Icon"); } }
        public static GUIContent Folder { get { return TryGet("Project"); } }
        public static GUIContent Hierarchy { get { return TryGet("UnityEditor.HierarchyWindow"); } }
        public static GUIContent Material { get { return TryGet("d_TreeEditor.Material"); } }
        public static GUIContent Group { get { return TryGet("EditCollider"); } }
        public static GUIContent Delete { get { return TryGet("d_TreeEditor.Trash"); } }
        public static GUIContent Split { get { return TryGet("VerticalSplit"); } }
        public static GUIContent Prefab { get { return TryGet("d_Prefab Icon"); } }
        public static GUIContent Asset { get { return TryGet("Folder Icon"); } }
        public static GUIContent Filesize { get { return TryGet("SavePassive"); } }
        public static GUIContent AssetBundle { get { return TryGet("CloudConnect"); } }
        public static GUIContent Script { get { return TryGet("dll Script Icon"); } }
           
#if UNITY_2019_3_OR_NEWER
        public static GUIContent Filter { get { return TryGet("d_ToggleUVOverlay@2x"); } }
#else
    public static GUIContent Filter { get { return TryGet("LookDevSplit"); } }
#endif
        public static GUIContent Visibility { get { return TryGet("ClothInspector.ViewValue"); } }
#if UNITY_2019_3_OR_NEWER
        public static GUIContent Panel { get { return TryGet("VerticalSplit"); } }
#else
    public static GUIContent Panel { get { return TryGet("d_LookDevSideBySide"); } }
#endif
        public static GUIContent Layout { get { return TryGet("FreeformLayoutGroup Icon"); } }
        public static GUIContent Sort { get { return TryGet("AlphabeticalSorting"); } } //d_DefaultSorting
        public static GUIContent Lock { get { return TryGet("LockIcon-On"); } }
        public static GUIContent Unlock { get { return TryGet("LockIcon"); } }
    
#if UNITY_2019_3_OR_NEWER
        public static GUIContent Refresh { get { return TryGet("d_Refresh@2x"); } }
#else
    public static GUIContent Refresh { get { return TryGet("LookDevResetEnv"); } }
#endif
    
        public static GUIContent Selection { get { return TryGet("d_RectTransformBlueprint"); } }
        public static GUIContent Favorite { get { return TryGet("d_Favorite"); } }
        public static GUIContent Setting { get { return TryGet("d_SettingsIcon"); } }
        public static GUIContent Ignore { get { return TryGet("ShurikenCheckMarkMixed"); } }
        public static GUIContent Plus
        {
            get { return TryGet("ShurikenPlus"); }
        }
    }
}