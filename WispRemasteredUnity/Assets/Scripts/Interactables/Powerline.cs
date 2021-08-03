using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerline : Interactable
{
    [SerializeField] private Transform startTransform, endTransform;
    [SerializeField] private float landLerpTimePerUnitDistance, travelTimePerUnitDistance;
    [SerializeField] private LineRenderer lineRenderer;

    public Transform EndTransform
    {
        get => endTransform;
    }

    private void Start() 
    {
        lineRenderer.SetPosition(0, startTransform.position);
        lineRenderer.SetPosition(1, endTransform.position);
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
        var powerlineStartPosition = startTransform.position;

        //Disable wisp physics and enable interacting state
        player.Rigidbody.isKinematic = true;
        player.IsInteracting = true;

        //Lerp the wisp to the start of the powerline, then reset the journey to avoid birdseye view
        yield return StartCoroutine(player.LerpToPosition(playerTransform, landLerpTimePerUnitDistance * Vector3.Distance(initialPlayerPosition, powerlineStartPosition), initialPlayerPosition, powerlineStartPosition));
        player.ResetJourney();

        //Update velocity and facing direction to face along the powerline
        var endPosition = endTransform.position;
        player.Rigidbody.velocity = (endPosition - powerlineStartPosition).normalized * currentSpeed;
        player.UpdateBodyFacingDirection();

        //Lerp the wisp along the powerline, make it land at the end and disable interacting state
        yield return StartCoroutine(player.LerpToPosition(playerTransform, travelTimePerUnitDistance * Vector3.Distance(powerlineStartPosition, endTransform.position), powerlineStartPosition, endTransform.position));
        player.Land(this, endTransform);
        player.IsInteracting = false;
    }
}
