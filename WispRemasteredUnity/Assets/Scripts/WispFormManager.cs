using System;
using System.Collections.Generic;
using UnityEngine;

public class WispFormManager : MonoBehaviour
{
    public static WispFormManager Instance;

    [SerializeField] List<WispFormDetails> wispFormList;
    private Dictionary<WispFormType, WispFormDetails> wispForms = new Dictionary<WispFormType, WispFormDetails>();

    private void Awake()
    {
        Instance = this;

        foreach (WispFormDetails wispFormDetails in wispFormList)
        {
            wispForms.Add(wispFormDetails.wispForm.wispFormType, wispFormDetails);
        }
    }

    public WispForm GetWispForm(WispFormType wispFormType)
    {
        if(wispForms.TryGetValue(wispFormType, out WispFormDetails wispForm))
        {
            return wispForm.wispForm;
        }

        return null;
    }

    public WispFormParticleDetails GetParticleDetails(WispFormType wispFormType)
    {
        wispForms.TryGetValue(wispFormType, out WispFormDetails wispForm);

        WispFormParticleDetails wispParticleDetails = wispForm?.wispParticleDetails;
        return wispParticleDetails;
    }

    //Other Helper functions go here
}

[Serializable]
public class WispFormDetails
{
    public WispFormType wispFormType;
    public WispForm wispForm;
    public WispFormParticleDetails wispParticleDetails; //These could have been included in the WispForm, however it references objects in the scene.
}
