using Unity.Collections;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [Header("References")]
    public GameObject Bullet;
    public Transform firePoint;
    private Transform Player;

    [Header("Combat Settings")]
    public float detectionRange = 8f;
    public float fireRate = 2f;
    public float bounceForce = 8f;
    private float nextFireTime = 0f;

    void Start()
    {
        GameObject PlayerObj = GameObject.FindGameObjectWithTag("Player");

        // Fixed: Changed == to != so it assigns Player when found
        if (PlayerObj != null)
        {
            Player = PlayerObj.transform;
        }
    }

    void Update()
    {
        if (Player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, Player.position);
        if (distanceToPlayer <= detectionRange && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        GameObject proj = Instantiate(Bullet, spawnPos, Quaternion.identity);

        Vector2 direction = (Player.position - spawnPos).normalized;
        proj.GetComponent<BulletScript>().Launch(direction);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                    if (playerRb != null)
                    {
                        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);
                    }

                    Destroy(gameObject);
                    return;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}