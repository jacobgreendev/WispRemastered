using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private LineRenderer dragLineRenderer;
    private Camera mainCamera;
    private Vector2 screenCentre = new(Screen.width / 2, Screen.height / 2);

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        PlayerController.Instance.DragPositionUpdated += OnDragPositionUpdate;
        mainCamera = Camera.main;
        dragLineRenderer.SetPosition(0, screenCentre); //Position is relative to line renderer position, which in this case is the bottom left
    }

    // Update is called once per frame
    private void Update()
    {

    }

    void OnDragPositionUpdate(Vector2 position)
    {
        Debug.Log($"Input drag position: {position}");
        dragLineRenderer.SetPosition(1, position);
    }
}
