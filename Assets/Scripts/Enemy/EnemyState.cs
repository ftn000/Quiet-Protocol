using UnityEngine;

public abstract class EnemyState
{
    protected EnemyAI enemy;

    public EnemyState(EnemyAI e)
    {
        enemy = e;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}
