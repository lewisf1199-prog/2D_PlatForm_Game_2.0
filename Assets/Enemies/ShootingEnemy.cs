using UnityEngine;

public class ShootingEnemy : MonoBehaviour 

{
    [Header("Refrrences")]
    public GameObject Bullet;
    public Transform firePoint;
    private Transform Player;

    [Header("Refrrences")]
    public float detectionRange = 8f;
    public float fireRate = 2f;
    private float nextFireTime = 0f;

    void Start() 
    {
        GameObject PlayerObj = GameObject.FindGameObjectWithTag("Player");

        if (PlayerObj == null)
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

    void OnDrawnGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
