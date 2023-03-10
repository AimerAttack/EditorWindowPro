using UnityEditor;
using UnityEngine;

namespace EditorUIMaker
{
    public class EUM_Toolbar : I_EUM_Drawable
    {
        public const float s_Height = 20;

        public EUM_Toolbar()
        {
            EUM_Helper.Instance.ZoomIndex = EUM_Helper.DefaultZoomIndex();
        }

        public void Draw(ref Rect rect)
        {
            var style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.UpperCenter;

            var drawRect = new Rect(rect.x, rect.y, rect.width, s_Height);
            rect.yMin += s_Height;

            GUILib.Area(drawRect, () =>
            {
                GUILib.HorizontalRect(() =>
                {
                    if (GUILib.Button("File", "ToolbarPopup", GUILayout.Width(40)))
                    {
                        var optionsMenu = new GenericMenu();
                        optionsMenu.AddItem(new GUIContent("Open"), false, EUM_Helper.Instance.OpenFile);
                        optionsMenu.AddItem(new GUIContent("New"), false, EUM_Helper.Instance.NewFile);
                        var buttonRect = GUILayoutUtility.GetLastRect();
                        var dropdownRect = new Rect(buttonRect);
                        dropdownRect.y += s_Height;
                        optionsMenu.DropDown(dropdownRect);
                    }

                    // GUILib.HorizontalRect(() =>
                    // {
                    //     GUILib.LabelField("Zoom:", style, GUILayout.Width(40));
                    //     GUILib.Popup(ref EUM_Helper.Instance.ZoomIndex, EUM_Helper.GetZoomScalesText(),
                    //         GUILayout.Width(60));
                    // });
                    //
                    // if (GUILib.Button("Fit Canvas", "ToolbarButton"))
                    // {
                    // }

                    if (GUILib.Button("Reset Position", "ToolbarButton"))
                    {
                        var viewPortRect = EUM_Helper.Instance.ViewportRect;
                        var windowRect = EUM_Helper.Instance.WindowRect;
                        windowRect.x = viewPortRect.x + (viewPortRect.width - windowRect.width) / 2;
                        windowRect.y = viewPortRect.y + (viewPortRect.height - windowRect.height) / 2;
                        EUM_Helper.Instance.WindowRect = windowRect;
                    }

                    if (GUILib.Button("Save", "ToolbarButton"))
                    {
                        EUM_Helper.Instance.SaveFile();
                    }

                    GUILib.HorizontalRect(() =>
                    {
                        GUILib.LabelField("MenuPath:", style, GUILayout.Width(70));
                        GUILib.TextField(ref EUM_Helper.Instance.MenuItemPath, GUILayout.ExpandWidth(true));
                    });

                    GUILib.FlexibleSpace();

                    if (GUILib.Toggle(ref EUM_Helper.Instance.Preview, new GUIContent("Preview"),
                            new GUIStyle("ToolbarButton")))
                    {
                        if (EUM_Helper.Instance.Preview)
                        {
                            EUM_Helper.Instance.SelectWidget = null;
                            EUM_Helper.Instance.HoverWidget = null;
                            EUM_Helper.Instance.OnSelectWidgetChange?.Invoke();
                        }
                    }
                }, EditorStyles.toolbar);
            });
        }
    }
}