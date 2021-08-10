using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelsPerUnitManager : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Image>().pixelsPerUnitMultiplier = GameConstants.ScreenWidthFor1PixelPerUnitSliced / Screen.width;
    }
}
