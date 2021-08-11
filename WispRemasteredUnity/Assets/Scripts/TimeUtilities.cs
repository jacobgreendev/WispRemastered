using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUtilities : MonoBehaviour
{
    public static TimeUtilities Instance;
    private Coroutine currentTimeLerpRoutine;

    private void Awake()
    {
        Instance = this;
        SetTimeScaleAndFixedDeltaTime(1);
    }

    public void SetTimescale(float timescale)
    {
        if (currentTimeLerpRoutine != null) StopCoroutine(currentTimeLerpRoutine);
        SetTimeScaleAndFixedDeltaTime(timescale);
    }

    public static string GetMinuteSecondRepresentation(float totalSeconds)
    {
        var seconds = totalSeconds % 60;
        var minutes = Mathf.FloorToInt(totalSeconds / 60);
        var representation = $"{minutes.ToString().PadLeft(2, '0')}:{seconds.ToString().PadLeft(2, '0')}";
        return representation;
    }

    private static void SetTimeScaleAndFixedDeltaTime(float timescale)
    {
        Time.timeScale = timescale;
        Time.fixedDeltaTime = timescale / GameConstants.FixedUpdatesPerSecond;
    }

    public void LerpTime(float targetTimeScale, float lerpTime)
    {
        if (currentTimeLerpRoutine != null) StopCoroutine(currentTimeLerpRoutine);

        currentTimeLerpRoutine = StartCoroutine(LerpTimeRoutine(targetTimeScale, lerpTime));
    }

    public IEnumerator LerpTimeRoutine(float targetTimeScale, float lerpTime)
    {
        float time = 0f;
        float initialTimeScale = Time.timeScale;

        while (time < lerpTime)
        {
            time += Time.deltaTime;
            SetTimeScaleAndFixedDeltaTime(Mathf.Lerp(initialTimeScale, targetTimeScale, time / lerpTime));
            yield return null;
        }

        SetTimeScaleAndFixedDeltaTime(targetTimeScale);
    }
}
