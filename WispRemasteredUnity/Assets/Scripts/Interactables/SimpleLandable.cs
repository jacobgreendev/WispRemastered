using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLandable : Interactable
{
    [SerializeField] Transform landPosition;

    public override void DoInteraction(PlayerController player)
    {
        player.Land(this, landPosition);
        DisableColliders();
    }
}
