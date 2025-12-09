using UnityEngine;

public class InvestigateState : EnemyState
{
    private Vector3 investigatePos;
    private float waitTimer = 1.0f;

    public InvestigateState(EnemyAI e, Vector3 pos) : base(e)
    {
        investigatePos = pos;
    }

    public override void Update()
    {
        if (enemy.CanSeePlayer())
        {
            enemy.ChangeState(new ChaseState(enemy));
            return;
        }

        if (enemy.IsAtPoint(investigatePos))
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
                enemy.ChangeState(new ReturnState(enemy));
        }
    }

    public override void FixedUpdate()
    {
        if (!enemy.IsAtPoint(investigatePos))
        {
            enemy.MoveTowards(investigatePos, enemy.patrolSpeed);
        }
    }
}
