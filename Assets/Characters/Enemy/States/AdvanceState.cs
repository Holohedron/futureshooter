using UnityEngine;

public class AdvanceState : EnemyBaseState, IEnemyState
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
            return new StopState();
        else
            return new FlankState();
    }

    public new void HandleUpdate(Enemy enemy)
    {
        enemy.transform.LookAt(player);
        doAdvance(enemy);
        --timer;
    }

    public new void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);

        // Set timer based on enemy settings
        timer = Random.Range(enemy.advanceTimeMin, enemy.advanceTimeMax + 1);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public new void OnExit(Enemy enemy)
    {
        base.OnExit(enemy);
    }

    private void doAdvance(Enemy enemy)
    {
        // Move towards player
        enemy.GetComponent<CharacterController>().Move(enemy.transform.TransformDirection(Vector3.forward) * enemy.advanceSpeed * Time.deltaTime);
    }
}
