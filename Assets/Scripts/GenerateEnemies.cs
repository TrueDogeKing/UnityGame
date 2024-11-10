using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{

    [SerializeField]
    private int enemiesNum = 5;

    [SerializeField]
    private float distance = 2.5f;


    private GameObject[] enemies;
    private Vector3[] positions;

    private Vector3 parentPosition;

    [SerializeField]
    GameObject eaglePrefab;



    // Start is called before the first frame update
    private void OnEnable()
    {
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.OnPlayerDeath += Respawn;
    }

    private void OnDisable()
    {
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.OnPlayerDeath -= Respawn;
    }

    void Start()
    {
        enemies = new GameObject[enemiesNum];

        parentPosition = transform.position;
        // Initialize the positions array with circular placement
        positions = new Vector3[enemiesNum];
        for (int i = 0; i < enemiesNum; i++)
        {
            positions[i] = parentPosition + new Vector3(i*distance, 0, 0);
        }

        // Instantiate the platforms at the specified positions
        for (int i = 0; i < enemiesNum; i++)
        {
            enemies[i] = Instantiate(eaglePrefab, positions[i], Quaternion.identity);
        }



    }

    // Update is called once per frame
    void Update()
    {
        


    }

    void Respawn()
    {
        for (int i = 0; i < enemiesNum; i++)
        {
            Destroy(enemies[i]);
        }
        for (int i = 0; i < enemiesNum; i++)
        {
            enemies[i] = Instantiate(eaglePrefab, positions[i], Quaternion.identity);
        }
    }
}
