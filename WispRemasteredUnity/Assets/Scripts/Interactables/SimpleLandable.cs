using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLandable : Interactable
{
    [SerializeField] Transform landPosition;

    public override void DoInteraction(PlayerController player)
    {
        PlayerController.Instance.OnLand += OnPlayerLand;
        player.Land(this, landPosition);  
    }

    void OnPlayerLand(Interactable interactable)
    {
        PlayerController.Instance.OnLand -= OnPlayerLand;
        Destroy(this.gameObject);
    }
}
