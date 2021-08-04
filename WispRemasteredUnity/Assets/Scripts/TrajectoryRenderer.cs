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

        PlayerController.Instance.OnFire += () => SetVisible(false); 
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
            List<Vector3> segments = new List<Vector3>();

            segments.Add(startPoint);

            Vector3 segVelocity = (force / mass) * fixedDeltaTime;
            var last = false;
            for (int i = 1; i < newSegCount; i++)
            {
                if (last) break;
                float segTime = (segVelocity.sqrMagnitude != 0) ? segmentScale / segVelocity.magnitude : 0;

                segVelocity += Physics.gravity * segTime;
                segVelocity *= (1.0f - drag * segTime);

                if (Physics.Raycast(segments[i - 1], segVelocity.normalized, segVelocity.magnitude * segTime)) last = true;

                segments.Add(segments[i-1] + segVelocity * segTime);
            }

            trajectoryLine.positionCount = segments.Count;
            for (int i = 0; i < segments.Count; i++)
            {
                trajectoryLine.SetPosition(i, segments[i]);
            }
        }
    }
}
