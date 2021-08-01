using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private LineRenderer dragLineRenderer;
    private Camera mainCamera;
    private Vector2 playerScreenPosition; //For use with centering line renderer

    // Start is called before the first frame update
    private void Start()
    {
        PlayerController.Instance.DragPositionUpdated += OnDragPositionUpdate;
        PlayerController.Instance.PlayerPositionUpdated += OnPlayerPositionUpdate;
        PlayerController.Instance.TouchDetectedWhileNotInFlight += DragEnabled;
        mainCamera = Camera.main;
        OnPlayerPositionUpdate(PlayerController.Instance.transform.position); //Initialise player position on first frame
        dragLineRenderer.SetPosition(0, playerScreenPosition); //SetPosition is relative to line renderer position, which in this case is the bottom left
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
        playerScreenPosition = mainCamera.WorldToScreenPoint(position);
        dragLineRenderer.SetPosition(0, playerScreenPosition); //Sets centre of line renderer to the player's position on screen
    }

    private void DragEnabled(bool enabled)
    {
        dragLineRenderer.enabled = enabled;
    }
}
