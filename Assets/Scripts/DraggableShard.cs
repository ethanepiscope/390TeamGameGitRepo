using UnityEngine;

public class DraggableShard : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private bool isSnapped = false;
    private PuzzleManager manager;

    [Header("Snapping")]
    public Vector3 correctPosition;
    public float snapThreshold = 0.5f;

    void Start()
    {
        manager = FindObjectOfType<PuzzleManager>();
    }

    void OnMouseDown()
    {
        if (isSnapped) return;

        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging && !isSnapped)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            newPosition.z = 0;
            transform.position = newPosition;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        if (!isSnapped && Vector3.Distance(transform.position, correctPosition) <= snapThreshold)
        {
            transform.position = correctPosition;
            isSnapped = true;
            manager?.NotifyShardSnapped();
        }
    }
}
