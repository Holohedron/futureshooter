using UnityEngine;

public enum MovementType
{
    Stop,
    Advance,
    Flank
}

public class Enemy : MonoBehaviour {

    public float advanceTimeMin;
    public float advanceTimeMax;
    public float flankTimeMin;
    public float flankTimeMax;
    public float stopTimeMin;
    public float stopTimeMax;
    public float advanceSpeed;
    public float flankSpeed;
    public int stopChance;

    private bool flankingLeft = true;
    private Vector3 flankCenter;
    private float movementTimer = 30f;
    private GameObject player;

    public MovementType currentMovement = MovementType.Stop;

    private MovementType CurrentMovement
    {
        get
        {
            return currentMovement;
        }
        set
        {
            switch (value)
            {
                case MovementType.Advance:
                    currentMovement = MovementType.Advance;
                    movementTimer = Random.Range(advanceTimeMin, advanceTimeMax);
                    break;
                case MovementType.Flank:
                    flankingLeft = Random.Range(0f, 2f) < 1f;
                    flankCenter = player.transform.position;
                    currentMovement = MovementType.Flank;
                    movementTimer = Random.Range(flankTimeMin, flankTimeMax);
                    break;
                default:
                    // Stop by default
                    currentMovement = MovementType.Stop;
                    movementTimer = Random.Range(stopTimeMin, stopTimeMax);
                    break;
            }
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update ()
    {
        if (player.GetComponent<PlayerCharacter>().dead)
        {
            CurrentMovement = MovementType.Stop;
            return;
        }

        transform.LookAt(player.transform);

        if (movementTimer <= 0)
        {
            ChooseNextMovement();
        }

        switch (CurrentMovement)
        {
            case MovementType.Advance:
                Advance();
                break;
            case MovementType.Flank:
                Flank();
                break;
            default:
                // Stop by default
                break;
        }

        movementTimer--;
	}

    private void OnTriggerEnter(Collider collider)
    {
        print("TRIGGER");

        if(collider.CompareTag("Projectile"))
        {
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
    }

    private void ChooseNextMovement()
    {
        if (Random.Range(0f, 1f) <= stopChance / 100f)
        {
            // Randomly stop based on stopChance
            CurrentMovement = MovementType.Stop;
            return;
        }

        switch (currentMovement)
        {
            case MovementType.Stop:
                // randomly pick between Advance and Flank
                if (Random.Range(0f, 2f) > 1f)
                {
                    CurrentMovement = MovementType.Advance;
                }
                else
                {
                    CurrentMovement = MovementType.Flank;
                }
                break;
            case MovementType.Advance:
                CurrentMovement = MovementType.Flank;
                break;
            case MovementType.Flank:
                CurrentMovement = MovementType.Advance;
                break;
            default:
                CurrentMovement = MovementType.Stop;
                break;
        }
    }

    private void Advance()
    {
        // Move towards player
        transform.Translate(Vector3.forward * advanceSpeed * Time.deltaTime);
    }

    private void Flank()
    {
        // Rotate around player position
        Vector3 vectorToCenter = transform.position - flankCenter;
        float speed = flankingLeft ? flankSpeed : -flankSpeed;
        speed = speed / vectorToCenter.magnitude * 180 / Mathf.PI; // maintain linear velocity
        vectorToCenter = Quaternion.AngleAxis(speed * Time.deltaTime, Vector3.up) * vectorToCenter;
        transform.position = vectorToCenter + flankCenter;
    }
}
