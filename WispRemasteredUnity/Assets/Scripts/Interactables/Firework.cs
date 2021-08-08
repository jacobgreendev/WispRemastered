using System.Collections;
using UnityEngine;

public class Firework : Interactable
{
    [SerializeField] private float landLerpTime, distance, fuseTime, travelTime, explosionForce;
    [SerializeField] private Transform fuseStart, fuseEnd;

    [Header("Slow-motion")]
    [SerializeField] private float slowMoTime = 0.5f;
    [SerializeField] private float targetSlowTimeScale = 0.3f;

    private PlayerController player;

    public override void DoInteraction(PlayerController player)
    {
        this.player = player;

        StartCoroutine(RideFirework());
    }

    private IEnumerator RideFirework()
    {
        //Reset last landed height of player and disable birdseye view
        player.ResetJourney(); 
        CameraController.Instance.DisableBirdseye();

        //Disable wisp physics and enable interacting state
        var playerTransform = player.transform;
        player.Rigidbody.isKinematic = true;
        player.IsInteracting = true;

        //Lerp to the start of the fuse
        yield return StartCoroutine(player.LerpToPosition(playerTransform, landLerpTime, transform.position, fuseStart.position));

        //Update velocity and facing direction so wisp is facing in the direction of movement
        player.Rigidbody.velocity = (fuseEnd.position - fuseStart.position).normalized; 
        player.UpdateBodyFacingDirection();

        //Lerp from the start of the fuse to the end of the fuse
        yield return StartCoroutine(player.LerpToPosition(playerTransform, fuseTime, fuseStart.position, fuseEnd.position));

        //Calculate the final position of the firework
        var endPosition = transform.position + transform.up * distance;

        //Parent the wisp to the firework, smooth lerp the firework to the end position, and unparent
        player.transform.parent = transform;
        yield return StartCoroutine(player.SmoothLerpToPosition(transform, travelTime, transform.position, endPosition));
        player.transform.parent = null;

        //Re-enable wisp physics, "land" in place (i.e enable controls), and give the wisp some velocity
        player.Rigidbody.isKinematic = false;
        player.Land(this, transform, inPlace: true);
        player.Rigidbody.velocity = transform.up * explosionForce;
        ScoreManager.Instance.AddScoreWithMessage(1, "BOOM!");

        //Disable interacting state and smoothly slow time until the player fires again
        player.IsInteracting = false;       
        TimeUtilities.Instance.LerpTime(targetSlowTimeScale, slowMoTime);
        PlayerController.Instance.OnFire += OnPlayerFired;
    }

    void OnPlayerFired()
    {
        PlayerController.Instance.OnFire -= OnPlayerFired;

        TimeUtilities.Instance.SetTimescale(1);
        Destroy(gameObject);
    }
}
