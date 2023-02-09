//-----------------------------------------------------------------------------------------
// UI Editor
// Copyright © Argiris Baltzis - All Rights Reserved
//
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
//-----------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

[System.Serializable]
public class UIEditorSceneSettings : MonoBehaviour
{
    public Camera BackgroundCamera;
    public BackgroundRenderId BackgroundRender = BackgroundRenderId.Color;
    public Color BackgroundColor = new Color(0.25f, 0.25f, 0.25f, 1);
}

public enum BackgroundRenderId
{
    None,
    Color,
    Scene,
}
