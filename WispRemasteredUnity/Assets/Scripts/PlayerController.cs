using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public event PositionUpdatedEventHandler DragPositionUpdated;
    public event PositionUpdatedEventHandler PlayerPositionUpdated;
    public event InputDetectedEventHandler TouchDetectedWhileNotInFlight;
    public event OnLandEventHandler OnLand;
    public event OnFireEventHandler OnFire;
    public event VelocityUpdatedEventHandler VelocityUpdated;

    private bool inFlight = false;
    private bool wasTouchingLastFrame = false;
    private Vector2 touchPosition;

    [Header("Physics")]
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private float forwardForce, maxSidewaysForce;

    private Transform currentlyLandedOn;

    [Header("Lerp Times")]
    [SerializeField] private float landLerpTime;

    [Header("Visuals")]
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private float bodyDirectionLerpSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;
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
            VelocityUpdated(playerRigidbody.velocity);
            UpdateBodyFacingDirection();
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

    private void UpdateBodyFacingDirection()
    {
        var targetRotation = inFlight && playerRigidbody.velocity.magnitude > 0 ? Quaternion.LookRotation(playerRigidbody.velocity) : Quaternion.LookRotation(Vector3.down);
        bodyTransform.rotation = Quaternion.Lerp(bodyTransform.rotation, targetRotation, bodyDirectionLerpSpeed * Time.deltaTime);
    }

    private void Fire()
    {
        OnFire();
        playerRigidbody.isKinematic = false;
        inFlight = true;
        var sidewaysDirection = (Vector3) UIManager.Instance.DragVector.normalized;
        var sidewaysPower = maxSidewaysForce * (UIManager.Instance.DragVector.magnitude / UIManager.Instance.MaxDragDistance); //Max power * Percentage of max drag length
        var totalSidewaysForce = sidewaysDirection * sidewaysPower;
        var totalForwardForce = forwardForce * transform.forward;
        playerRigidbody.AddForce(totalForwardForce + totalSidewaysForce);
    }

    private void Land(Transform landedOn)
    {
        OnLand(landedOn);
        playerRigidbody.isKinematic = true;
        currentlyLandedOn = landedOn;
        VelocityUpdated(Vector3.zero);
        StartCoroutine(LandingLerp(transform.position, landedOn.position, landLerpTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConstants.LandingTrigger_Tag) && other.transform != currentlyLandedOn)
        {
            Land(other.transform);
        }
    }

    private IEnumerator LandingLerp(Vector3 initialPosition, Vector3 targetPosition, float lerpTime)
    {
        float time = 0;
        while(time < lerpTime)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, time / lerpTime);
            yield return null;
        }
        inFlight = false;
    }
}

public delegate void PositionUpdatedEventHandler(Vector3 newPosition);
public delegate void InputDetectedEventHandler(bool detected);
public delegate void VelocityUpdatedEventHandler(Vector3 velocity);
public delegate void OnLandEventHandler(Transform landedOn);
public delegate void OnFireEventHandler();