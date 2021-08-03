using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : Interactable
{
    [SerializeField] private float landLerpTime, distance, fuseTime, travelTime, explosionForce;
    [SerializeField] private Transform parentTransform, fuseStart, fuseEnd;

    public override void DoInteraction(PlayerController player)
    {
        StartCoroutine(RideFirework(player));
    }

    private IEnumerator RideFirework(PlayerController player)
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
        Destroy(parentTransform.gameObject);
    }


}
