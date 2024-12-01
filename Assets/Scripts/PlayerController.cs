using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [Range(0.01f, 20.0f)]
    [SerializeField]
    private float moveSpeed = 8.0f;

    [Range(0.01f, 20.0f)]
    [SerializeField]
    private float jumpForce = 15.0f;

    [SerializeField]
    private AudioClip bonusSound;

    [SerializeField]
    private AudioClip deathSound;

    [SerializeField]
    private AudioClip keySound;

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
    private int lifes = 3;
    private float timeToDie = 1.1f;
    // should be deleted?
    private int foundKeys = 0;

    private const int keysNumber= 3;
    bool hurt = false;
    private Vector2 startPosition;
    float vertical;
    public event Action OnPlayerDeath;

    private AudioSource source;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition=transform.position;
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (GameManager.instance.currentGameState != GameManager.GameState.GAME)
            return;
        if (hurt)
            return;

        Status();

        Walking();
        Jump();
        Climb();
    }

    void Status()
    {
        vertical = Input.GetAxis("Vertical");
        animator.SetBool("IsGrounded", IsGrounded());
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

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = IsFacingRight ? 1 : -1;
        transform.localScale = scale;
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


    

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hurt)
            return;

        Points(collision);

        Ladder(collision);

        Enemy(collision);

        Spikes(collision);

        Key(collision);

        LifePotion(collision);

        MovingPlatform(collision);


    }

    void Points(Collider2D collision)
    {
        if (collision.CompareTag("Bonus"))
        {
            score += 50;
            GameManager.instance.UpdatePoints(score);
            collision.gameObject.SetActive(false);
            source.PlayOneShot(bonusSound, AudioListener.volume);
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
                GameManager.instance.UpdatePoints(score);
                GameManager.instance.UpdateEnemies();
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
                rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            else
            {
               LostLife();
            }
        }
    }

    void Spikes(Collider2D collision)
    {
        if (collision.CompareTag("Spikes"))
        {
            Debug.Log("Spikes");
            LostLife();
        }
    }

    void Key(Collider2D collision)
    {
        if (collision.CompareTag("Key"))
        {
            int id = GetColorId(collision);
            GameManager.instance.AddKeys(id);
            collision.gameObject.SetActive(false);
            source.PlayOneShot(keySound, AudioListener.volume);
        }
    }
    void LifePotion(Collider2D collision)
    {
        if (collision.CompareTag("LifePotion"))
        {
            lifes++;
            GameManager.instance.UpdatePlayerLifes(lifes);
            Debug.Log("lifes: " + lifes);
            collision.gameObject.SetActive(false);
        }
    }
    void MovingPlatform(Collider2D collision)
    {
        if (collision.CompareTag("MovingPlatform"))
        {
            transform.SetParent(collision.transform);
        }
    }

    void LostLife()
    {
        source.PlayOneShot(deathSound, AudioListener.volume);
        lifes--;
        GameManager.instance.UpdatePlayerLifes(lifes);
        if (lifes < 0)
        {
            Debug.Log("End of game");
        }
        else
        {
            hurt = true;
            Debug.Log("lifes left" + lifes);
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
        OnPlayerDeath?.Invoke(); 
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
            animator.SetBool("IsLadder", isLadder);
        }
        PlatformExit(collision);
    }
    void PlatformExit(Collider2D collision)
    {
        if (collision.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);

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

    int GetColorId(Collider2D collision)
    {
        SpriteRenderer spriteRenderer = collision.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            return 0;

        Color color = spriteRenderer.color;

        if (color == Color.white)
            return 0;
        if (color == Color.red)
            return 1;
        if (color == new Color(1f, 1f, 0f))
            return 2;

        return 0; 
    }

     public bool IfKeysFound() 
    {
        return foundKeys == keysNumber;
    }

}
