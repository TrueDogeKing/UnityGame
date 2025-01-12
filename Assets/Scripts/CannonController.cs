using System.Collections;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] private Transform player;           // Reference to the player
    [SerializeField] private GameObject projectilePrefab; // The projectile prefab to instantiate
    [SerializeField] private Transform firePoint;         // The point from which the projectile will be fired
    [SerializeField] private float fireRate = 2f;         // Time in seconds between shots
    [SerializeField] private float projectileSpeed = 10f; // Speed of the fired projectile
    [SerializeField] private float rotationSpeed = 5f;    // Speed at which the cannon rotates

    private float fireTimer = 0f;                         // Tracks time since the last shot
    private Rigidbody2D playerRb;                         // Rigidbody2D of the player for velocity calculation


    void Start()
    {
        if (player == null)
        {
            // Automatically find the player if not set in the Inspector
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Attempt to get the player's Rigidbody2D
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        if (player != null)
        {
            AimAtPlayer();
            FireAtPlayer();
        }
    }

    void AimAtPlayer()
    {
        Vector3 predictedPosition = PredictPlayerPosition();

        // Calculate the direction to the predicted position
        Vector3 direction = predictedPosition - transform.position;


        // Calculate the rotation needed to look at the predicted position
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Smoothly rotate towards the target angle
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void FireAtPlayer()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            // Instantiate the projectile at the fire point
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Get the Rigidbody2D component of the projectile
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Set the velocity of the projectile toward the predicted position
                //rb.velocity = firePoint.right * projectileSpeed;

                //rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.velocity = new Vector2(0, 0);
                rb.velocity = firePoint.right * projectileSpeed;
            }
        }
    }

    Vector3 PredictPlayerPosition()
    {
        if (playerRb == null)
        {
            // If no Rigidbody2D is found, use the player's current position
            return player.position;
        }

        // Calculate the time it would take for the projectile to reach the player
        Vector3 directionToPlayer = player.position - transform.position;
        float distance = directionToPlayer.magnitude;
        float timeToReach = distance / projectileSpeed;

        // Predict the player's future position based on their velocity
        Vector3 futurePosition = (Vector3)playerRb.velocity * timeToReach + player.position;

        return futurePosition;
    }
}
