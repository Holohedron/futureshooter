using UnityEngine;

public class FlankState : BaseEnemyState, EnemyState
{
    Vector3 flankCenter;
    bool flankingLeft;
    private int timer;
    private Transform player;

    public new EnemyState HandleTransition(Enemy enemy)
    {
        var baseTransition = base.HandleTransition(enemy);
        if (baseTransition != null)
            return baseTransition;

        if (timer > 0)
            return null;

        if (Random.value > 0.5f)
            return ScriptableObject.CreateInstance<StopState>();
        else
            return ScriptableObject.CreateInstance<AdvanceState>();
    }

    public new void HandleUpdate(Enemy enemy)
    {
        enemy.transform.LookAt(player);
        doFlank(enemy);
        --timer;
    }

    public new void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);

        // set timer based on enemy settings
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timer = Random.Range(enemy.flankTimeMin, enemy.flankTimeMax + 1);
        flankCenter = GameObject.FindGameObjectWithTag("Player").transform.position;
        flankingLeft = Random.value > 0.5f;
    }

    public new void OnExit(Enemy enemy)
    {
        base.OnExit(enemy);
    }

    private void doFlank(Enemy enemy)
    {
        // Rotate around player position
        Vector3 vectorToCenter = enemy.transform.position - flankCenter;
        float speed = flankingLeft ? enemy.flankSpeed : -enemy.flankSpeed;
        speed = speed / vectorToCenter.magnitude * 180 / Mathf.PI; // maintain linear velocity
        vectorToCenter = Quaternion.AngleAxis(speed * Time.deltaTime, Vector3.up) * vectorToCenter;
        enemy.transform.position = vectorToCenter + flankCenter;
    }
}
