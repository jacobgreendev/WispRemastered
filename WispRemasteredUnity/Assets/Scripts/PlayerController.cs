using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public event PositionUpdatedEventHandler DragPositionUpdated;
    public event PositionUpdatedEventHandler PlayerPositionUpdated;
    public event InputDetectedEventHandler TouchDetectedWhileNotInFlight;
    public event OnLandEventHandler OnLand;
    public event OnFireEventHandler OnFire;
    public event VelocityUpdatedEventHandler VelocityUpdated;
    public event OnDeathEventHandler OnDeath;
    public event OnFormChangeEventHandler OnFormChange;

    private bool inFlight = false;
    private bool isInteracting = false;
    private bool wasTouchingLastFrame = false;
    private Vector2 touchPosition;

    private WispForm currentForm;

    public WispForm CurrentForm
    {
        get => currentForm;
    }

    [Header("Physics")]
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private float forwardForce, maxSidewaysForce;

    private Transform currentlyLandedOn;

    [Header("Time Values")]
    [SerializeField] private float landLerpTime;
    [SerializeField] private float deathLoadDelay;
    [SerializeField] private float powerlineTravelTime;

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

    private void Start()
    {
        currentForm = WispForm.Flame;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInteracting)
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

        if (Input.GetKeyDown(KeyCode.O))
        {
            ChangeForm(WispForm.Spark);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeForm(WispForm.Flame);
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
        var targetRotation = inFlight && playerRigidbody.velocity.magnitude > 0 ? Quaternion.LookRotation(playerRigidbody.velocity.normalized) : Quaternion.LookRotation(Vector3.down);
        bodyTransform.rotation = Quaternion.Lerp(bodyTransform.rotation, targetRotation, bodyDirectionLerpSpeed * Time.deltaTime);
    }

    private void Fire()
    {
        OnFire();
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.isKinematic = false;
        inFlight = true;
        var sidewaysDirection = (Vector3) DragManager.Instance.DragVector.normalized;
        var sidewaysPower = maxSidewaysForce * (DragManager.Instance.DragVector.magnitude / DragManager.Instance.MaxDragDistance); //Max power * Percentage of max drag length
        var totalSidewaysForce = sidewaysDirection * sidewaysPower;
        var totalForwardForce = forwardForce * transform.forward;
        playerRigidbody.AddForce(totalForwardForce + totalSidewaysForce);
    }

    private void Land(Transform landedOn, bool inPlace = false)
    {
        currentlyLandedOn = landedOn;

        if (!inPlace)
        {
            playerRigidbody.isKinematic = true;
            VelocityUpdated(Vector3.zero);
            StartCoroutine(LandingLerp(transform.position, landedOn.position, landedOn));
        }
        else
        {
            inFlight = false;
            OnLand(currentlyLandedOn);
        }
    }

    private void ChangeForm(WispForm newForm)
    {
        OnFormChange(currentForm, newForm);
        currentForm = newForm;
    }

    private void Die()
    {
        OnDeath();
        bodyTransform.gameObject.SetActive(false);
        playerRigidbody.isKinematic = true;
        StartCoroutine(WaitAndReloadScene());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isInteracting && inFlight)
        {
            if (other.CompareTag(GameConstants.Tag_LandingTrigger) && other.transform != currentlyLandedOn)
            {
                Land(other.transform);
            }
            else if (other.CompareTag(GameConstants.Tag_InteractableTrigger) && other.transform != currentlyLandedOn)
            {
                TryInteractWith(other.transform);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(GameConstants.Tag_Ground))
        {
            Die();
        }
    }

    private void TryInteractWith(Transform interactableTransform)
    {
        Interactable interactable = interactableTransform.GetComponent<Interactable>();
        Debug.Log($"Trying to interact with {interactable.Type}");

        if (interactable.IsUsableBy(currentForm))
        {
            currentlyLandedOn = interactableTransform;
            switch (interactable.Type)
            {
                case InteractableType.Powerline:
                    InteractPowerline(interactableTransform, (Powerline) interactable);
                    break;

                case InteractableType.Firework:
                    InteractFirework(interactableTransform, (Firework)interactable);
                    break;
            }
        }
    }

    private IEnumerator LerpToPosition(Transform lerpingTransform, float lerpTime, Vector3 initialPosition, Vector3 endPosition, bool isPlayer = true)
    {
        float time = 0;
        while (time < lerpTime)
        {
            time += Time.deltaTime;
            lerpingTransform.position = Vector3.Lerp(initialPosition, endPosition, time / lerpTime);
            if(isPlayer) PlayerPositionUpdated(transform.position);
            yield return null;
        }
    }

    private IEnumerator SmoothLerpToPosition(Transform lerpingTransform, float lerpTime, Vector3 initialPosition, Vector3 endPosition, bool isPlayer = true)
    {
        float time = 0;
        while (time < lerpTime)
        {
            time += Time.deltaTime;
            lerpingTransform.position = Vector3.Lerp(initialPosition, endPosition, Mathf.SmoothStep(0, 1, time / lerpTime));
            if (isPlayer) PlayerPositionUpdated(transform.position);
            yield return null;
        }
    }

    private void InteractPowerline(Transform powerlineTransform, Powerline powerline)
    {
        StartCoroutine(RidePowerline(transform.position, powerlineTransform.position, powerline.EndTransform));
    }

    private IEnumerator RidePowerline(Vector3 initialPlayerPosition, Vector3 powerlineStartPosition, Transform powerlineEndTransform)
    {
        var currentSpeed = playerRigidbody.velocity.magnitude;
        playerRigidbody.isKinematic = true;
        isInteracting = true;

        yield return StartCoroutine(LerpToPosition(transform, landLerpTime, initialPlayerPosition, powerlineStartPosition));

        var endPosition = powerlineEndTransform.position;
        playerRigidbody.velocity = (endPosition - powerlineStartPosition).normalized * currentSpeed;
        VelocityUpdated(playerRigidbody.velocity);
        UpdateBodyFacingDirection();

        yield return StartCoroutine(LerpToPosition(transform, powerlineTravelTime, powerlineStartPosition, powerlineEndTransform.position));

        Land(powerlineEndTransform);
        isInteracting = false;
    }

    private void InteractFirework(Transform fireworkTransform, Firework firework)
    {
        StartCoroutine(RideFirework(fireworkTransform, firework));
    }

    private IEnumerator RideFirework(Transform fireworkTransform, Firework firework)
    {
        OnLand(fireworkTransform);
        playerRigidbody.isKinematic = true;
        isInteracting = true;

        yield return StartCoroutine(LerpToPosition(transform, landLerpTime, transform.position, firework.FuseStart.position));

        playerRigidbody.velocity = (firework.FuseEnd.position - firework.FuseStart.position).normalized;
        VelocityUpdated(playerRigidbody.velocity);
        UpdateBodyFacingDirection();

        yield return StartCoroutine(LerpToPosition(transform, firework.FuseTime, firework.FuseStart.position, firework.FuseEnd.position));

        var endPosition = firework.ParentTransform.position + firework.ParentTransform.up * firework.Distance;


        transform.parent = firework.ParentTransform;
        yield return StartCoroutine(SmoothLerpToPosition(firework.ParentTransform, firework.TravelTime, firework.ParentTransform.position, endPosition));
        transform.parent = null;

        playerRigidbody.isKinematic = false;
        Land(fireworkTransform, inPlace : true);
        playerRigidbody.velocity = fireworkTransform.up * firework.ExplosionForce;
        isInteracting = false;
        Destroy(firework.ParentTransform.gameObject);
    }

    //Probably temporary until a proper death handler is created
    private IEnumerator WaitAndReloadScene()
    {
        yield return new WaitForSeconds(deathLoadDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator LandingLerp(Vector3 initialPosition, Vector3 targetPosition, Transform landedOn)
    {
        float time = 0;
        while (time < landLerpTime)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, time / landLerpTime);
            yield return null;
        }
        inFlight = false;
        OnLand(landedOn);
    }
}

public delegate void PositionUpdatedEventHandler(Vector3 newPosition);
public delegate void InputDetectedEventHandler(bool detected);
public delegate void VelocityUpdatedEventHandler(Vector3 velocity);
public delegate void OnLandEventHandler(Transform landedOn);
public delegate void OnFireEventHandler();
public delegate void OnDeathEventHandler();
public delegate void OnFormChangeEventHandler(WispForm oldForm, WispForm newForm);
