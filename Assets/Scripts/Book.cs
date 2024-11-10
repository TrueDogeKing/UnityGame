using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float destroyHeight = -25.0f;
    private float rotationSpeed;

    void Start()
    {
        rotationSpeed = Random.Range(-200f, 200f);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        if (transform.position.y < destroyHeight)
        {
            Destroy(gameObject);
        }
    }
}
