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
    }
}