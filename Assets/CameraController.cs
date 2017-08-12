using UnityEngine;

public class CameraController : MonoBehaviour
{    
    public float followDistance;
    public float followHeight;
    public float followWidth;
    public float adjustSpeed;
    public float deathCamMoveSpeed;
    public float deathCamRotateSpeed;
    public float deathCamMaxDist;

    public int aimViewOffsetX;
    public int aimViewOffsetY;
    public int aimViewOffsetZ;

    public Vector3 rotationAxis;
    private PlayerCharacter player;
    private Vector3? deadPlayerPos = null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        if (!player.dead)
        {
            trackPlayer();
        }
        else
        {
            if (deadPlayerPos == null)
            {
                deadPlayerPos = player.transform.position;
            }
            deathCam();
        }
	}

    private void trackPlayer()
    {
        var target = player.transform;

        if (Input.GetButton("Aim") || Input.GetAxis("AimAxis") > 0)
        {
            transform.position = setFollowPoint(target.TransformPoint(new Vector3(followWidth, followHeight * 0.75f, -followDistance / 2)));
            Vector3 focalPoint = target.TransformPoint(new Vector3(aimViewOffsetX, aimViewOffsetY, aimViewOffsetZ));
            transform.LookAt(focalPoint);
        }
        else
        {
            transform.position = setFollowPoint(target.TransformPoint(new Vector3(0, followHeight, -followDistance)));
            transform.LookAt(target);
        }
    }

    private Vector3 setFollowPoint(Vector3 followPoint)
    {
        var velocity = Vector3.zero;
        return Vector3.SmoothDamp(transform.position, followPoint, ref velocity, 0.03f);
    }

    private void deathCam()
    {
        transform.LookAt(player.transform);
        Vector3 playerPos = (Vector3)deadPlayerPos; 
        var vecToPlayer = playerPos - transform.position;

        if (vecToPlayer.magnitude < deathCamMaxDist)
        {
            transform.Translate(new Vector3(-deathCamMoveSpeed * 4, deathCamMoveSpeed, 0) * Time.deltaTime, Space.World);
        }

        transform.RotateAround(playerPos, Vector3.up, deathCamRotateSpeed * Time.deltaTime);
    }
}
