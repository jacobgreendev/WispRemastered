using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelAnimator : MonoBehaviour
{
    [SerializeField] private float animationLength = 0.3f;
    [SerializeField] private AnimationCurve animationCurve;
    //[SerializeField] private float rotationAmount = 90f;

    private Vector3 initialScale;
    //private Quaternion initialRotation, rotatedRotation;
    private Coroutine currentAnimation;

    private void Awake()
    {
        initialScale = transform.localScale;
        //initialRotation = transform.localRotation;
        //var initialEulers = initialRotation.eulerAngles;
        //rotatedRotation = Quaternion.Euler(initialEulers.x, initialEulers.y, initialEulers.z + rotationAmount);
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
            //transform.localRotation = Quaternion.Lerp(rotatedRotation, initialRotation, animationCurve.Evaluate(time / animationLength));
            yield return null;
        }
        transform.localScale = initialScale;
        //transform.localRotation = initialRotation;
    }
}
