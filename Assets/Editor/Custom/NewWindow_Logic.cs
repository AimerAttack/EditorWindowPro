
using System;
using EditorUIMaker;
using UnityEngine;

public partial class NewWindow_Logic : EUM_BaseWindowLogic
{
    public NewWindow_Logic()
    {
        CallMethod("Init");
    }

    
    
    public string Dropdown1Str ="1";
    public int Dropdown1Index;
    public string[] Dropdown1Options = {"1","2","3"};
    
    public void Dropdown1ValueChange()
    {
        CallMethod("OnDropdown1ValueChange");
    }
    
    public bool Foldout1 = false;
    
    public void ClickButton1()
    {
        CallMethod("OnClickButton1");
    }
}
