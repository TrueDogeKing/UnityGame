using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StickThrowDirection
{
    LEFT,
    RIGHT,
}

public class BossRoomStick : MonoBehaviour
{
    private Rigidbody2D stickRigidbody;

    void Awake()
    {
        stickRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
    }

    public void Throw(StickThrowDirection throwDir, bool hit)
    {
        float dir = throwDir == StickThrowDirection.LEFT ? -1f : 1f;
        float angle = 45.2f + (hit ? 0f : 2f);
        float force = 15.8f - (hit ? 0f : 2f);

        Debug.Log("Throw():");
        Debug.Log("Throwing stick with angle: " + angle + " and force: " + force);
        Debug.Log("Hit: " + hit);

        transform.position = new Vector3(-dir * 14f, -1.9f, 0f);
        transform.rotation = Quaternion.identity;
        transform.localScale = dir * new Vector3(0.1f, 0.1f, 0.1f);
        stickRigidbody.velocity = Vector2.zero;
        stickRigidbody.angularVelocity = 0;
        Debug.Log("Position: " + transform.position);
        Debug.Log("Rotation: " + transform.rotation);
        Debug.Log("Scale: " + transform.localScale);
        Debug.Log("Velocity: " + stickRigidbody.velocity);
        Debug.Log("Angular velocity: " + stickRigidbody.angularVelocity);
        stickRigidbody.AddForce(new Vector2(dir * Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * force, ForceMode2D.Impulse);
        stickRigidbody.AddTorque(dir * 0.5f, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            BossController bossController = collision.GetComponent<BossController>();
            transform.position = new Vector3(4f, 100f, 0f);
            bossController.hasStick = true;
        }
        else if (collision.CompareTag("Player"))
        {
            BossPlayerController playerController = collision.GetComponent<BossPlayerController>();
            transform.position = new Vector3(4f, 100f, 0f);
            playerController.hasStick = true;
        }
    }
}
