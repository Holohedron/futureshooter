
using UnityEngine;

public class ShooterState : ScriptableObject, PlayerState
{
    private const float FIRETIMERMAX = 60.0f;
    private float fireTimer;

    public PlayerState HandleTransition(PlayerCharacter player)
    {
        // release aim key to go back to Melee State
        if (Input.GetKeyUp("left shift"))
        {
            return ScriptableObject.CreateInstance<MeleeState>();
        }
        return null;
    }

    public void OnEnter(PlayerCharacter player)
    {
        // zoom camera in
        fireTimer = 0;
        player.aiming = true;
    }

    public void OnExit(PlayerCharacter player)
    {
        // zoom camera out
        player.transform.GetChild(0).rotation = new Quaternion(0, 0, 0, 0);
        player.aiming = false;
    }

    public void HandleUpdate(PlayerCharacter player)
    {
        doMovement(player);
        doFire(player);
        doAim(player);
    }

    private void doMovement(PlayerCharacter player)
    {
        float speed = player.moveSpeed * Time.deltaTime;

        if (Input.GetKey("up") || Input.GetKey("w"))
        {
            player.transform.Translate(new Vector3(0, 0, speed));
        }
        else if (Input.GetKey("down") || Input.GetKey("s"))
        {
            player.transform.Translate(new Vector3(0, 0, -speed));
        }

        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            player.transform.Translate(new Vector3(-speed, 0, 0));
        }
        else if (Input.GetKey("right") || Input.GetKey("d"))
        {
            player.transform.Translate(new Vector3(speed, 0, 0));
        }
    }

    private void doFire(PlayerCharacter player)
    {
        if (fireTimer > 0)
        {
            fireTimer -= player.fireRate;
        }

        if ((Input.GetKey("space") || Input.GetMouseButton(0)) && fireTimer <= 0)
        {
            Transform gun = player.transform.GetChild(0).GetChild(0);

            Projectile p = Instantiate<Projectile>(player.projectileFab);
            p.transform.rotation = gun.rotation * Quaternion.Euler(-90, 0, 0);
            p.transform.position = gun.position + gun.up * gun.localScale.y * 2;

            fireTimer = FIRETIMERMAX;

            player.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    private void doAim(PlayerCharacter player)
    {
        Transform gun = player.transform.GetChild(0);

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
            var targetRadius = player.radius;
            targetPos.z = getZOnSphere(targetPos.x, targetPos.y, targetRadius);
            targetPos = player.transform.TransformPoint(targetPos);
            gun.LookAt(targetPos);

            // Account for the camera's rotated view
            var camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
            gun.Rotate(new Vector3(camera.rotation.eulerAngles.x * (1 - (Mathf.Abs(mousePosX) / (Screen.width))), 0, 0));
        }
    }

    private float getZOnSphere(float x, float y, float r)
    {
        return Mathf.Sqrt((r * r) - (x * x) - (y * y)) / 2;
    }
}
