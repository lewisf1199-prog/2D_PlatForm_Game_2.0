using UnityEngine;

public class JumpBreakBlock : MonoBehaviour
{
    [Tooltip("Minimum downward speed required to break the block when landing on it.")]
    [SerializeField] private float minBreakImpactSpeed = 5f;

    [Tooltip("Optional particle prefab or broken pieces prefab to spawn.")]
    [SerializeField] private GameObject breakEffectPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // Check if the player landed on top of the block
        bool landedOnTop = false;
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // A normal pointing downward (y < -0.5) means the other object hit from above
            if (contact.normal.y < -0.5f)
            {
                landedOnTop = true;
                break;
            }
        }

        if (!landedOnTop) return;

        // Check downward speed at the moment of impact
        // relativeVelocity.y > 0 means the player was moving downward relative to the block
        float impactSpeed = collision.relativeVelocity.y;

        if (impactSpeed >= minBreakImpactSpeed)
        {
            BreakBlock();
        }
    }

    private void BreakBlock()
    {
        if (breakEffectPrefab != null)
        {
            Instantiate(breakEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}