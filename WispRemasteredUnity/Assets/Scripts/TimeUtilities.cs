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
    }

    public void SetTimescale(float timescale)
    {
        if (currentTimeLerpRoutine != null) StopCoroutine(currentTimeLerpRoutine);
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
            Time.timeScale = Mathf.Lerp(initialTimeScale, targetTimeScale, time / lerpTime);
            yield return null;
        }

        Time.timeScale = targetTimeScale;
    }
}
