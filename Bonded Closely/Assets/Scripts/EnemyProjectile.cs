using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SharedHealthSystem.Instance.TakeDamage(10f); // set your damage value
            Destroy(gameObject);
        }
    }
}
