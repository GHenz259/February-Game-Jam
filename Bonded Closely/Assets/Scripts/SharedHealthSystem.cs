using UnityEngine;

public class SharedHealthSystem : MonoBehaviour
{
    public static SharedHealthSystem Instance;

    [Header("Health Settings")]
    public float maxHealth = 200f;
    private float currentHealth;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            currentHealth = maxHealth;
        }
        else
        {
            Destroy(gameObject);
        }
    }

public void TakeDamage(float damageAmount)
{
    currentHealth -= damageAmount;
    currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    Debug.Log("Shared Health: " + currentHealth);

    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    foreach (GameObject player in players)
    {
        Bandit bandit = player.GetComponent<Bandit>();
        if (bandit != null)
        {
            bandit.TriggerHurt();
            continue;
        }

        PlayerWeak playerWeak = player.GetComponent<PlayerWeak>();
        if (playerWeak != null)
            playerWeak.TriggerHurt();
    }

    if (currentHealth <= 0f)
    {
        Die();
    }
}
    void Die()
    {
        Debug.Log("GAME OVER - Shared Health Depleted");

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Bandit bandit = player.GetComponent<Bandit>();
            if (bandit != null)
            {
                bandit.TriggerDeath();
                continue;
            }

            PlayerWeak playerWeak = player.GetComponent<PlayerWeak>();
            if (playerWeak != null)
                playerWeak.TriggerDeath();
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}