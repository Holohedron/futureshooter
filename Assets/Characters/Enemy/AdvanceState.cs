using UnityEngine;

public class AdvanceState : ScriptableObject, EnemyState
{
    private int timer;
    private Transform player;

    public EnemyState HandleTransition(Enemy enemy)
    {
        if (timer > 0)
            return null;
        
        if (Random.value > 0.5f)
            return ScriptableObject.CreateInstance<StopState>();
        else
            return ScriptableObject.CreateInstance<FlankState>();
    }

    public void HandleUpdate(Enemy enemy)
    {
        enemy.transform.LookAt(player);
        doAdvance(enemy);
        --timer;
    }

    public void OnEnter(Enemy enemy)
    {
        // Set timer based on enemy settings
        timer = Random.Range(enemy.advanceTimeMin, enemy.advanceTimeMax + 1);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void OnExit(Enemy enemy)
    {
        // nothing
    }

    private void doAdvance(Enemy enemy)
    {
        // Move towards player
        enemy.transform.Translate(Vector3.forward * enemy.advanceSpeed * Time.deltaTime);
    }
}
