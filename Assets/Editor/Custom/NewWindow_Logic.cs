
using System;
using EditorUIMaker;
using UnityEngine;

public partial class NewWindow_Logic : EUM_BaseWindowLogic
{
    
    public string TextField1;
    public void TextField1ValueChange()
    {
        CallMethod("OnTextField1ValueChange");
    }

    void OnTextField1ValueChange()
    {
        Debug.Log("OnTextField1ValueChange");
    }
}
