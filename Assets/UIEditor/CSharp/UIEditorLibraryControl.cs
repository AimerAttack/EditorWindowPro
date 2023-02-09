using UnityEngine;
using System.Collections;

public class UIEditorLibraryControl : MonoBehaviour
{
    public string DisplayName = "New Control";

#if UNITY_EDITOR
    public static bool RequiresToolboxRebuild;

    void Reset()
    {
        RequiresToolboxRebuild = true;
    }

    [ContextMenu("Update Toolbox")]
    public void UpdateToolbox()
    {
        RequiresToolboxRebuild = true;
    }
#endif
}
