using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int advanceTimeMin;
    public int advanceTimeMax;
    public int flankTimeMin;
    public int flankTimeMax;
    public int stopTimeMin;
    public int stopTimeMax;
    public int hitTime;
    public float advanceSpeed;
    public float flankSpeed;

    public int health;
    
    private GameObject player;
    private EnemyState state;

    public delegate void EnemyHit();
    public event EnemyHit OnEnemyHit;

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
            HandleProjectileHit(collider);
        }
    }

    private void HandleProjectileHit(Collider projectile)
    {
        Destroy(projectile.gameObject);

        OnEnemyHit();
        var audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        --health;
        if (health <= 0)
        {
            GetComponent<MeshRenderer>().enabled = false;
            var renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var renderer in renderers)
                renderer.enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            Destroy(gameObject, audioSource.clip.length);
        }
    }
}
