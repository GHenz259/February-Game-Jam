using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public Transform player;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float projectileSpeed = 10f;
    public float detectionRange = 15f;

    private float nextFireTime;

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            AimAtPlayer();

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void AimAtPlayer()
    {
        Vector2 direction = player.position - transform.position;
        // Calculate angle toward player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Set rotation so front of the enemy faces the player
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Fire projectile toward where the enemy is facing
            rb.linearVelocity = projectile.transform.right * projectileSpeed;
        }

        Collider2D projCollider = projectile.GetComponent<Collider2D>();
        Collider2D enemyCollider = GetComponent<Collider2D>();
        if (projCollider != null && enemyCollider != null)
        {
            Physics2D.IgnoreCollision(projCollider, enemyCollider);
        }

        Destroy(projectile, 5f);
    }
}