using UnityEngine;

public class ReturnState : EnemyState
{
    private Transform returnTarget;

    public ReturnState(EnemyAI e) : base(e)
    {
        returnTarget = enemy.patrolPoints[enemy.patrolIndex];
    }

    public override void Update()
    {
        if (enemy.CanSeePlayer())
        {
            enemy.ChangeState(new ChaseState(enemy));
            return;
        }

        if (enemy.IsAtPoint(returnTarget.position))
        {
            enemy.ChangeState(new PatrolState(enemy));
        }
    }

    public override void FixedUpdate()
    {
        enemy.MoveTowards(returnTarget.position, enemy.patrolSpeed);
    }
}
