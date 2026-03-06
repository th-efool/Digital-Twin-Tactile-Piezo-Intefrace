using UnityEngine;

public class MoveDownDeltaY : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float deltaY = 2.06f;   // how much to move down
    [SerializeField] private float duration = 1.5f;  // time in seconds

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float elapsed;

    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition +Vector3.down * deltaY;
        elapsed = 0f;
    }

    void Update()
    {
        if (elapsed >= duration)
            return;

        elapsed += Time.deltaTime;

        float t = Mathf.Clamp01(elapsed / duration);

        transform.position = Vector3.Lerp(startPosition, endPosition, t);
    }
}
