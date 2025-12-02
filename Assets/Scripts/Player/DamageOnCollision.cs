using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damageAmount = 10;

    [Tooltip("If true, uses Trigger events; if false, uses Collision events.")]
    public bool useTrigger = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!useTrigger) return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (useTrigger) return;

        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
