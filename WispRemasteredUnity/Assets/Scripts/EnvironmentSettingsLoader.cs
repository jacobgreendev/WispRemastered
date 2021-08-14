using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSettingsLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneData.levelToLoad == null) throw new UnassignedReferenceException("SceneData has no reference to loaded level");
        if (SceneData.levelToLoad.environmentSettings == null) throw new UnassignedReferenceException("Loaded level has no environment settings");

        var settings = SceneData.levelToLoad.environmentSettings;
        RenderSettings.fogColor = settings.fogColor;
        RenderSettings.fogMode = settings.fogMode;
        RenderSettings.fogDensity = settings.expFogDensity;
        RenderSettings.fogStartDistance = settings.linearFogStart;
        RenderSettings.fogEndDistance = settings.linearFogEnd;
        RenderSettings.skybox.SetFloat("_EnableFog", settings.skyboxFogEnabled ? 1 : 0);
        RenderSettings.skybox.SetFloat("_FogIntensity", settings.skyboxFogIntensity);
        RenderSettings.skybox.SetFloat("_FogHeight", settings.skyboxFogHeight);
        RenderSettings.skybox.SetFloat("_FogSmoothness", settings.skyboxFogSmoothness);
        RenderSettings.skybox.SetFloat("_FogFill", settings.skyboxFogFill);

        RenderSettings.subtractiveShadowColor = settings.realtimeShadowColor;

        RenderSettings.skybox.SetFloat("_Exposure", settings.skyboxExposure);
        RenderSettings.skybox.SetColor("_TintColor", settings.skyboxTintColor);
    }
}
