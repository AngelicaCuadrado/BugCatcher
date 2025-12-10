using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float idleDuration;
    private float elapsedTime;
    public override void EnterState(EnemyStateManager enemy)
    {

        if (enemy.animator != null)
            enemy.animator.SetFloat("speed", 0f);

        if (enemy.agent != null)
        {
            enemy.agent.isStopped = true;
            enemy.agent.ResetPath();
            enemy.agent.velocity = Vector3.zero;   // kills any residual slide
        }

        elapsedTime = 0f;

        float variation = enemy.idleDurationVariance * enemy.idleDuration;
        float min = enemy.idleDuration - variation;
        float max = enemy.idleDuration + variation;
        
        idleDuration = Random.Range(min, max);
        
    }
    public override void UpdateState(EnemyStateManager enemy)
    {

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= idleDuration)
        {
            enemy.SwitchState(enemy.patrolState);
            

        }

    }
    public override void ExitState(EnemyStateManager enemy)
    {
        
        //enemy.animator.SetFloat("speed", 1f);
        
    }

}
