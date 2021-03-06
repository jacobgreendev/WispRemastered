using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public event CameraPositionUpdatedEventHandler CameraPositionUpdated;

    private Transform following;
    private Rigidbody followingRigidbody;
    private Transform focal;
    private Vector3 offset;
    private new Camera camera;

    [SerializeField] private float birdseyeViewHeight, heightForFullBirdseyeView;
    private float previousLandedHeight;
    private Vector3 birdseyeViewOffset;
    private bool birdseyeViewEnabled = false;

    [Header("Lerp Speeds")]
    [SerializeField] private float followLerpSpeed;

    [Header("FOV")]
    [SerializeField] private float defaultFov;
    [SerializeField] private float fovPerUnitSpeed, fovLerpSpeed;


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        camera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        PlayerController.Instance.OnResetJourney += UpdatePreviousLandedHeight;
        PlayerController.Instance.OnFire += EnableBirdseye;
        PlayerController.Instance.OnLand += DisableBirdseye;
    }

    private void Start()
    {
        following = PlayerController.Instance.transform;
        followingRigidbody = following.GetComponent<Rigidbody>();
        offset = transform.position - following.position;
        focal = CameraFocal.TransformInstance;
        birdseyeViewOffset = birdseyeViewHeight * Vector3.up;
        previousLandedHeight = following.position.y;
    }

    private void OnDisable()
    {
        //Unsubscribe from all events
        PlayerController.Instance.OnResetJourney -= UpdatePreviousLandedHeight;
        PlayerController.Instance.OnFire -= EnableBirdseye;
        PlayerController.Instance.OnLand -= DisableBirdseye;
    }

    void FixedUpdate()
    {
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        var followingHeightAbovePrevious = following.position.y - previousLandedHeight;

        float birdseyeAmount = 0;
        if (birdseyeViewEnabled)
        {
            birdseyeAmount = Mathf.Clamp(followingHeightAbovePrevious / heightForFullBirdseyeView, 0, 1);
        }

        var currentPosition = transform.position;
        var targetPosition = Vector3.Lerp(following.position + offset, following.position + birdseyeViewOffset, birdseyeAmount);

        if (Vector3.Distance(currentPosition, targetPosition) > 0)
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, followLerpSpeed * Time.deltaTime);
            CameraPositionUpdated?.Invoke(transform.position);
        }

        transform.LookAt(focal);
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, defaultFov + (fovPerUnitSpeed * followingRigidbody.velocity.z), fovLerpSpeed * Time.deltaTime);
    }

    public void EnableBirdseye()
    {
        birdseyeViewEnabled = true;
    }

    public void DisableBirdseye(Interactable landedOn = null)
    {
        birdseyeViewEnabled = false;
    }

    private void UpdatePreviousLandedHeight(Vector3 position)
    {
        previousLandedHeight = position.y;
    }

    public delegate void CameraPositionUpdatedEventHandler(Vector3 position);
}
