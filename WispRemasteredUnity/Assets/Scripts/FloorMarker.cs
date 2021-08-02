using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FloorMarker : MonoBehaviour
{
    private DecalProjector projector;
    [SerializeField] private float minSize, maxSize;

    [Header("Fading In/Out")]
    [SerializeField] private float fadeTime;
    private Coroutine currentFadeRoutine;
    private Material decalMaterial;

    // Start is called before the first frame update
    void Start()
    {
        projector = GetComponent<DecalProjector>();
        PlayerController.Instance.PlayerPositionUpdated += UpdateFloorMarkerSize;
        PlayerController.Instance.OnLand += FadeOutFloorMarker;
        PlayerController.Instance.OnFire += FadeInFloorMarker;
        UpdateFloorMarkerSize(PlayerController.Instance.transform.position);

        decalMaterial = new Material(projector.material);
        projector.material = decalMaterial; //Changing projector.material will change the asset, so make a copy instead
    }

    // Update is called once per frame
    private void UpdateFloorMarkerSize(Vector3 position)
    {
        var currentSize = projector.size;
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out var hit, currentSize.z))
        {
            var distance = Vector3.Distance(hit.point, transform.position);
            var newSize = Mathf.Lerp(minSize, maxSize, distance / currentSize.z);
            currentSize.x = newSize;
            currentSize.y = newSize;
        }
        else
        {
            currentSize.x = minSize;
            currentSize.y = minSize;
        }

        projector.size = currentSize;
    }

    private void FadeOutFloorMarker(Transform landedOn)
    {
        if (currentFadeRoutine != null)
        {
            StopCoroutine(currentFadeRoutine);
        }
        currentFadeRoutine = StartCoroutine(FadeFloorMarker(decalMaterial.GetFloat("_Alpha"), 0, fadeTime));
    }

    private void FadeInFloorMarker()
    {
        if(currentFadeRoutine != null)
        {
            StopCoroutine(currentFadeRoutine);
        }
        currentFadeRoutine = StartCoroutine(FadeFloorMarker(decalMaterial.GetFloat("_Alpha"), 1, fadeTime));
    }

    private IEnumerator FadeFloorMarker(float initialAlpha, float targetAlpha, float fadeTime)
    {
        float time = 0;
        float newAlpha;
        while (time < fadeTime)
        {
            newAlpha = Mathf.Lerp(initialAlpha, targetAlpha, time / fadeTime);
            decalMaterial.SetFloat("_Alpha", newAlpha);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
