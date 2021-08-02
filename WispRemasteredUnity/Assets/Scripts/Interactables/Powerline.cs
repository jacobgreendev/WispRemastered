using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerline : MonoBehaviour
{
    [SerializeField] private Transform endTransform;
    [SerializeField] private LineRenderer lineRenderer;

    public Transform EndTransform
    {
        get => endTransform;
    }
}
