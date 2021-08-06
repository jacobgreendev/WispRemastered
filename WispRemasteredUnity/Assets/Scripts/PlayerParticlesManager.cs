using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticlesManager : MonoBehaviour
{
    public static PlayerParticlesManager Instance;

    private WispFormParticleDetails currentFormDetails;

    private void OnEnable()
    {
        PlayerController.Instance.OnFormChange += ChangeActiveSystem;
        PlayerController.Instance.OnLand += PlayLandEffect;
        PlayerController.Instance.OnFire += PlayFireEffect;
        PlayerController.Instance.OnDeath += DeathParticles;
    }

    // Start is called before the first frame update
    private void Start()
    {
        //currentFormDetails = WispFormManager.Instance.GetParticleDetails(PlayerController.Instance.CurrentForm);
    }

    private void OnDisable()
    {
        //Unsubscribe from all events
        PlayerController.Instance.OnFormChange -= ChangeActiveSystem;
        PlayerController.Instance.OnLand -= PlayLandEffect;
        PlayerController.Instance.OnFire -= PlayFireEffect;
        PlayerController.Instance.OnDeath -= DeathParticles;
    }

    private void DeathParticles()
    {
        var emission = currentFormDetails.particleSystem.GetComponent<ParticleSystem>().emission;
        emission.rateOverTime = 0;

        currentFormDetails.landEffect.Play();
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

    private void PlayLandEffect(Interactable landedOn)
    {
        //Effect transform is temporarily moved to the landing position for more seamless effect
        var effectTransform = currentFormDetails.particleSystem.transform;
        var oldPos = effectTransform.position;
        effectTransform.position = landedOn.transform.position;
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