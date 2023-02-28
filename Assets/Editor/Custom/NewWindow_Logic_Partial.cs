using UnityEngine;

public partial class NewWindow_Logic
{
    void Init()
    {
        Slider1Max = 100;
    }

    void OnIntSlider1ValueChange()
    {
        Debug.Log(IntSlider1);
    }

    void OnVector2Field1ValueChange()
    {
        Debug.Log(Vector2Field1);
    }
}