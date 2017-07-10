using UnityEngine;

public class CharacterController : MonoBehaviour {

    private const float MOVESCALE = 0.01f;
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

	private void Update ()
    {
        checkMovement();

        if (Input.GetKey("left shift"))
        {
            aim();
            checkFire();
        }
        
        if (Input.GetKeyUp("left shift"))
        {
            //reset arm position
            transform.GetChild(0).rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    private void checkMovement()
    {
        float speed = moveSpeed * MOVESCALE;

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
