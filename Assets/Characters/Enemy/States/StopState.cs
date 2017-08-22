using UnityEngine;

public class StopState : EnemyBaseState, IEnemyState
{
    private int timer;
    private Transform player;

    public new IEnemyState HandleTransition(Enemy enemy)
    {
        var baseTransition = base.HandleTransition(enemy);
        if (baseTransition != null)
            return baseTransition;

        if (timer > 0)
            return null;

        if (Random.value > 0.5f)
            return new AdvanceState();
        else
            return new FlankState();
    }

    public new void HandleUpdate(Enemy enemy)
    {
        // no movement
        enemy.transform.LookAt(player);
        --timer;
    }

    public new void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);

        // set timer based on enemy settings
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timer = Random.Range(enemy.stopTimeMin, enemy.stopTimeMax+1);
    }

    public new void OnExit(Enemy enemy)
    {
        base.OnExit(enemy);
    }
}
