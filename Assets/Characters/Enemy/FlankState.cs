using UnityEngine;

public class FlankState : ScriptableObject, EnemyState
{
    Vector3 flankCenter;
    bool flankingLeft;
    private int timer;
    private Transform player;

    public EnemyState HandleTransition(Enemy enemy)
    {
        if (timer > 0)
            return null;

        if (Random.value > 0.5f)
            return ScriptableObject.CreateInstance<StopState>();
        else
            return ScriptableObject.CreateInstance<AdvanceState>();
    }

    public void HandleUpdate(Enemy enemy)
    {
        enemy.transform.LookAt(player);
        doFlank(enemy);
        --timer;
    }

    public void OnEnter(Enemy enemy)
    {
        // set timer based on enemy settings
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timer = Random.Range(enemy.flankTimeMin, enemy.flankTimeMax + 1);
        flankCenter = GameObject.FindGameObjectWithTag("Player").transform.position;
        flankingLeft = Random.value > 0.5f;
    }

    public void OnExit(Enemy enemy)
    {
        // nothing
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
