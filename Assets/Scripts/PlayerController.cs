using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movements")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbSpeed = 3f;

    [Header("Projectile")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] Transform bulletTarget;

    [Header("Ground Cheak")]
    [SerializeField] float castDistance;
    [SerializeField] Vector2 boxSize;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] int totalJumps = 2;
    public bool isFacingRight = true;
    bool isAlive = true;
    float playerGravity;
    bool isGrounded = false;
    bool canDoubleJump = false;
    Vector2 runInput;
    Rigidbody2D rb;
    Animator animator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        playerGravity = rb.gravityScale;
    }

    void Update()
    {
        if (!isAlive) return;

        GroundCheck();
        Run();
        Flip();
        ClimbLadder();
        Die();

        if (totalJumps < 2 && isGrounded)
        {
            totalJumps = 2;
        }
    }

    void OnMove(InputValue value)
    {
        runInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) return;

        if (value.isPressed)
        {
            Jump();
        }
    }

    void Jump()
    {
        if (totalJumps > 0 && isGrounded)
        {
            rb.velocity += Vector2.up * jumpSpeed;
            totalJumps--;
            canDoubleJump = true;
        }
        else if (canDoubleJump)
        {
            rb.velocity = Vector2.zero;
            rb.velocity += Vector2.up * jumpSpeed;
            totalJumps--;
            canDoubleJump = false;
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) return;
        if (value.isPressed)
        {
            Instantiate(bullet, gun.position, transform.rotation);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(runInput.x * moveSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;

        bool isPlayerMovingHorizontally = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;    //Check if player is moving or not (horizontally)
        animator.SetBool("isRunning", isPlayerMovingHorizontally);

    }

    void ClimbLadder()
    {
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rb.gravityScale = playerGravity;
            animator.SetBool("isClimbing", false);
            return;
        }
        rb.gravityScale = 0f; //player will not fall from ladder when gravity is 0.
        rb.velocity = new Vector2(rb.velocity.x, runInput.y * climbSpeed);

        bool isPlayerMovingVertically = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;  //Check if player is moving or not (Vertically)
        animator.SetBool("isClimbing", isPlayerMovingVertically);
    }

    void Flip()
    {
        bool playerHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;    //Check if player is moving or not (horizontally)

        if (playerHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }

        if (transform.localScale.x < 0)
        {
            isFacingRight = false;
        }
        else
        {
            isFacingRight = true;
        }
    }

    void Die()
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics2D.BoxCast(transform.position, boxSize, 0f, -transform.up, castDistance, groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }
}
