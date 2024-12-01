using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            if (collision.gameObject.GetComponent<PlayerController>().IfKeysFound())
                Debug.Log("finnished stage");
            else
                Debug.Log("collect all keys");

    }
}
