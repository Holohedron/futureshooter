using UnityEngine;

public class BaseEnemyState : ScriptableObject, EnemyState
{
    protected bool gotHit = false;

    public EnemyState HandleTransition(Enemy enemy)
    {
        if (gotHit)
            return ScriptableObject.CreateInstance<HitState>();

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
