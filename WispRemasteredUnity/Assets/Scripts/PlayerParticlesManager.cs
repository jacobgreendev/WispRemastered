using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticlesManager : MonoBehaviour
{
    public static PlayerParticlesManager Instance;

    private WispFormParticleDetails currentFormDetails;

    // Start is called before the first frame update
    private void Start()
    {
        currentFormDetails = WispFormManager.Instance.GetParticleDetails(PlayerController.Instance.CurrentForm);
        PlayerController.Instance.OnFormChange += ChangeActiveSystem;
        PlayerController.Instance.OnLand += PlayLandEffect;
        PlayerController.Instance.OnFire += PlayFireEffect;
    }

    private void ChangeActiveSystem(WispFormType oldForm, WispFormType newForm)
    {
        WispFormParticleDetails newFormDetails = WispFormManager.Instance.GetParticleDetails(newForm);

        if(newFormDetails != null)
        {
            WispFormManager.Instance.GetParticleDetails(oldForm)?.particleSystem.SetActive(false);
            newFormDetails.particleSystem.SetActive(true);
            currentFormDetails = newFormDetails;
        }        
    }

    private void PlayLandEffect(Transform landedOn)
    {
        var effectTransform = currentFormDetails.particleSystem.transform;
        var oldPos = effectTransform.position;
        effectTransform.position = landedOn.position;
        currentFormDetails.landEffect.Play();
        effectTransform.position = oldPos;
    }

    private void PlayFireEffect()
    {
        currentFormDetails.landEffect.Play();
    }
}

[Serializable]
public class WispFormParticleDetails
{
    public GameObject particleSystem;
    public ParticleSystem landEffect;
}