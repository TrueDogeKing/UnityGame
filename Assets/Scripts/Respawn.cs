using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Sprite newSprite; // The new sprite to change to
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    // Start is called before the first frame update
    void Start()
    {
        // Get the SpriteRenderer component attached to this object
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // This method will be called when the player collides with the object
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            spriteRenderer.sprite = newSprite;
        }
    }
}
