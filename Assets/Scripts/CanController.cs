using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanController : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            BossController bossController = collision.GetComponent<BossController>();
            transform.position = new Vector3(-4f, 100f, 0f);
            bossController.hasCan = true;
        }
        else if (collision.CompareTag("Player"))
        {
            BossPlayerController playerController = collision.GetComponent<BossPlayerController>();
            transform.position = new Vector3(-4f, 100f, 0f);
            playerController.hasCan = true;
        }
    }
}
