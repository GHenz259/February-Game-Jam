using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public Transform player;
    public GameObject projectilePrefab; // The horse projectile
    public Transform firePoint;

    public float fireRate = 1f;
    public float projectileSpeed = 10f;
    public float detectionRange = 15f;

    private float nextFireTime;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            AimAtPlayer(); // Enemy rotation / flip stays intact

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void AimAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Keep your top-to-bottom flip logic exactly
        if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(180f, 0f, -angle);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    void Shoot()
    {
        // Spawn the horse projectile with enemy rotation
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );

        // Move the horse along its local X-axis (change to transform.forward if needed)
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = projectile.transform.right * projectileSpeed;
        }

        // Prevent collision with the enemy
        Collider projCollider = projectile.GetComponent<Collider>();
        Collider enemyCollider = GetComponent<Collider>();
        if (projCollider != null && enemyCollider != null)
        {
            Physics.IgnoreCollision(projCollider, enemyCollider);
        }

        // Destroy after 5 seconds to clean up
        Destroy(projectile, 5f);
    }
}
