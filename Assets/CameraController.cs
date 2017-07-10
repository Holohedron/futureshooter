using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;
    public float followDistance;
    public float followHeight;
	
	private void Update () {
        
        transform.LookAt(target);

        if (Input.GetKey("left shift"))
        {
            transform.position = target.TransformPoint(new Vector3(0, followHeight/2, -followDistance/2));
        }
        else
        {
            transform.position = target.TransformPoint(new Vector3(0, followHeight, -followDistance));
        }
	}
}
