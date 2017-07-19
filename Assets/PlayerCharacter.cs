using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private const float FIRETIMERMAX = 60f;

    public float moveSpeed;
    public float turnSpeed;
    public float fireRate;

    //debug info
    public float mouseX;
    public float mouseY;
    public float mouseZ;

    public Projectile projectileFab;

    private float fireTimer = 0;
    public bool dead = false;

	private void Update ()
    {
        if (dead)
        {
            return;
        }

        if (Input.GetKey("left shift"))
        {
            aim();
            checkFire();
            checkAimMovement();
        }
        else
        {
            checkMovement();
        }
        
        if (Input.GetKeyUp("left shift"))
        {
            //reset arm position
            transform.GetChild(0).rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var rb = GetComponent<Rigidbody>();
            rb.freezeRotation = false;

            dead = true;
        }
    }

    private void checkMovement()
    {
        float speed = moveSpeed * Time.deltaTime;

        if (Input.GetKey("up") || Input.GetKey("w"))
        {
            transform.Translate(new Vector3(0, 0, speed));
        }
        else if (Input.GetKey("down") || Input.GetKey("s"))
        {
            transform.Translate(new Vector3(0, 0, -speed));
        }

        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            transform.Rotate(new Vector3(0, -turnSpeed, 0));
        }
        else if (Input.GetKey("right") || Input.GetKey("d"))
        {
            transform.Rotate(new Vector3(0, turnSpeed, 0));
        }
    }

    private void checkAimMovement()
    {
        float speed = moveSpeed * Time.deltaTime;

        if (Input.GetKey("up") || Input.GetKey("w"))
        {
            transform.Translate(new Vector3(0, 0, speed));
        }
        else if (Input.GetKey("down") || Input.GetKey("s"))
        {
            transform.Translate(new Vector3(0, 0, -speed));
        }

        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            transform.Translate(new Vector3(-speed, 0, 0));
        }
        else if (Input.GetKey("right") || Input.GetKey("d"))
        {
            transform.Translate(new Vector3(speed, 0, 0));
        }
    }

    private void checkFire()
    {
        if (fireTimer > 0)
        {
            fireTimer -= fireRate;
        }

        if ((Input.GetKey("space") || Input.GetMouseButton(0)) && fireTimer <= 0)
        {
            Transform gun = transform.GetChild(0).GetChild(0);

            Projectile p = Instantiate<Projectile>(projectileFab);
            p.transform.rotation = gun.rotation * Quaternion.Euler(-90, 0, 0);
            p.transform.position = gun.position + gun.up * gun.localScale.y * 2;

            fireTimer = FIRETIMERMAX;

            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    private void aim()
    {
        var mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;
        mousePos.z = 300;

        Transform gun = transform.GetChild(0);
        gun.LookAt(transform.TransformPoint(mousePos));

        //Debug info
        mouseX = mousePos.x;
        mouseY = mousePos.y;
        mouseZ = mousePos.z;
    }
}
