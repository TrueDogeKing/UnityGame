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
    float vertical;
    void Start()
    {
        
    }

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Walking();
        Jump();

        animator.SetBool("IsGrounded", IsGrounded());

        vertical=Input.GetAxis("Vertical");
        Climb();
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
        bool jumpInput = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space);
        bool grounded = IsGrounded();
        if (jumpInput && (grounded || doubleJump))
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x,0);
            if (!grounded)
            {
                doubleJump = false;
            }
            
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
        if (collision.CompareTag("Bonus"))
        {
            score += 50;
            Debug.Log("Score: " + score);
            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
            animator.SetBool("IsLadder", isLadder);
        }


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
