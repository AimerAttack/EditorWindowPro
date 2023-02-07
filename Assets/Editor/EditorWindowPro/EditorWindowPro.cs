using UnityEditor;

namespace EditorWindowPro
{
    public class EditorWindowPro : EditorWindow
    {
        [MenuItem("Tools/EditorWindowPro")]
        static EditorWindowPro OpenWindow()
        {
            var window = GetWindow<EditorWindowPro>();
            window.Focus();
            window.Repaint();
            window.wantsMouseMove = true;
            return window;
        }
    }
}