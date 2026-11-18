using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MoveDirection
    {
        Vertical,
        Horizontal
    }

    [Header("Movement Settings")]
    [SerializeField] private MoveDirection direction = MoveDirection.Vertical;
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float speed = 2f;

    [Header("Timing Settings")]
    [SerializeField] private float startDelay = 0f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingToTarget = true;
    private float delayTimer;
    private bool hasStarted = false;

    private void Start()
    {
        startPosition = transform.position;

        if (direction == MoveDirection.Vertical)
        {
            targetPosition = startPosition + new Vector3(0f, moveDistance, 0f);
        }
        else
        {
            targetPosition = startPosition + new Vector3(moveDistance, 0f, 0f);
        }

        delayTimer = startDelay;
    }

    private void Update()
    {
        if (!hasStarted)
        {
            if (delayTimer > 0f)
            {
                delayTimer -= Time.deltaTime;
                return;
            }
            hasStarted = true;
        }

        Vector3 destination = movingToTarget ? targetPosition : startPosition;
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            transform.position = destination;
            movingToTarget = !movingToTarget;
        }
    }
}
