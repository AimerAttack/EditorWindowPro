
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

    public void ClickButton4()
    {
        CallMethod("OnClickButton4");
    }
    public Vector2 ScrollView1;
    public void ClickButton3()
    {
        CallMethod("OnClickButton3");
    }
}
