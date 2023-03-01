
using System;
using EditorUIMaker;
using UnityEngine;

public partial class NewWindow_Logic : EUM_BaseWindowLogic
{
    public NewWindow_Logic()
    {
        CallMethod("Init");
    }

    
    
    public Vector3 Vector3Field1;
    
    public void Vector3Field1ValueChange()
    {
        CallMethod("OnVector3Field1ValueChange");
    }
    
    public Vector3Int Vector3IntField1;
    
    public void Vector3IntField1ValueChange()
    {
        CallMethod("OnVector3IntField1ValueChange");
    }
    
    public bool Toggle1;
    
    public void Toggle1ValueChange()
    {
        CallMethod("OnToggle1ValueChange");
    }
    
    public float minMinMaxSlider2;
    public float maxMinMaxSlider2;
    
    public void MinMaxSlider2ValueChange()
    {
        CallMethod("OnMinMaxSlider2ValueChange");
    }

}
