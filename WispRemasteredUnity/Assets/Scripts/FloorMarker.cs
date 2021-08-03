using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FloorMarker : MonoBehaviour
{
    [SerializeField] private Transform marker;
    private LineRenderer lineRenderer;
    [SerializeField] private float minSize, maxSize, maxDistance;

    [Header("Fading In/Out")]
    [SerializeField] private float fadeTime;
    private float maxAlpha;
    private Coroutine currentFadeRoutine;
    private Material markerMaterial;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        PlayerController.Instance.OnLand += FadeOutFloorMarker;
        PlayerController.Instance.OnFire += FadeInFloorMarker;

        markerMaterial = marker.GetComponent<Renderer>().material;
        maxAlpha = markerMaterial.GetFloat("_Alpha");
    }

    private void Update()
    {
        var playerPosition = PlayerController.Instance.transform.position;

        Ray ray = new Ray(playerPosition, Vector3.down);

        var distance = Mathf.Infinity;

        if (Physics.Raycast(ray, out var hit, maxDistance, 1 << LayerMask.NameToLayer(GameConstants.Layer_Ground)))
        {
            distance = Vector3.Distance(hit.point, transform.position);
            UpdateLineRendererPositions(new Vector3[] { playerPosition, hit.point });
        }

        UpdateFloorMarker(hit.point, distance);
    }

    private void UpdateFloorMarker(Vector3 position, float distance)
    {
        marker.position = position;
        var currentSize = marker.localScale;

        if(distance <= currentSize.z)
        {
            var newSize = Mathf.Lerp(minSize, maxSize, distance / maxDistance);
            currentSize.x = newSize;
            currentSize.y = newSize;
        }
        else
        {
            currentSize.x = minSize;
            currentSize.y = minSize;
        }

        marker.localScale = currentSize;
    }

    private void UpdateLineRendererPositions(Vector3[] positions)
    {
        lineRenderer.SetPositions(positions);
    }

    private void FadeOutFloorMarker(Interactable landedOn)
    {
        if (currentFadeRoutine != null)
        {
            StopCoroutine(currentFadeRoutine);
        }
        currentFadeRoutine = StartCoroutine(FadeFloorMarker(markerMaterial.GetFloat("_Alpha"), 0, fadeTime));
    }

    private void FadeInFloorMarker()
    {
        if(currentFadeRoutine != null)
        {
            StopCoroutine(currentFadeRoutine);
        }
        currentFadeRoutine = StartCoroutine(FadeFloorMarker(markerMaterial.GetFloat("_Alpha"), maxAlpha, fadeTime));
    }

    private IEnumerator FadeFloorMarker(float initialAlpha, float targetAlpha, float fadeTime)
    {
        float time = 0;
        float newAlpha;
        while (time <= fadeTime)
        {
            time += Time.deltaTime;
            newAlpha = Mathf.Lerp(initialAlpha, targetAlpha, Mathf.Clamp01(time / fadeTime));
            markerMaterial.SetFloat("_Alpha", newAlpha);
            yield return null;
        }
    }
}
