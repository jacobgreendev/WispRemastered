using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    public static DragManager Instance;

    private Camera mainCamera;
    private Vector2 playerScreenPosition; //For use with centering line renderer

    [Header("Drag Indicator")]
    [SerializeField] private LineRenderer dragLineRenderer;

    [Range(0f, 1f)]
    [SerializeField] private float maxDragDistanceAsScreenSizeRatio;


    public Vector2 DragVector
    {
        get
        {
            return dragLineRenderer.GetPosition(0) - dragLineRenderer.GetPosition(1);
        }
    }

    public float MaxDragDistance
    {
        get
        {
            return maxDragDistanceAsScreenSizeRatio * Mathf.Min(Screen.width, Screen.height);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        PlayerController.Instance.DragPositionUpdated += OnDragPositionUpdate;
        PlayerController.Instance.TouchDetectedWhileNotInFlight += DragEnabled;
        CameraController.Instance.CameraPositionUpdated += OnCameraPositionUpdate;
    }

    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main;
        dragLineRenderer.SetPosition(0, playerScreenPosition); //SetPosition is relative to line renderer position, which in this case is the bottom left
    }

    private void OnDisable()
    {
        //Unsubscribe from all events
        PlayerController.Instance.DragPositionUpdated -= OnDragPositionUpdate;
        PlayerController.Instance.TouchDetectedWhileNotInFlight -= DragEnabled;
        CameraController.Instance.CameraPositionUpdated -= OnCameraPositionUpdate;
    }

    private void OnDragPositionUpdate(Vector3 position)
    {
        playerScreenPosition = mainCamera.WorldToScreenPoint(PlayerController.Instance.transform.position);
        dragLineRenderer.SetPosition(0, playerScreenPosition); //Sets centre of line renderer to the player's position on screen

        var newDragPos = (Vector2) position;
        if(Vector3.Distance(playerScreenPosition, newDragPos) > maxDragDistanceAsScreenSizeRatio * Mathf.Min(Screen.width, Screen.height))
        {
            newDragPos = playerScreenPosition + (newDragPos - playerScreenPosition).normalized * maxDragDistanceAsScreenSizeRatio * Mathf.Min(Screen.width, Screen.height);
        }

        dragLineRenderer.SetPosition(1, (Vector2) newDragPos);
    }

    private void OnCameraPositionUpdate(Vector3 position)
    {
        dragLineRenderer.SetPosition(0, playerScreenPosition); //Sets centre of line renderer to the player's position on screen
    }

    private void DragEnabled(bool enabled)
    {
        dragLineRenderer.enabled = enabled;
    }
}
