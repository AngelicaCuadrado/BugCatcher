using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        // Safely set animator parameter only if an Animator exists
        if (enemy.animator != null)
        {
            enemy.animator.SetFloat("speed", enemy.patrolSpeed * 1.5f);
        }

        if (enemy.agent != null)
        {
            enemy.agent.isStopped = false;
            enemy.agent.speed = enemy.patrolSpeed * 1.5f;
            enemy.agent.stoppingDistance = enemy.attackRange;
        }
    }
    public override void UpdateState(EnemyStateManager enemy)
    {
        // If no player or no agent, stop chasing and go idle
        if (enemy.player == null || enemy.agent == null)
        {
            if (enemy.agent != null)
                enemy.agent.ResetPath();

            enemy.SwitchState(enemy.idleState);
            return;
        }

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.position);

        // Player too far — go back to patrol
        if (distanceToPlayer > enemy.detectionRange)
        {
            enemy.agent.ResetPath();
            enemy.SwitchState(enemy.patrolState);
            return;
        }

        // Otherwise chase via NavMesh
        enemy.agent.SetDestination(enemy.player.position);
  

    // Close enough  attack
    //if (distanceToPlayer <= enemy.attackRange)
    //{
    //    enemy.agent.ResetPath();
    //    enemy.SwitchState(enemy.attackingState);
    //    return;
    //}
       
    }
    public override void ExitState(EnemyStateManager enemy)
    {

        // Reset chase animation
        //enemy.GetComponent<Animator>().SetBool("isChasing", false);

        if (enemy.agent != null)
            enemy.agent.ResetPath();

    }

}
