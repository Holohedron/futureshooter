using UnityEngine;

public class StopState : ScriptableObject, EnemyState
{
    private int timer;
    private Transform player;

    public EnemyState HandleTransition(Enemy enemy)
    {
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
        enemy.transform.LookAt(player);
        --timer;
    }

    public void OnEnter(Enemy enemy)
    {
        // set timer based on enemy settings
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timer = Random.Range(enemy.stopTimeMin, enemy.stopTimeMax+1);
    }

    public void OnExit(Enemy enemy)
    {
        // nothing
    }
}
