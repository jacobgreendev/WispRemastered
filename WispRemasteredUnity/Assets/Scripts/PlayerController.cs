using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public event PositionUpdatedEventHandler DragPositionUpdated;
    public event OnResetJourneyEventHandler OnResetJourney;
    public event InputDetectedEventHandler TouchDetectedWhileNotInFlight;
    public event OnLandEventHandler OnLand;
    public event OnFireEventHandler OnFire;
    public event OnDeathEventHandler OnDeath;
    public event OnFormChangeEventHandler OnFormChange;

    private bool inFlight = false;
    private bool isInteracting = false;
    private bool wasTouchingLastFrame = false;
    private Vector2 touchPosition;
    private Vector3 currentForceVector;

    private WispFormType currentForm;

    public WispFormType CurrentForm
    {
        get => currentForm;
    }

    public Rigidbody Rigidbody
    {
        get => playerRigidbody;
    }

    public bool IsInteracting
    {
        get => isInteracting; set => isInteracting = value;
    }

    [Header("Physics")]
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private float generalForceStrength, sidewaysForceMultiplier, forwardForceMultiplier, verticalForceMultiplier;

    private Interactable currentlyLandedOn;

    [Header("Time Values")]
    [SerializeField] private float landLerpTime;
    [SerializeField] private float deathLoadDelay;

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
        currentForm = WispFormType.Flame;
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
                UpdateBodyFacingDirection();
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            ChangeForm(WispFormType.Spark);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeForm(WispFormType.Flame);
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

            UpdateForceVector();
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

    public void UpdateBodyFacingDirection()
    {
        var targetRotation = inFlight && playerRigidbody.velocity.magnitude > 0 ? Quaternion.LookRotation(playerRigidbody.velocity.normalized) : Quaternion.LookRotation(Vector3.down);
        bodyTransform.rotation = Quaternion.Lerp(bodyTransform.rotation, targetRotation, bodyDirectionLerpSpeed * Time.deltaTime);
    }

    private void UpdateForceVector()
    {
        Vector2 dragVector = DragManager.Instance.DragVector;

        Vector3 xForce = transform.right * dragVector.normalized.x * sidewaysForceMultiplier;
        Vector3 zForce = transform.forward * dragVector.normalized.y * forwardForceMultiplier;
        Vector3 yForce = transform.up * dragVector.normalized.y * verticalForceMultiplier;
        if (zForce.z < 0) zForce = Vector3.zero; //Do not allow player to travel backwards

        currentForceVector = (xForce + yForce + zForce) * generalForceStrength * (dragVector.magnitude / DragManager.Instance.MaxDragDistance);
        TrajectoryRenderer.Instance.DisplayPath(transform.position, currentForceVector, playerRigidbody.mass, playerRigidbody.drag, 1);
    }

    private void Fire()
    {
        OnFire();
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.isKinematic = false;
        inFlight = true;
        
        playerRigidbody.AddForce(currentForceVector);
    }

    public void Land(Interactable landedOn, Transform landTransform, bool inPlace = false)
    {      
        ResetJourney();

        if (!inPlace)
        {
            playerRigidbody.isKinematic = true;
            StartCoroutine(LandingLerp(transform.position, landTransform.position, landedOn));
        }
        else
        {
            inFlight = false;
            OnLand(currentlyLandedOn);
        }
    }

    public void ResetJourney()
    {
        OnResetJourney(transform.position);
    }

    private void ChangeForm(WispFormType newForm)
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
            Interactable interactableHit = other.GetComponent<Interactable>();

            if(other.CompareTag(GameConstants.Tag_InteractableTrigger) && interactableHit != currentlyLandedOn)
            {
                currentlyLandedOn = interactableHit;
                TryInteractWith(interactableHit);
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

    private void TryInteractWith(Interactable interactable)
    {
        Transform interactableTransform = interactable.transform;
        Debug.Log($"Trying to interact with {interactable.Type}");

        if (interactable.IsUsableBy(currentForm))
        {
            currentlyLandedOn = interactable;
            interactable.DoInteraction(this);
        }
    }

    public IEnumerator LerpToPosition(Transform lerpingTransform, float lerpTime, Vector3 initialPosition, Vector3 endPosition)
    {
        float time = 0;
        while (time < lerpTime)
        {
            time += Time.deltaTime;
            lerpingTransform.position = Vector3.Lerp(initialPosition, endPosition, time / lerpTime);
            yield return null;
        }
    }

    public IEnumerator SmoothLerpToPosition(Transform lerpingTransform, float lerpTime, Vector3 initialPosition, Vector3 endPosition, bool isPlayer = true)
    {
        float time = 0;
        while (time < lerpTime)
        {
            time += Time.deltaTime;
            lerpingTransform.position = Vector3.Lerp(initialPosition, endPosition, Mathf.SmoothStep(0, 1, time / lerpTime));
            yield return null;
        }
    }

    //Probably temporary until a proper death handler is created
    private IEnumerator WaitAndReloadScene()
    {
        yield return new WaitForSeconds(deathLoadDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator LandingLerp(Vector3 initialPosition, Vector3 targetPosition, Interactable landedOn)
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
public delegate void OnLandEventHandler(Interactable landedOn);
public delegate void OnFireEventHandler();
public delegate void OnDeathEventHandler();
public delegate void OnFormChangeEventHandler(WispFormType oldForm, WispFormType newForm);
public delegate void OnResetJourneyEventHandler(Vector3 position);
