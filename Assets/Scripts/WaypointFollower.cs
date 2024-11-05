using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypoint = 0;
    [Range(0.01f, 20.0f)]
    [SerializeField]
    private float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, speed * Time.deltaTime);
        if (Vector2.Distance(waypoints[currentWaypoint].transform.position, transform.position)<0.1f)
        {
            currentWaypoint=(currentWaypoint+1)%waypoints.Length;
        }

    }


}
