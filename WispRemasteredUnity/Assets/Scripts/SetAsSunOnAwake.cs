using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAsSunOnAwake : MonoBehaviour
{
    private void Awake()
    {
        RenderSettings.sun = GetComponent<Light>();
    }
}
