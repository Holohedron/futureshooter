using UnityEngine;

public class HitState : EnemyBaseState, IEnemyState
{
    private const int COLORTIME = 5;

    private int timer;
    private int colorTimer;

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
        --timer;

        if (colorTimer <= 0)
        {
            Material mat = Resources.Load("EnemyMat", typeof(Material)) as Material;
            enemy.GetComponent<Renderer>().material = mat;
        }
        else
        {
            --colorTimer;
        }
    }

    public new void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);

        // set timer based on enemy settings
        timer = enemy.hitTime;
        colorTimer = COLORTIME;
        Material mat = Resources.Load("EnemyHurtMat", typeof(Material)) as Material;
        enemy.GetComponent<Renderer>().material = mat;
    }

    public new void OnExit(Enemy enemy)
    {
        base.OnExit(enemy);
    }
}
