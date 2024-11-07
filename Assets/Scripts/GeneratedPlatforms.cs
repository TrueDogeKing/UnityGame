using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{


    [SerializeField]
    private int platformsNum=8;

    [SerializeField]
    private float radius = 5.0f;


    private GameObject[] platforms;
    private Vector3[] positions;
    [SerializeField] private float speed = 1.0f;

    [SerializeField]
    GameObject platformPrefab;


    private float angle = 0f;

    private Vector3 parentPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        // Initialize the platforms array
        platforms = new GameObject[platformsNum];

        parentPosition = transform.position;
        // Initialize the positions array with circular placement
        positions = new Vector3[platformsNum];
        for (int i = 0; i < platformsNum; i++)
        {
            float angle = i * Mathf.PI * 2 / platformsNum;
            positions[i] = parentPosition + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
        }

        // Instantiate the platforms at the specified positions
        for (int i = 0; i < platformsNum; i++)
        {
            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);
        }




    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < platformsNum; i++)
        {
            float anglex = (i * Mathf.PI * 2 / platformsNum)+angle;
            platforms[i].transform.position = parentPosition + new Vector3(Mathf.Cos(anglex) * radius, Mathf.Sin(anglex) * radius, 0);
            if (angle >= 360f) {
                angle = 0f;
                Debug.Log("full rotation angle:" + angle);
                Debug.Log("speed:" + speed);
            }
        }
        angle += (Time.deltaTime * speed)/10;
    }
}
