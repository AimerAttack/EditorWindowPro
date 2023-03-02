
using System;
using EditorUIMaker;
using UnityEngine;

public partial class NewWindow_Logic : EUM_BaseWindowLogic
{
    public NewWindow_Logic()
    {
        CallMethod("Init");
    }

    
    
    public string bStr ="1";
    public int bIndex;
    public string[] bOptions = {"1","2","3"};
    
    public void bValueChange()
    {
        CallMethod("OnbValueChange");
    }
    
    public bool c = false;
    
    public void ClickButton1()
    {
        CallMethod("OnClickButton1");
    }
    public void ClickButton2()
    {
        CallMethod("OnClickButton2");
    }
    public void ClickButton3()
    {
        CallMethod("OnClickButton3");
    }
    public string TextField1;
    public void TextField1ValueChange()
    {
        CallMethod("OnTextField1ValueChange");
    }

}
