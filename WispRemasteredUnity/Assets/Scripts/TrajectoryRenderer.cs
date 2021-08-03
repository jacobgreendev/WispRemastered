using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    public static TrajectoryRenderer Instance;

    [SerializeField] private LineRenderer trajectoryLine;
    [SerializeField] private int segmentCount;
    [SerializeField] private float segmentScale;
    private float fixedDeltaTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
    }

    public void SetVisible(bool visible)
    {
        trajectoryLine.enabled = visible;
    }

    public void DisplayPath(Vector3 startPoint, Vector3 force, float mass, float drag, float segmentMultiplier)
    {
        if (!trajectoryLine.enabled) trajectoryLine.enabled = true;

        int newSegCount = (int)(segmentCount * segmentMultiplier);
        if (newSegCount > 0)
        {
            Vector3[] segments = new Vector3[newSegCount];

            segments[0] = startPoint;

            Vector3 segVelocity = (force / mass) * fixedDeltaTime;

            for (int i = 1; i < newSegCount; i++)
            {
                float segTime = (segVelocity.sqrMagnitude != 0) ? segmentScale / segVelocity.magnitude : 0;

                segVelocity += Physics.gravity * segTime;
                segVelocity *= (1.0f - drag * segTime);

                segments[i] = segments[i - 1] + segVelocity * segTime;
            }

            trajectoryLine.positionCount = newSegCount;
            for (int i = 0; i < newSegCount; i++)
            {
                trajectoryLine.SetPosition(i, segments[i]);
            }
        }
    }
}
