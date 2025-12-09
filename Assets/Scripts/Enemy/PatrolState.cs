using UnityEngine;

public class PatrolState : EnemyState
{
    public PatrolState(EnemyAI e) : base(e) { }

    public override void Enter()
    {
        if (enemy.patrolPoints.Length == 0)
            return;
    }

    public override void Update()
    {
        if (enemy.CanSeePlayer())
        {
            enemy.ChangeState(new ChaseState(enemy));
            return;
        }

        if (enemy.CanHearPlayer())
        {
            Vector3 pos = enemy.player.position;
            enemy.ChangeState(new InvestigateState(enemy, pos));
            return;
        }
    }

    public override void FixedUpdate()
    {
        if (enemy.patrolPoints.Length == 0)
            return;

        Transform target = enemy.patrolPoints[enemy.patrolIndex];

        enemy.MoveTowards(target.position, enemy.patrolSpeed);

        if (enemy.IsAtPoint(target.position))
        {
            enemy.patrolIndex++;
            if (enemy.patrolIndex >= enemy.patrolPoints.Length)
                enemy.patrolIndex = 0;
        }
    }
}
