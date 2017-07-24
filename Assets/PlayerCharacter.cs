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
    public float radius = Screen.width/2;

    public Projectile projectileFab;
    public Texture2D crosshairImage;

    private float fireTimer = 0;
    private bool aiming = false;
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
            aiming = true;
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
            aiming = false;
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

    private void OnGUI()
    {
        if (aiming)
        {
            float xMin = Screen.width - (Screen.width - Input.mousePosition.x) - (crosshairImage.width / 2);
            float yMin = (Screen.height - Input.mousePosition.y) - (crosshairImage.height / 2);
            GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width, crosshairImage.height), crosshairImage);
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
        Transform gun = transform.GetChild(0);

        RaycastHit hitInfo;
        Vector3 targetPos;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, 1000))
        {
            targetPos = hitInfo.point;
            gun.LookAt(targetPos);
        }
        else
        {
            // Roughly aim in the right direction if ray hits nothing
            targetPos = Input.mousePosition;
            targetPos.x -= Screen.width / 2;
            targetPos.y -= Screen.height / 2;
            var mousePosX = targetPos.x;

            // Look at a spot around an imaginary sphere around the character
            var targetRadius = radius;
            targetPos.z = getZOnSphere(targetPos.x, targetPos.y, targetRadius);
            targetPos = transform.TransformPoint(targetPos);
            gun.LookAt(targetPos);

            // Account for the camera's rotated view
            var camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
            gun.Rotate(new Vector3(camera.rotation.eulerAngles.x * (1 - (Mathf.Abs(mousePosX)/(Screen.width))), 0, 0));
        }
        
        //Debug info
        transform.InverseTransformPoint(targetPos);
        mouseX = targetPos.x;
        mouseY = targetPos.y;
        mouseZ = targetPos.z;
    }

    private float getZOnSphere(float x, float y, float r)
    {
        return Mathf.Sqrt((r * r) - (x * x) - (y * y))/2;
    }
}
