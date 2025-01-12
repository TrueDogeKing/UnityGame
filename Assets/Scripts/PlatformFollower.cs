using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFollower : MonoBehaviour
{
    [SerializeField] private GameObject player;       // Reference to the player object
    [SerializeField] private Vector3 platformOffset;  // Offset from the player's position
    [SerializeField] private float followSpeed = 5f;  // Speed at which the platform follows the player

    // Start is called before the first frame update
    void Start()
    {
        // If the player reference isn't set in the inspector, find the player by tag
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;

        Vector3 targetPos = player.transform.position + platformOffset;

        // Smoothly move the platform towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
}
