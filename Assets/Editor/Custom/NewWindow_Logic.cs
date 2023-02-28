
using System;
using EditorUIMaker;

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

}
