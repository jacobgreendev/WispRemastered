using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public event CameraPositionUpdatedEventHandler CameraPositionUpdated;

    [SerializeField] private float followLerpSpeed;
    private Transform following;
    private Vector3 offset;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        following = PlayerController.Instance.transform;
        offset = transform.position - following.position;
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

    public delegate void CameraPositionUpdatedEventHandler(Vector3 position);
}
