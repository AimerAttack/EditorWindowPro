
using System;
using EditorUIMaker;
using UnityEngine;

public partial class NewWindow_Logic : EUM_BaseWindowLogic
{
    public NewWindow_Logic()
    {
        CallMethod("Init");
    }

    
    public string TextField1;
    public void TextField1ValueChange()
    {
        CallMethod("OnTextField1ValueChange");
    }
    
    public float Slider1;
    public float Slider1Min=0;
    public float Slider1Max=10;
    
    public void Slider1ValueChange()
    {
        CallMethod("OnSlider1ValueChange");
    }
    
    public int IntSlider1;
    public int IntSlider1Min=0;
    public int IntSlider1Max=10;
    
    public void IntSlider1ValueChange()
    {
        CallMethod("OnIntSlider1ValueChange");
    }
    
    public int IntField1;
    
    public void IntField1ValueChange()
    {
        CallMethod("OnIntField1ValueChange");
    }
    
    public float FloatField1;
    
    public void FloatField1ValueChange()
    {
        CallMethod("OnFloatField1ValueChange");
    }
    
    public long LongField1;
    
    public void LongField1ValueChange()
    {
        CallMethod("OnLongField1ValueChange");
    }
    
    public double DoubleField1;
    
    public void DoubleField1ValueChange()
    {
        CallMethod("OnDoubleField1ValueChange");
    }
    
    public Vector2 Vector2Field1;
    
    public void Vector2Field1ValueChange()
    {
        CallMethod("OnVector2Field1ValueChange");
    }

}
