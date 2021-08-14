using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnvironmentSettings", menuName = "Levels/New Environment Settings")]
public class EnvironmentSettings : ScriptableObject
{
    [Header("Fog Settings")]
    public Color fogColor;
    public FogMode fogMode;
    public float expFogDensity;
    public float linearFogStart;
    public float linearFogEnd;
    public bool skyboxFogEnabled;

    [Range(0, 1)]
    public float skyboxFogIntensity, skyboxFogHeight, skyboxFogSmoothness, skyboxFogFill;

    [Header("Lighting Settings")]
    public Color realtimeShadowColor;

    [Header("Skybox Settings")]
    public Color skyboxTintColor;

    [Range(0, 8)]
    public float skyboxExposure;
    
}
