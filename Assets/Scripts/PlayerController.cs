using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public event PositionUpdatedEventHandler DragPositionUpdated;
    public event PositionUpdatedEventHandler PlayerPositionUpdated;
    public event InputDetectedEventHandler TouchDetectedWhileNotInFlight;

    private bool inFlight = false;
    private bool wasTouchingLastFrame = false;
    private Vector2 touchPosition;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private float forwardForce, sidewaysForcePerUnitDragLength;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        playerRigidbody.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inFlight)
        {
            DoInput();
        }
        else
        {
            PlayerPositionUpdated(transform.position);
        }

        //Debug
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Land();
        }
    }

    private void DoInput()
    {
        var isTouching = Input.touchCount > 0 || Input.GetMouseButton(0);
        if (isTouching)
        {
            TouchDetectedWhileNotInFlight(true);
            touchPosition = Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;
            DragPositionUpdated(touchPosition);
            wasTouchingLastFrame = true;
        }
        else
        {
            TouchDetectedWhileNotInFlight(false);
            if (wasTouchingLastFrame)
            {
                wasTouchingLastFrame = false;
                Fire();
            }
        }
    }

    private void Fire()
    {
        playerRigidbody.isKinematic = false;
        inFlight = true;
        playerRigidbody.AddForce((forwardForce * transform.forward) + (sidewaysForcePerUnitDragLength * (Vector3) UIManager.Instance.DragLength));
    }

    private void Land()
    {
        playerRigidbody.isKinematic = true;
        inFlight = false;
    }

}

public delegate void PositionUpdatedEventHandler(Vector3 newPosition);
public delegate void InputDetectedEventHandler(bool detected);
