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
    [SerializeField] private float maxDragDistanceAsScreenWidthRatio;


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
            return maxDragDistanceAsScreenWidthRatio * Screen.width;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        PlayerController.Instance.DragPositionUpdated += OnDragPositionUpdate;
        PlayerController.Instance.PlayerPositionUpdated += OnPlayerPositionUpdate;
        PlayerController.Instance.TouchDetectedWhileNotInFlight += DragEnabled;
        CameraController.Instance.CameraPositionUpdated += OnCameraPositionUpdate;
        mainCamera = Camera.main;
        OnPlayerPositionUpdate(PlayerController.Instance.transform.position); //Initialise player position on first frame
        dragLineRenderer.SetPosition(0, playerScreenPosition); //SetPosition is relative to line renderer position, which in this case is the bottom left
    }

    private void OnDragPositionUpdate(Vector3 position)
    {
        var newDragPos = (Vector2) position;
        if(Vector3.Distance(playerScreenPosition, newDragPos) > maxDragDistanceAsScreenWidthRatio * Screen.width)
        {
            newDragPos = playerScreenPosition + (newDragPos - playerScreenPosition).normalized * maxDragDistanceAsScreenWidthRatio * Screen.width;
        }

        dragLineRenderer.SetPosition(1, (Vector2) newDragPos);
    }

    private void OnPlayerPositionUpdate(Vector3 position)
    {
        playerScreenPosition = mainCamera.WorldToScreenPoint(position);
        dragLineRenderer.SetPosition(0, playerScreenPosition); //Sets centre of line renderer to the player's position on screen
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
