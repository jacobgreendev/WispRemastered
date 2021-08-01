using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private LineRenderer dragLineRenderer;
    private Camera mainCamera;
    private Vector3 playerWorldPosition; //For use with centering line renderer

    public Vector2 DragVector
    {
        get
        {
            return dragLineRenderer.GetPosition(0) - dragLineRenderer.GetPosition(1);
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
        dragLineRenderer.SetPosition(0, mainCamera.WorldToScreenPoint(playerWorldPosition)); //SetPosition is relative to line renderer position, which in this case is the bottom left
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void OnDragPositionUpdate(Vector3 position)
    {
        dragLineRenderer.SetPosition(1, (Vector2) position);
    }

    private void OnPlayerPositionUpdate(Vector3 position)
    {
        playerWorldPosition = position;
        dragLineRenderer.SetPosition(0, mainCamera.WorldToScreenPoint(playerWorldPosition)); //Sets centre of line renderer to the player's position on screen
    }

    private void OnCameraPositionUpdate(Vector3 position)
    {
        dragLineRenderer.SetPosition(0, mainCamera.WorldToScreenPoint(playerWorldPosition)); //Sets centre of line renderer to the player's position on screen
    }

    private void DragEnabled(bool enabled)
    {
        dragLineRenderer.enabled = enabled;
    }
}
