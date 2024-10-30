using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movements")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float climbSpeed = 3f;

    [Header("Jump Settings")]
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float fallMultiplier = 5f;
    [SerializeField] float cayoteTime = 0.3f;
    [SerializeField] float cayoteTimeCounter;
    [SerializeField] float colliderUpTime = 0.2f;
    [SerializeField] float colliderUpCounter;

    [Header("Ground Cheak")]
    [SerializeField] float castDistance;
    [SerializeField] Vector2 boxSize;
    [SerializeField] LayerMask groundLayer;

    public bool isFacingRight = true;
    float playerGravity;
    bool isGrounded = false;
    bool canDoubleJump = false;
    Vector2 runInput;
    Rigidbody2D rb;
    Animator animator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    PlayerMortility playerMortility;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        playerMortility = FindObjectOfType<PlayerMortility>();
    }

    void Start()
    {
        playerGravity = rb.gravityScale;
    }

    void Update()
    {
        if (!playerMortility.isAlive) return;

        GroundCheck();
        Run();
        Flip();
        CalculateJump();
        ClimbLadder();
    }

    void CalculateJump()
    {
        //custom falling speed
        if (rb.velocity.y < 0)
        {
            rb.velocity += Physics2D.gravity * fallMultiplier * Time.deltaTime;
        }

        //cayote time counter
        if (isGrounded)
        {
            if (colliderUpCounter < Mathf.Epsilon)
            myFeetCollider.offset = new Vector2(0, -0.6281902f);//move collider down on landed..=*=This is hard coded=*=

            cayoteTimeCounter = cayoteTime;

            if (animator.GetBool("isJumping"))
                animator.SetBool("isJumping", false);
        }
        else
        {
            cayoteTimeCounter -= Time.deltaTime;
            
            if (!animator.GetBool("isJumping"))
                animator.SetBool("isJumping", true);
        }

        colliderUpCounter -= Time.deltaTime;
    }

    void OnMove(InputValue value)//Method for new Input System
    {
        runInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)//Method for new Input System
    {
        if (!playerMortility.isAlive) return;

        if (value.isPressed)
        {
            ProcessJump();
        }
    }

    void ProcessJump()
    {
        animator.SetBool("doubleJumping", false);
        if (isGrounded || (!isGrounded && cayoteTimeCounter > Mathf.Epsilon))
        {
            DoJump();
            canDoubleJump = true;
        }
        else if (canDoubleJump)
        {
            DoJump();
            animator.SetBool("doubleJumping", true);
            canDoubleJump = false;
        }
    }

    void DoJump()
    {
        colliderUpCounter = colliderUpTime;
        myFeetCollider.offset = new Vector2(0, -0.25f);//Move collider up while jumping..=*=This is hard coded=*=
        rb.velocity = Vector2.zero;//this will avoide adding previous velocity into this jump
        rb.velocity += Vector2.up * jumpSpeed;
        cayoteTimeCounter = 0;
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

    void GroundCheck()
    {
        isGrounded = Physics2D.BoxCast(transform.position, boxSize, 0f, -transform.up, castDistance, groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }
}
