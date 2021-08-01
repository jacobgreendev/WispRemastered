using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public event PositionUpdatedEventHandler DragPositionUpdated;

    private bool inFlight = false;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inFlight)
        {
            var touchPos = Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector2) Input.mousePosition;
            DragPositionUpdated(touchPos);
        }
    }

}

public delegate void PositionUpdatedEventHandler(Vector2 newPosition);
