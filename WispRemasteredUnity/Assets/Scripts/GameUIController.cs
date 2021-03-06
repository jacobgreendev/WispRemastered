using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIController : UIBase
{
    public static GameUIController Instance;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private TextMeshProUGUI timeElapsedText;
    [SerializeField] private Slider timeoutSlider;
    [SerializeField] private GameObject achievementPanel;
    [SerializeField] private TextMeshProUGUI achievementTitleText, achievementLabel;

    [Header("Level Complete Elements")]
    [SerializeField] private GameObject levelCompleteScreen;
    [SerializeField] private StarCounter levelCompleteScoreStars, levelCompleteTimeStars;
    [SerializeField] private TextMeshProUGUI levelCompleteScoreText, levelCompleteTimeText;
    [SerializeField] private Button levelCompleteReturnToMenuButton;

    [Header("Score Text Values")]
    [SerializeField] private AnimationCurve scoreAnimationScaleCurve, scoreAnimationAlphaCurve;
    [SerializeField] private float scoreAnimationDuration, scoreAnimationScaleMultiplier;

    [Header("Popup Text Values")]
    [SerializeField] private AnimationCurve popupAnimationScaleCurve, popupAnimationAlphaCurve;
    [SerializeField] private float popupAnimationDuration, popupAnimationScaleMultiplier;

    [Header("Timeout Slider Values")]
    [Range(0f, 1f)]
    [SerializeField] private float timeoutSliderThreshold;


    private Coroutine scoreTextAnimationRoutine, popupTextAnimationRoutine;
    private Vector3 scoreTextInitialScale, popupTextInitialScale;
    private bool timeoutSliderEnabled = true;

    private void Awake()
    {
        Instance = this;

        scoreText.ForceMeshUpdate();
        var scoreTextFontSize = scoreText.fontSize;
        scoreText.enableAutoSizing = false;
        scoreText.fontSize = scoreTextFontSize;

        popupText.ForceMeshUpdate();
        var popupTextFontSize = popupText.fontSize;
        popupText.enableAutoSizing = false;
        popupText.fontSize = popupTextFontSize;

        timeElapsedText.ForceMeshUpdate();
        var timeElapsedTextFontSize = timeElapsedText.fontSize;
        timeElapsedText.enableAutoSizing = false;
        timeElapsedText.fontSize = timeElapsedTextFontSize;

        scoreTextInitialScale = scoreText.transform.localScale;
        popupTextInitialScale = popupText.transform.localScale;

        levelCompleteScreen.SetActive(false);
        achievementPanel.SetActive(false);
    }

    private void OnEnable()
    {
        ScoreManager.Instance.OnScoreUpdate += UpdateScore;
        ScoreManager.Instance.OnTimeElapsedUpdate += UpdateTimeElapsed;
        PlayerController.Instance.OnFormChange += ShowFormPopup;
        PlayerController.Instance.OnTimeoutTimerUpdate += UpdateTimeoutSlider;
        PlayerController.Instance.OnDeath += DisableTimeoutSlider;
    }

    private void OnDisable()
    {
        //Unsubscribe from all events
        ScoreManager.Instance.OnScoreUpdate -= UpdateScore;
        ScoreManager.Instance.OnTimeElapsedUpdate -= UpdateTimeElapsed;
        PlayerController.Instance.OnFormChange -= ShowFormPopup;
        PlayerController.Instance.OnTimeoutTimerUpdate -= UpdateTimeoutSlider;
        PlayerController.Instance.OnDeath -= DisableTimeoutSlider;

    }

    void DisableTimeoutSlider()
    {
        timeoutSliderEnabled = false;
    }

    void UpdateScore(int newScore)
    {
        if (scoreTextAnimationRoutine != null) StopCoroutine(scoreTextAnimationRoutine);

        scoreTextAnimationRoutine = StartCoroutine(TextAnimation(scoreText, scoreAnimationScaleCurve, scoreAnimationAlphaCurve, 
            scoreTextInitialScale, scoreAnimationDuration, scoreAnimationScaleMultiplier, false));

        scoreText.text = newScore.ToString();
    }

    void UpdateTimeElapsed(float timeElapsed)
    {
        timeElapsedText.text = TimeUtilities.GetMinuteSecondRepresentation(Mathf.Floor(timeElapsed));
    }

    void UpdateTimeoutSlider(float amount)
    {
        if (amount >= timeoutSliderThreshold && timeoutSliderEnabled)
        {
            var fillAmount = 1 - ((amount - timeoutSliderThreshold) / (1 - timeoutSliderThreshold));
            timeoutSlider.value = fillAmount;
        }
        else
        {
            timeoutSlider.value = 0;
        }
    }

    void ShowFormPopup(WispFormType oldForm, WispFormType newForm)
    {
        ShowPopup(newForm.ToString().ToUpper());
    }

    public void ShowPopup(string text)
    {
        popupText.text = text;

        if (popupTextAnimationRoutine != null) StopCoroutine(popupTextAnimationRoutine);

        popupTextAnimationRoutine = StartCoroutine(TextAnimation(popupText, popupAnimationScaleCurve, popupAnimationAlphaCurve,
            popupTextInitialScale, popupAnimationDuration, popupAnimationScaleMultiplier, true));
    }

    public void ShowAchievementPopup(string text)
    {
        achievementTitleText.text = text;
        achievementPanel.SetActive(true);
        RefreshFontSize(new() { achievementTitleText, achievementLabel });
    }

    public void ShowLevelCompleteStats(int score, float time)
    {
        scoreText.enabled = false;
        timeElapsedText.enabled = false;
        levelCompleteScreen.SetActive(true);
        levelCompleteScoreText.text = score.ToString();
        levelCompleteTimeText.text = TimeUtilities.GetMinuteSecondRepresentation(Mathf.Floor(time));
        levelCompleteScoreStars.SetStarAmount<int>(SceneData.levelToLoad.scoreStarInfo.starThresholds, score, true);
        levelCompleteTimeStars.SetStarAmount<float>(SceneData.levelToLoad.timeSecondsStarInfo.starThresholds, time, false, true);
        RefreshFontSize(new() { levelCompleteScoreText, levelCompleteTimeText });
        levelCompleteReturnToMenuButton.onClick.AddListener(delegate { SceneManager.LoadScene("MainMenu"); });
    }

    private IEnumerator TextAnimation(TextMeshProUGUI text, AnimationCurve scaleCurve, AnimationCurve alphaCurve, 
        Vector3 initialScale, float duration, float scaleMultiplier, bool disableAfter)
    {
        text.enabled = true;

        var time = 0f;
        while(time < duration)
        {
            time += Time.unscaledDeltaTime;
            var animationTime = time / duration;
            text.transform.localScale = initialScale * (1 + (scaleMultiplier * scaleCurve.Evaluate(animationTime)));
            text.alpha = alphaCurve.Evaluate(animationTime);
            yield return null;
        }

        if (disableAfter) text.enabled = false;
    }
}
