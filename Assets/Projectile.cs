using UnityEngine;

public class Projectile : MonoBehaviour {
    private const float MOVESCALE = 1f;

    public float velocity;
    public float duration;
    
	private void Update ()
    {
        transform.Translate(Vector3.forward * velocity * MOVESCALE);

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
