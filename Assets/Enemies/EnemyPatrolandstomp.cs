using UnityEngine;

public class EnemyPatrolandstomp : MonoBehaviour
{
    public enum PatrolDirection { SideToSide, UpAndDown }

    [Header("Movement Settings")]
    [SerializeField] private PatrolDirection movementType = PatrolDirection.SideToSide;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float moveDistance = 3f;

    [Header("Stomp Settings")]
    [SerializeField] private float bounceForce = 10f;

    private Vector2 startPosition;
    private int direction = 1;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (movementType == PatrolDirection.SideToSide)
        {
            MoveHorizontal();
        }
        else
        {
            MoveVertical();
        }
    }

    private void MoveHorizontal()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        if (transform.position.x >= startPosition.x + moveDistance)
        {
            direction = -1;
            if (spriteRenderer != null) spriteRenderer.flipX = true;
        }
        else if (transform.position.x <= startPosition.x - moveDistance)
        {
            direction = 1;
            if (spriteRenderer != null) spriteRenderer.flipX = false;
        }
    }

    private void MoveVertical()
    {
        transform.Translate(Vector2.up * direction * speed * Time.deltaTime);

        if (transform.position.y >= startPosition.y + moveDistance)
        {
            direction = -1;
        }
        else if (transform.position.y <= startPosition.y - moveDistance)
        {
            direction = 1;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            if (playerRb != null && playerRb.linearVelocity.y <= 0.1f)
            {
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);

                Destroy(gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerRespawn>()?.Respawn();
        }
    }
}