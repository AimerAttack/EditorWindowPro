using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorUIMaker.Utility
{
    public static class Utility
    {
        internal static string GetRelativePathInProject(string path)
        {
            var result = path.Replace(Application.dataPath, "Assets");
            return result;
        }

        internal static readonly Dictionary<GUIIconLib.E_Icon, Texture2D> _Cache = new Dictionary<GUIIconLib.E_Icon, Texture2D>();

        internal static Texture2D GetIcon(GUIIconLib.E_Icon iconType)
        {
            if (_Cache.TryGetValue(iconType, out var result))
            {
                return result;
            }

            if (GUIIconLib.IconInfo.ContainsKey(iconType))
            {
                var texture = ConvertBase64ToTexture(GUIIconLib.IconInfo[iconType]);
                _Cache.Add(iconType, texture);
                return texture;
            }
            else
            {
                return Texture2D.grayTexture;
            }
        }

        private static Texture2D ConvertBase64ToTexture(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            Texture2D texture = new Texture2D(500, 500);
            texture.LoadImage(bytes);
            return texture;
        }
    }
}