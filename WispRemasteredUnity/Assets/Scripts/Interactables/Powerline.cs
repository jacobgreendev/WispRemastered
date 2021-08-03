using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerline : Interactable
{
    [SerializeField] private Transform endTransform;
    [SerializeField] private float landLerpTime, travelTime;

    public Transform EndTransform
    {
        get => endTransform;
    }

    public override void DoInteraction(PlayerController player)
    {
        StartCoroutine(RidePowerline(player));
    }
    private IEnumerator RidePowerline(PlayerController player)
    {
        var currentSpeed = player.Rigidbody.velocity.magnitude;
        var playerTransform = player.transform;
        var initialPlayerPosition = playerTransform.position;
        var powerlineStartPosition = transform.position;
        player.Rigidbody.isKinematic = true;
        player.IsInteracting = true;

        yield return StartCoroutine(player.LerpToPosition(playerTransform, landLerpTime, initialPlayerPosition, powerlineStartPosition));

        player.ResetJourney();

        var endPosition = endTransform.position;
        player.Rigidbody.velocity = (endPosition - powerlineStartPosition).normalized * currentSpeed;
        player.UpdateBodyFacingDirection();

        yield return StartCoroutine(player.LerpToPosition(playerTransform, travelTime, powerlineStartPosition, endTransform.position));

        player.Land(endTransform);
        player.IsInteracting = false;
    }
}
