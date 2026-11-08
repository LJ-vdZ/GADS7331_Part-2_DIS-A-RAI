using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        TakeDamage(amount, transform.position);
    }

    public void TakeDamage(float amount, Vector3 hitPoint)
    {
        CombatEffects.ShockBot(transform, hitPoint);
        currentHealth -= amount;
        Debug.Log(gameObject.name + " took " + amount + " damage. Remaining: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        CombatEffects.Explode(transform.position);
        Destroy(gameObject);
    }
}
