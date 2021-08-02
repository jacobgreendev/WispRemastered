using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    public static ParticlesManager Instance;

    [SerializeField] private List<WispFormParticleDetails> formDetails;
    private Dictionary<WispForm, WispFormParticleDetails> formDetailDict = new();
    private WispFormParticleDetails currentFormDetails;

    private void Awake()
    {
        foreach(var pair in formDetails)
        {
            formDetailDict.Add(pair.form, pair);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        currentFormDetails = formDetailDict[PlayerController.Instance.CurrentForm];
        PlayerController.Instance.OnFormChange += ChangeActiveSystem;
        PlayerController.Instance.OnLand += PlayLandEffect;
        PlayerController.Instance.OnFire += PlayFireEffect;
    }

    private void ChangeActiveSystem(WispForm oldForm, WispForm newForm)
    {
        if (formDetailDict.TryGetValue(oldForm, out var oldFormDetails))
        {
            oldFormDetails.particleSystem.SetActive(false);
        }

        if (formDetailDict.TryGetValue(newForm, out var newFormDetails))
        {
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
    public WispForm form;
    public GameObject particleSystem;
    public ParticleSystem landEffect;
}