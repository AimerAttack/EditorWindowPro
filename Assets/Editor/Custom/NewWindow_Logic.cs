
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

    public Color Color1;
    
    public void Color1ValueChange()
    {
        CallMethod("OnColor1ValueChange");
    }

}
