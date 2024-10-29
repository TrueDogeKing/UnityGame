using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float cameraOffset = 2f;
    [SerializeField] private float cameraSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y + cameraOffset, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, cameraSpeed * Time.deltaTime);
    }
}
