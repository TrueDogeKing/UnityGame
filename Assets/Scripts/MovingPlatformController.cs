using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [Range(0.01f, 20.0f)]
    [SerializeField]
    private float moveSpeed = 6.0f;

    [Range(0.01f, 20.0f)]
    [SerializeField]
    private float moveRange = 1.0f;
    private bool isMovingRight = true;

    private float startPositionX;
    // Start is called before the first frame update
    void Start()
    {
        startPositionX = this.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float positionX;
        positionX = this.transform.position.x;
        MovePath(positionX);
    }

    void MovePath(float positionX)
    {

        if (isMovingRight)
        {
            if (positionX < startPositionX + moveRange)
            {
                MoveRight();
            }
            else
            {
                isMovingRight = false;
            }
        }
        else
        {
            if (positionX > startPositionX - moveRange)
            {
                MoveLeft();
            }
            else
            {
                isMovingRight = true;
            }
        }
    }

    void MoveRight()
    {

        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    void MoveLeft()
    {

        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
}
