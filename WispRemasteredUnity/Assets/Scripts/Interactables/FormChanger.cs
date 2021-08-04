using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormChanger : SimpleLandable
{
    [SerializeField] private WispFormType formChange;

    public override void DoInteraction(PlayerController player)
    {
        base.DoInteraction(player);
        player.ChangeForm(formChange);
    }
}
