using UnityEngine;

public class EnemyWalkBackState : EnemyBaseState
{
    private float walkBackDuration = 2f; 
    private float elapsedTime = 0f;
    private const float WALK_SPEED_MODIFIER = 3f;

    private const string RETREAT_PARAM = "isRetreating";

    public override void EnterState(EnemyStateManager enemy)
    {
        
        //Debug.Log("Entering Walk Back State");
        elapsedTime = 0f;
        enemy.animator.SetBool(RETREAT_PARAM, true);

        if (enemy.agent != null)
        {
            enemy.agent.isStopped = true;
            enemy.agent.ResetPath();

        }
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        elapsedTime += Time.deltaTime;
        Vector3 backDirection = -enemy.transform.forward;
        Vector3 newPosition = enemy.transform.position +
                              backDirection * (enemy.patrolSpeed * WALK_SPEED_MODIFIER) * Time.deltaTime;

        enemy.transform.position = newPosition;

        if (elapsedTime >= walkBackDuration)
        {
        
            enemy.SwitchState(enemy.idleState);
        }
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        enemy.animator.SetBool(RETREAT_PARAM, false);
        if (enemy.agent != null)
        {
            enemy.agent.isStopped = false;
        }
    }

}