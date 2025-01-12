using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 6f;  // Time in seconds before the projectile is destroyed
    private PlayerController playerController;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }

        Destroy(gameObject, lifetime);
    }

    // This function is called when the projectile collides with another collider
    void Update()
    {
        // Check if the player is dead and destroy the projectile
        if (playerController != null && playerController.IsHurt())
        {
            Destroy(gameObject);
        }
    }
}
