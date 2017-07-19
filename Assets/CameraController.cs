using UnityEngine;

public class CameraController : MonoBehaviour
{    
    public float followDistance;
    public float followHeight;
    public float deathCamMoveSpeed;
    public float deathCamRotateSpeed;
    public float deathCamMaxDist;
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

        transform.LookAt(target);

        if (Input.GetKey("left shift"))
        {
            transform.position = target.TransformPoint(new Vector3(0, followHeight / 2, -followDistance / 2));
        }
        else
        {
            transform.position = target.TransformPoint(new Vector3(0, followHeight, -followDistance));
        }
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
