using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyAI e) : base(e) { }

    public override void Enter()
    {
        Debug.Log("CHASE!");
    }

    public override void Update()
    {
        if (!enemy.CanSeePlayer())
        {
            enemy.ChangeState(new ReturnState(enemy));
            return;
        }
    }

    public override void FixedUpdate()
    {
        enemy.MoveTowards(enemy.player.position, enemy.chaseSpeed);
    }
}
