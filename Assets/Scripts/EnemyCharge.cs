using UnityEngine;

public class EnemyCharge : MonoBehaviour
{
    [SerializeField] int rayHitIndex = 2;
    [SerializeField] LayerMask detectionLayer;

    [Header("Speeds")]
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float chargeSpeed = 10f;

    [Header("Delays")]
    [SerializeField] float powerUpDelay = 2f;
    [SerializeField] float stunDelay = 2f;

    [Header("Patrol Points")]
    [SerializeField] Transform leftPoint;
    [SerializeField] Transform rightPoint;

    [Header("Target")]
    [SerializeField] Transform target;

    [Header("Collision Check")]
    [SerializeField] Vector2 boxSize;
    [SerializeField] float castDistance = 2f;
    [SerializeField] LayerMask groundLayer;

    bool hasLineOfSight = false;
    bool movingLeft = true;
    bool isCharging = false;
    bool isStuned = false;
    float powerUpCounter;
    float stunCounter;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (hasLineOfSight || isCharging)//put isCharging bcz enemy should be charging foreward even if line of sight breaks.
        {
            isCharging = true;
            powerUpCounter -= Time.deltaTime;
            animator.SetBool("isPatroling", false);
            animator.SetBool("isPowerUp", true);

            if (powerUpCounter < Mathf.Epsilon)
            {
                animator.SetBool("isPowerUp", false);
                Charging();
            }
        }

        if (!isCharging)
        {
            powerUpCounter = powerUpDelay;
            stunCounter = stunDelay;
            animator.SetBool("isPatroling", true);
            Patroling();
        }
    }

    void Charging()
    {
        isCharging = true;

        if (!isStuned)
        {
            animator.SetBool("isCharging", true);
            transform.position = new Vector2(transform.position.x + Time.deltaTime * -transform.localScale.x * chargeSpeed, transform.position.y);
        }

        bool isCollided = Physics2D.BoxCast(transform.position, boxSize, 0f, new Vector2(-transform.localScale.x, 0f), castDistance, groundLayer);

        if (isCollided)
        {
            animator.SetBool("isCharging", false);
            isStuned = true;
            stunCounter -= Time.deltaTime;

            animator.SetBool("isStuned", true);

            if (stunCounter < Mathf.Epsilon)
            {
                animator.SetBool("isStuned", false);
                isCharging = false;
                isStuned = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - new Vector3(transform.localScale.x, 0f) * castDistance, boxSize);
    }

    void Patroling()
    {
        if (movingLeft)
        {
            if (transform.position.x >= leftPoint.transform.position.x)
            {
                MoveInDirection(-1, walkSpeed);
            }
            else
            {
                ChangeDirection();
            }
        }
        else
        {
            if (transform.position.x <= rightPoint.transform.position.x)
            {
                MoveInDirection(1, walkSpeed);
            }
            else
            {
                ChangeDirection();
            }
        }
    }

    void ChangeDirection()
    {
        movingLeft = !movingLeft;
    }   

    void FixedUpdate()
    {
        RaycastHit2D[] ray = Physics2D.RaycastAll(transform.position - new Vector3(0f, 0.2f, 0f), new Vector2(-transform.localScale.x, 0f), Mathf.Infinity);

        if (ray[rayHitIndex].collider.CompareTag("Player"))
        {
            hasLineOfSight = true;
        }
        else
        {
            hasLineOfSight = false;
        }

        if (hasLineOfSight)
        {
            Debug.DrawRay(transform.position - new Vector3(0f, 0.2f, 0f), new Vector2(-transform.localScale.x, 0f) * 10, Color.green);
        }
        else
        {
            Debug.DrawRay(transform.position - new Vector3(0f, 0.2f, 0f), new Vector2(-transform.localScale.x, 0f) * 10, Color.red);
        }
    }
    
    void MoveInDirection(int direction, float speed)
    {
        transform.localScale = new Vector2(direction * -1, 1f);

        transform.position = new Vector2(transform.position.x + Time.fixedDeltaTime * direction * speed, transform.position.y);
    }

}
