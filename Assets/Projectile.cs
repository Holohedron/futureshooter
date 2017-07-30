using UnityEngine;

public class Projectile : MonoBehaviour {
    private const float MOVESCALE = 1f;

    public float velocity;
    public float duration;

    private void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward) * velocity;
    }

    private void Update ()
    {

        if (duration > 0 )
        {
            duration--;
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
