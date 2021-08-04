using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Score Text Fields")]
    [SerializeField] private AnimationCurve scoreAnimationScaleCurve, scoreAnimationAlphaCurve;
    [SerializeField] private float scoreAnimationLength, scoreAnimationScaleMultiplier;

    private Coroutine scoreTextAnimationRoutine;
    private Vector3 scoreTextInitialScale;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.enabled = false;
        ScoreManager.Instance.OnScoreUpdate += UpdateScore;
        scoreTextInitialScale = scoreText.transform.localScale;
    }
    void UpdateScore(int newScore)
    {
        if (scoreTextAnimationRoutine != null) StopCoroutine(scoreTextAnimationRoutine);
        scoreTextAnimationRoutine = StartCoroutine(ScoreTextAnimation());
        scoreText.text = newScore.ToString();
    }

    private IEnumerator ScoreTextAnimation()
    {
        scoreText.enabled = true;

        var time = 0f;
        while(time < scoreAnimationLength)
        {
            time += Time.unscaledDeltaTime;
            var animationTime = time / scoreAnimationLength;
            scoreText.transform.localScale = scoreTextInitialScale * (1 + (scoreAnimationScaleMultiplier * scoreAnimationScaleCurve.Evaluate(animationTime)));
            scoreText.alpha = scoreAnimationAlphaCurve.Evaluate(animationTime);
            yield return null;
        }

        scoreText.enabled = false;
    }
}
