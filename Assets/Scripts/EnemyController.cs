using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [Range(0.01f, 20.0f)]
    [SerializeField]
    private float moveSpeed = 6.0f;

    [Range(0.01f, 20.0f)]
    [SerializeField]
    private float moveRange = 1.0f;
    private Animator animator;

    [SerializeField]
    private bool isMovingRight = true;


    private float startPositionX;
    private float timeToDie = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        //rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPositionX = this.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float positionX ;
        positionX=this.transform.position.x;
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
        Flip();
    }

    void MoveLeft()
    {

        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        Flip();
    }
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = isMovingRight ? 1 : -1;
        transform.localScale = scale;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (transform.position.y < collision.gameObject.transform.position.y)
            {
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
            }
        }
    }


    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(timeToDie); 

        gameObject.SetActive(false);
    }
}
