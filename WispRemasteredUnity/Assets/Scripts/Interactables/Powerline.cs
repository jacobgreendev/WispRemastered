using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerline : Interactable
{
    [SerializeField] private Transform endTransform;

    public Transform EndTransform
    {
        get => endTransform;
    }
}
