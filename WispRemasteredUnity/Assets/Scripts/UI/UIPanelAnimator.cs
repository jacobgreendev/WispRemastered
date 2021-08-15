using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelAnimator : MonoBehaviour
{
    [SerializeField] private float animationLength = 0.3f;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private bool reverseAfter = false;
    [SerializeField] private float waitTimeBeforeReverse;
    [SerializeField] private bool deactivateAfter = false;

    private Vector3 initialScale;
    private Coroutine currentAnimation;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    void OnEnable()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }
        currentAnimation = StartCoroutine(EnableAnimation());
    }

    private IEnumerator EnableAnimation()
    {
        float time = 0f;
        while (time < animationLength)
        {
            time += Time.deltaTime;
            transform.localScale = initialScale * animationCurve.Evaluate(time / animationLength);
            yield return null;
        }
        transform.localScale = initialScale;

        yield return new WaitForSeconds(waitTimeBeforeReverse);

        if (reverseAfter)
        {
            time = 0f;
            while (time < animationLength)
            {
                time += Time.deltaTime;
                transform.localScale = initialScale * animationCurve.Evaluate( 1 - (time / animationLength));
                yield return null;
            }
            transform.localScale = initialScale * animationCurve.Evaluate(0);
        }

        if (deactivateAfter)
        {
            gameObject.SetActive(false);
        }
    }
}
