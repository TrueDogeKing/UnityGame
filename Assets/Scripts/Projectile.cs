using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 6f;  // Time in seconds before the projectile is destroyed


    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // This function is called when the projectile collides with another collider
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
