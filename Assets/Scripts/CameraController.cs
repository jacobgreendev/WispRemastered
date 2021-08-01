using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public event CameraPositionUpdatedEventHandler CameraPositionUpdated;

    private Transform following;
    private Vector3 offset;
    private new Camera camera;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var currentPosition = transform.position;
        var targetPosition = following.position + offset;

        if (Vector3.Distance(currentPosition, targetPosition) > 0)
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, followLerpSpeed * Time.deltaTime);
            CameraPositionUpdated(transform.position);
        }
    }
    private void OnPlayerVelocityUpdate(Vector3 velocity)
    {
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, defaultFov + (fovPerUnitSpeed * velocity.z), fovLerpSpeed * Time.deltaTime);
    }

    public delegate void CameraPositionUpdatedEventHandler(Vector3 position);
}
