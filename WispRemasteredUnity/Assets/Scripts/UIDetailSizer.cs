using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDetailSizer : MonoBehaviour
{
    private void Awake()
    {
        var image = GetComponent<Image>();
        if (image != null) image.pixelsPerUnitMultiplier = GameConstants.PixelPerUnitReferenceScreenWidth / Screen.width;

        var shadow = GetComponent<Shadow>();
        if (shadow != null) shadow.effectDistance = new Vector2(1, -1) * GameConstants.DefaultShadowSize * (Screen.width / GameConstants.ShadowReferenceScreenWidth);
    }
}
