using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FloorMarker : MonoBehaviour
{
    private DecalProjector projector;
    [SerializeField] private float minSize, maxSize;

    // Start is called before the first frame update
    void Start()
    {
        projector = GetComponent<DecalProjector>();
        PlayerController.Instance.PlayerPositionUpdated += UpdateFloorMarkerSize;
    }

    // Update is called once per frame
    private void UpdateFloorMarkerSize(Vector3 position)
    {
        var currentSize = projector.size;
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out var hit, currentSize.z))
        {
            var distance = Vector3.Distance(hit.point, transform.position);
            var newSize = Mathf.Lerp(minSize, maxSize, distance / currentSize.z);
            currentSize.x = newSize;
            currentSize.y = newSize;
            projector.size = currentSize;
        }
    }
}
