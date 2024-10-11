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

    bool isAlive = true;
    float playerGravity;
    bool isGrounded = false;
    public bool isFacingRight = true;
    Vector2 runInput;
    Rigidbody2D rb;
    Animator animator;
    CapsuleCollider2D myFeetCollider;
    BoxCollider2D myBodyCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myFeetCollider = GetComponent<CapsuleCollider2D>();
        myBodyCollider = GetComponent<BoxCollider2D>();
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
    }

    void OnMove(InputValue value)
    {
        runInput = value.Get<Vector2>();
        Debug.Log(runInput);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) return;

        if (!isGrounded) { return; }

        if (value.isPressed)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.velocity += new Vector2(0f, jumpSpeed);
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

        bool playerHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;    //Check if player is moving or not (horizontally)
        animator.SetBool("isRunning", playerHorizontalSpeed);

    }

    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rb.gravityScale = playerGravity;
            animator.SetBool("isClimbing", false);
            return;
        }
        rb.gravityScale = 0f; //player will not fall from ladder when gravity is 0.
        rb.velocity = new Vector2(rb.velocity.x, runInput.y * climbSpeed);

        bool playerVerticalSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;  //Check if player is moving or not (Vertically)
        animator.SetBool("isClimbing", playerVerticalSpeed);
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
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    void GroundCheck()
    {
        isGrounded = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }
}
