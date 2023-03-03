
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

    public GameObject GameObject1;
    public void GameObject1ValueChange()
    {
        CallMethod("OnGameObject1ValueChange");
    }
    
    public Material Material1;
    public void Material1ValueChange()
    {
        CallMethod("OnMaterial1ValueChange");
    }
    
    public float ProgressBar1;
}
