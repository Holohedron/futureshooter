using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int advanceTimeMin;
    public int advanceTimeMax;
    public int flankTimeMin;
    public int flankTimeMax;
    public int stopTimeMin;
    public int stopTimeMax;
    public float advanceSpeed;
    public float flankSpeed;
    
    private GameObject player;
    private EnemyState state;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        state = ScriptableObject.CreateInstance<StopState>();
    }

    private void Update ()
    {
        if (player.GetComponent<PlayerCharacter>().dead)
        {
            state = ScriptableObject.CreateInstance<StopState>();
            return;
        }

        var newState = state.HandleTransition(this);
        if (newState != null)
        {
            state.OnExit(this);
            state = newState;
            state.OnEnter(this);
        }
        state.HandleUpdate(this);
	}

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Projectile"))
        {
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
    }
}
