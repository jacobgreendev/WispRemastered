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
    private Material markerMaterial, lineMaterial;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        markerMaterial = marker.GetComponent<Renderer>().material;
        lineMaterial = lineRenderer.material;
        maxAlpha = markerMaterial.GetFloat("_Alpha");
    }

    private void OnEnable()
    {
        PlayerController.Instance.OnLand += LandFade;
        PlayerController.Instance.OnFire += FadeInFloorMarker;
        PlayerController.Instance.OnDeath += DeathFade;
    }

    private void OnDisable()
    {
        //Unsubscribe from all events
        PlayerController.Instance.OnLand -= LandFade;
        PlayerController.Instance.OnFire -= FadeInFloorMarker;
        PlayerController.Instance.OnDeath -= DeathFade;
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

    private void LandFade(Interactable landedOn)
    {
        FadeOutFloorMarker();
    }

    private void DeathFade()
    {
        FadeOutFloorMarker();
    }

    private void FadeOutFloorMarker()
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
            lineMaterial.SetFloat("_Alpha", newAlpha);
            yield return null;
        }
    }
}
