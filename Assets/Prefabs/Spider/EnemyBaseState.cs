using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void EnterState(EnemyStateManager enemy);
    
    public abstract void UpdateState(EnemyStateManager enemy);

    public abstract void ExitState(EnemyStateManager enemy);
    public virtual void OnColliderEnter(EnemyStateManager enemy, Collider other)
    {
        
        if (!other.CompareTag("Player")) return;

        //  gets PlayerHealth
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth == null) return;

        // Use the enemys attackDamage
        int dmg = Mathf.RoundToInt(enemy.attackDamage);
        playerHealth.TakeDamage(dmg);
        
    }
}

