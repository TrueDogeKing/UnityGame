using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickAnimation : MonoBehaviour
{
    public float speed = 2.0f;       // Speed of the wiggling effect
    public float amplitude = 15.0f;  // Amplitude of rotation (in degrees)
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Sin(Time.time * speed) * amplitude;

        // Apply the rotation to the Z axis
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
