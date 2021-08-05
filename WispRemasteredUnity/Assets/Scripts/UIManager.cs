using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI popupText;

    [Header("Score Text Fields")]
    [SerializeField] private AnimationCurve scoreAnimationScaleCurve, scoreAnimationAlphaCurve;
    [SerializeField] private float scoreAnimationDuration, scoreAnimationScaleMultiplier;

    [Header("Popup Text Fields")]
    [SerializeField] private AnimationCurve popupAnimationScaleCurve, popupAnimationAlphaCurve;
    [SerializeField] private float popupAnimationDuration, popupAnimationScaleMultiplier;

    private Coroutine scoreTextAnimationRoutine, popupTextAnimationRoutine;
    private Vector3 scoreTextInitialScale, popupTextInitialScale;

    private void Awake()
    {
        scoreTextInitialScale = scoreText.transform.localScale;
        popupTextInitialScale = popupText.transform.localScale;
        popupText.enabled = false;
    }

    private void OnEnable()
    {
        ScoreManager.Instance.OnScoreUpdate += UpdateScore;
        PlayerController.Instance.OnFormChange += ShowFormPopup;
    }

    private void OnDisable()
    {
        //Unsubscribe from all events
        ScoreManager.Instance.OnScoreUpdate -= UpdateScore;
        PlayerController.Instance.OnFormChange -= ShowFormPopup;
    }

    void UpdateScore(int newScore)
    {
        if (scoreTextAnimationRoutine != null) StopCoroutine(scoreTextAnimationRoutine);

        scoreTextAnimationRoutine = StartCoroutine(TextAnimation(scoreText, scoreAnimationScaleCurve, scoreAnimationAlphaCurve, 
            scoreTextInitialScale, scoreAnimationDuration, scoreAnimationScaleMultiplier, false));

        scoreText.text = newScore.ToString();
    }

    void ShowFormPopup(WispFormType oldForm, WispFormType newForm)
    {
        ShowPopup(newForm.ToString().ToUpper() + "!");
    }

    void ShowPopup(string text)
    {
        popupText.text = text;

        if (popupTextAnimationRoutine != null) StopCoroutine(popupTextAnimationRoutine);

        popupTextAnimationRoutine = StartCoroutine(TextAnimation(popupText, popupAnimationScaleCurve, popupAnimationAlphaCurve,
            popupTextInitialScale, popupAnimationDuration, popupAnimationScaleMultiplier, true));
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
