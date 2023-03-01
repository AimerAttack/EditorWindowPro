using UnityEngine;

public partial class NewWindow_Logic
{
    void Init()
    {
        Dropdown1Options = new []{"a","b","c"};
        Dropdown1Index = 0;
        Dropdown1Str = Dropdown1Options[Dropdown1Index];
    }

    void OnIntSlider1ValueChange()
    {
    }

    void OnVector2Field1ValueChange()
    {
    }

    void OnToggle1ValueChange()
    {
    }

    void OnDropdown1ValueChange()
    {
        Debug.Log(string.Format("{0},{1}",Dropdown1Str,Dropdown1Index));
    }
}