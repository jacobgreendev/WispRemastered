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
        //Get some initial values
        var currentSpeed = player.Rigidbody.velocity.magnitude;
        var playerTransform = player.transform;
        var initialPlayerPosition = playerTransform.position;
        var powerlineStartPosition = transform.position;

        //Disable wisp physics and enable interacting state
        player.Rigidbody.isKinematic = true;
        player.IsInteracting = true;

        //Lerp the wisp to the start of the powerline, then reset the journey to avoid birdseye view
        yield return StartCoroutine(player.LerpToPosition(playerTransform, landLerpTime, initialPlayerPosition, powerlineStartPosition));
        player.ResetJourney();

        //Update velocity and facing direction to face along the powerline
        var endPosition = endTransform.position;
        player.Rigidbody.velocity = (endPosition - powerlineStartPosition).normalized * currentSpeed;
        player.UpdateBodyFacingDirection();

        //Lerp the wisp along the powerline, make it land at the end and disable interacting state
        yield return StartCoroutine(player.LerpToPosition(playerTransform, travelTime, powerlineStartPosition, endTransform.position));
        player.Land(endTransform);
        player.IsInteracting = false;
    }
}
