using UnityEngine;

public class EnemyBaseState : IEnemyState
{
    protected bool gotHit = false;

    public IEnemyState HandleTransition(Enemy enemy)
    {
        if (gotHit)
            return new HitState();

        return null;
    }

    public void HandleUpdate(Enemy enemy)
    {
    }

    public void OnEnter(Enemy enemy)
    {
        enemy.OnEnemyHit += HandleHit;
    }

    public void OnExit(Enemy enemy)
    {
        enemy.OnEnemyHit -= HandleHit;
    }

    protected void HandleHit()
    {
        gotHit = true;
    }
}
