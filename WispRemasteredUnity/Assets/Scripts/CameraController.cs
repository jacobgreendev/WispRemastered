using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public event CameraPositionUpdatedEventHandler CameraPositionUpdated;

    private Transform following;
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

    private void Start()
    {
        following = PlayerController.Instance.transform;
        offset = transform.position - following.position;
        PlayerController.Instance.VelocityUpdated += OnPlayerVelocityUpdate;
        PlayerController.Instance.OnLand += UpdatePreviousLandedHeight;
        PlayerController.Instance.OnFire += EnableBirdseye;
        focal = CameraFocal.TransformInstance;
        birdseyeViewOffset = birdseyeViewHeight * Vector3.up;
        previousLandedHeight = following.position.y;
    }

    // Update is called once per Physics Update
    void FixedUpdate()
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
            CameraPositionUpdated(transform.position);
        }

        transform.LookAt(focal);
    }

    private void EnableBirdseye()
    {
        birdseyeViewEnabled = true;
    }

    private void DisableBirdseye()
    {
        birdseyeViewEnabled = false;
    }

    private void UpdatePreviousLandedHeight(Transform landedOn)
    {
        previousLandedHeight = landedOn.position.y;
        DisableBirdseye();
    }

    private void OnPlayerVelocityUpdate(Vector3 velocity)
    {
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, defaultFov + (fovPerUnitSpeed * velocity.z), fovLerpSpeed * Time.deltaTime);
    }

    public delegate void CameraPositionUpdatedEventHandler(Vector3 position);
}
