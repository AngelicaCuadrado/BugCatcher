using UnityEngine;

public class SpiderContactDamage : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        // Make sure we only hit the player
        if (!other.CompareTag("Player"))
            return;

        // Get player health component
        var health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
        var enemyManager = GetComponentInParent<EnemyStateManager>();
        if (enemyManager != null) { enemyManager.SwitchState(enemyManager.walkBackState); }
    }
}
