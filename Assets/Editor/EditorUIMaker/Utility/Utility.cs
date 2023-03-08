using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.Utility
{
    public static class Utility
    {
        public static string GetRelativePathInProject(string path)
        {
            var result = path.Replace(Application.dataPath, "Assets");
            return result;
        }

        private static readonly Dictionary<string, GUIContent> _cache = new Dictionary<string, GUIContent>();

        public static GUIContent TryGet(string id)
        {
            GUIContent result;
            if (_cache.TryGetValue(id, out result)) return result ?? GUIContent.none;
            GUIContent icon = null;
            if (string.IsNullOrEmpty(id))
            {
                icon = GUIContent.none;
            }
            else
            {
                icon = EditorGUIUtility.IconContent(id) ?? new GUIContent(Texture2D.whiteTexture);
            }

            _cache.Add(id, icon);
            return icon;
        }
    }
}