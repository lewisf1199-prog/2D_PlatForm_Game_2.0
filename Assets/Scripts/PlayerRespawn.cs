using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector2 respawnPoint;
    private Rigidbody2D rb;

    void Start()
    {
        respawnPoint = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    public void UpdateCheckpoint(Vector2 newPosition)
    {
        respawnPoint = newPosition;
        Debug.Log("2D Checkpoint saved!");
    }

    public void Respawn()
    {
        transform.position = respawnPoint;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}

