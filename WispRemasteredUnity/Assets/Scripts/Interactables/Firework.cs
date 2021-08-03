using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : Interactable
{
    [SerializeField] private float distance, fuseTime, travelTime, explosionForce;
    [SerializeField] private Transform parentTransform, fuseStart, fuseEnd;

    public float Distance { get => distance; }

    public float FuseTime { get => fuseTime; }

    public float TravelTime { get => travelTime; }

    public float ExplosionForce { get => explosionForce; }

    public Transform ParentTransform { get => parentTransform; }

    public Transform FuseStart { get => fuseStart; }

    public Transform FuseEnd { get => fuseEnd; }
}
