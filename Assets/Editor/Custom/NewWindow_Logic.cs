
using System;
using EditorUIMaker;
using UnityEngine;

public partial class NewWindow_Logic : EUM_BaseWindowLogic
{
    public NewWindow Window;

    public NewWindow_Logic(NewWindow window)
    {
        Window = window;
    }

    public void Init()
    {
        CallMethod("DoInit");
    }

    public void BeforeReloadDoMain()
    {
        CallMethod("OnBeforeReloadDoMain");
    }

    public void AfterReloadDoMain()
    {
        CallMethod("OnAfterReloadDoMain");
    }

    
    public void TreeView2SelectChange()
    {
        CallMethod("OnTreeView2SelectChange");
    }
    public void ClickButton1()
    {
        CallMethod("OnClickButton1");
    }
}
