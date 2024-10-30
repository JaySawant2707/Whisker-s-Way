using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyCharge : MonoBehaviour
{
    [SerializeField] int rayHitIndex = 2;
    [SerializeField] LayerMask detectionLayer;

    [Header("Speedzz")]
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float chargeSpeed = 10f;
    [SerializeField] float chargeDelay = 2f;

    [Header("Patrol Points")]
    [SerializeField] Transform leftPoint;
    [SerializeField] Transform rightPoint;
    [SerializeField] Transform player;

    [Header("Collision Check")]
    [SerializeField] Vector2 boxSize;
    [SerializeField] float castDistance = 2f;
    [SerializeField] LayerMask groundLayer;

    bool hasLineOfSight = false;
    bool movingLeft = true;
    bool isCharging = false;
    bool canPatrol = true;
    Rigidbody2D rb;

    float pauseCounter;

    void Awake()
    {
        rb = FindObjectOfType<Rigidbody2D>();
    }

    void Update()
    {
        if (hasLineOfSight || isCharging)//It's noooooot  Wooorkingggg........
        {
            if(!isCharging)
            pauseCounter = chargeDelay;

            isCharging = true;
            pauseCounter = -Time.deltaTime;
            if (pauseCounter < Mathf.Epsilon)
            {
                Charging();
            }
        }

        if (!isCharging)
        {
            
            Patroling();
        }
    }

    IEnumerator StunPause()
    {
        yield return new WaitForSeconds(chargeDelay);
        isCharging = false;
    }

    void Charging()
    {
        isCharging = true;

        transform.position = new Vector2(transform.position.x + Time.deltaTime * -transform.localScale.x * chargeSpeed, transform.position.y);

        bool isCollided = Physics2D.BoxCast(transform.position, boxSize, 0f, new Vector2(-transform.localScale.x, 0f), castDistance, groundLayer);

        if (isCollided)
        {
            isCharging = false;
            //StartCoroutine("StunPause");
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

    void MoveInDirection(int direction, float speed)
    {
        transform.localScale = new Vector2(direction * -1, 1f);

        transform.position = new Vector2(transform.position.x + Time.deltaTime * direction * speed, transform.position.y);
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


}
