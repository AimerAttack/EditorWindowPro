using System.Collections.Generic;

namespace EditorUIMaker.Utility
{
    public static class GUIIconLib
    {
        public enum E_Icon
        {
            Null,
            Foldout,
            Horizontal,
            Vertical,
            ScrollView,
            Button, 
            Color,
            Dropdown,
            FlexibleSpace,
            GameObject,
            HelpBox,
            IntSlider,
            Label,
            Material,
            ProgressBar,
            Slider,
            Space,
            TextField,
            Toggle,
            TreeView,
            Double,
            Float,
            Int,
            Long,
            MinMaxIntSlider,
            MinMaxSlider,
            Vector2,
            Vector2Int,
            Vector3,
            Vector3Int,
        }

        public static Dictionary<E_Icon, string> IconInfo = new Dictionary<E_Icon, string>()
        {
        };
    }
}