using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0.01f, 20.0f)]
    [SerializeField]
    private float moveSpeed = 8.0f;

    [Range(0.01f, 20.0f)]
    [SerializeField]
    private float jumpForce = 15.0f;

    private float rayLength = 1.1f;
    private float playersGravity = 4;
    public LayerMask groundLayer;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool IsWalking;
    private bool IsFacingRight = true;
    private bool isLadder = false;
    private bool isClimbing = false;
    private bool doubleJump = false;
    private int score = 0;
    private int lives = 3;
    private float timeToDie = 1.1f;
    bool hurt = false;
    private Vector2 startPosition;
    float vertical;
    void Start()
    {
        
    }

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition=transform.position;
    }

    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        animator.SetBool("IsGrounded", IsGrounded());
        if (!hurt)
        {
            Walking();
            Jump();
            Climb();
        }

        // set the yVelocity in the animator
        animator.SetFloat("yVelocity", rigidBody.velocity.y);
    }


    void Walking()
    {
        IsWalking = false;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            IsWalking = true;
            IsFacingRight = false;
            Flip();
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            IsWalking = true;
            IsFacingRight = true;
            Flip();
        }

        animator.SetBool("IsWalking", IsWalking);
    }
    bool IsGrounded()
    {
        bool grounded =Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
        if (grounded) {
            doubleJump = true;
        }
        return grounded;
    }

    void Jump()
    {
        // skakanie tylko na spacji
        bool jumpInput = Input.GetKeyDown(KeyCode.Space);
        bool grounded = IsGrounded();
        if (jumpInput && (grounded || doubleJump))
        {
            if (!grounded)
            {
                doubleJump = false;
            }
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("jumping");
        }
    }

    void Climb()
    {
        if (isLadder && vertical!=0)
        {
            isClimbing = true;
            animator.SetBool("IsClimbing", isClimbing);
        }
        else
        {
            animator.SetBool("IsClimbing", false);
        }

    }


    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = IsFacingRight ? 1 : -1;
        transform.localScale = scale;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hurt)
        {
            Points(collision);

            Ladder(collision);

            Enemy(collision);

            Spikes(collision);
        }
    }

    void Points(Collider2D collision)
    {
        if (collision.CompareTag("Bonus"))
        {
            score += 50;
            Debug.Log("Score: " + score);
            collision.gameObject.SetActive(false);
        }
    }

    void Ladder(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
            animator.SetBool("IsLadder", isLadder);
        }
    }

    void Enemy(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (transform.position.y> collision.gameObject.transform.position.y)
            {
                score += 50;
                Debug.Log("Score: " + score);
                Debug.Log("Killed an enemy");

                rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
                rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            else
            {
               LostLive();
            }
        }
    }

    void Spikes(Collider2D collision)
    {
        if (collision.CompareTag("Spikes"))
        {
            Debug.Log("Spikes");
            LostLive();
        }
    }

    void LostLive()
    {
        lives--;
        if (lives < 0)
        {
            Debug.Log("End of game");
        }
        else
        {
            hurt = true;
            Debug.Log("Lives left" + lives);
            StartCoroutine(HurtAnimation());
            
        }

    }
    private IEnumerator HurtAnimation()
    {
        animator.SetBool("IsHurt", true);
        yield return new WaitForSeconds(timeToDie);
        transform.position = startPosition;
        animator.SetBool("IsHurt", false);
        hurt = false;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
            animator.SetBool("IsLadder", isLadder);
        }
    }



    void FixedUpdate()
    {
        if (isClimbing)
        {
            rigidBody.gravityScale = 0;
            rigidBody.velocity= new Vector2(rigidBody.velocity.x, vertical*moveSpeed);

        }
        else
        {
            rigidBody.gravityScale = playersGravity;
        }
    }
}
