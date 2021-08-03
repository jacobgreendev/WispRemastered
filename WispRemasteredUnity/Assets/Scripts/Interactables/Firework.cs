using System.Collections;
using UnityEngine;

public class Firework : Interactable
{
    [SerializeField] private float landLerpTime, distance, fuseTime, travelTime, explosionForce;
    [SerializeField] private Transform parentTransform, fuseStart, fuseEnd;

    [Header("Slow-motion")]
    [SerializeField] private float slowMoTime = 0.5f, targetTimeScale = 0.3f;

    private PlayerController player;

    public override void DoInteraction(PlayerController player)
    {
        this.player = player;

        StartCoroutine(RideFirework());
    }

    private IEnumerator RideFirework()
    {
        player.ResetJourney();
        CameraController.Instance.DisableBirdseye();

        var playerTransform = player.transform;
        player.Rigidbody.isKinematic = true;
        player.IsInteracting = true;

        yield return StartCoroutine(player.LerpToPosition(playerTransform, landLerpTime, transform.position, fuseStart.position));

        player.Rigidbody.velocity = (fuseEnd.position - fuseStart.position).normalized;
        player.UpdateBodyFacingDirection();

        yield return StartCoroutine(player.LerpToPosition(playerTransform, fuseTime, fuseStart.position, fuseEnd.position));

        var endPosition = parentTransform.position + parentTransform.up * distance;

        player.transform.parent = parentTransform;
        yield return StartCoroutine(player.SmoothLerpToPosition(parentTransform, travelTime, parentTransform.position, endPosition));
        player.transform.parent = null;

        player.Rigidbody.isKinematic = false;
        player.Land(transform, inPlace: true);
        player.Rigidbody.velocity = parentTransform.up * explosionForce;
        player.IsInteracting = false;       

        //Slow Down time
        yield return StartCoroutine(SmoothSlowDownTime());
        PlayerController.Instance.OnFire += OnPlayerFired;
    }


    void OnPlayerFired()
    {
        PlayerController.Instance.OnFire -= OnPlayerFired;

        Time.timeScale = 1;
        Destroy(parentTransform.gameObject);
    }

    IEnumerator SmoothSlowDownTime()
    {
        float time = 0f;
        float initialTimeScale = Time.timeScale;

        while(time < landLerpTime)
        {
            time += Time.deltaTime;
            Time.timeScale = Mathf.Lerp(initialTimeScale, targetTimeScale, time / slowMoTime);
            yield return null;
        }

        Time.timeScale = targetTimeScale;
    }
}
