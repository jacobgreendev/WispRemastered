using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocal : MonoBehaviour
{
    public static Transform TransformInstance;
    // Start is called before the first frame update
    void Awake()
    {
        TransformInstance = transform;
    }
}