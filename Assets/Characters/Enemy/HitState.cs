using UnityEngine;

public class HitState : BaseEnemyState, EnemyState
{
    private const int COLORTIME = 5;

    private int timer;
    private int colorTimer;

    public EnemyState HandleTransition(Enemy enemy)
    {
        var baseTransition = base.HandleTransition(enemy);
        if (baseTransition != null)
            return baseTransition;

        if (timer > 0)
            return null;

        if (Random.value > 0.5f)
            return ScriptableObject.CreateInstance<AdvanceState>();
        else
            return ScriptableObject.CreateInstance<FlankState>();
    }

    public void HandleUpdate(Enemy enemy)
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

    public void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);

        // set timer based on enemy settings
        timer = enemy.hitTime;
        colorTimer = COLORTIME;
        Material mat = Resources.Load("EnemyHurtMat", typeof(Material)) as Material;
        enemy.GetComponent<Renderer>().material = mat;
    }

    public void OnExit(Enemy enemy)
    {
        base.OnExit(enemy);
    }
}
