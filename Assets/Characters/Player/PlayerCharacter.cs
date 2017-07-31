using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private const float FIRETIMERMAX = 60f;

    public float moveSpeed;
    public float turnSpeed;
    public float aimingTurnSpeed;
    public float fireRate;
    
    // for tuning aim when aiming at nothing
    public float radius = Screen.width/2;

    public Projectile projectileFab;
    public Texture2D crosshairImage;

    public bool aiming = false;
    public bool dead = false;

    private PlayerState state;

    private void Awake()
    {
        state = ScriptableObject.CreateInstance<MeleeState>();
    }

    private void Update ()
    {
        if (dead)
        {
            return;
        }

        // State implementation
        var newState = state.HandleTransition(this);
        if (newState != null)
        {
            state.OnExit(this);
            state = newState;
            state.OnEnter(this);
        }

        state.HandleUpdate(this);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    private void OnGUI()
    {
        if (aiming)
        {
            float xMin = Screen.width - (Screen.width - Input.mousePosition.x) - (crosshairImage.width / 2);
            float yMin = (Screen.height - Input.mousePosition.y) - (crosshairImage.height / 2);
            GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width, crosshairImage.height), crosshairImage);
        }
    }

    public void Die()
    {
        GetComponent<CharacterController>().enabled = false;
        gameObject.AddComponent<CapsuleCollider>();
        gameObject.AddComponent<Rigidbody>();
        transform.Rotate(new Vector3(1, 0, 1)); // to make sure we fall over dramatically

        dead = true;
        aiming = false;
    }
}
