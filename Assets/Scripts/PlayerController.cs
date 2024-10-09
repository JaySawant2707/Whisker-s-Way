using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    float playerGravity;
    Vector2 runInput;
    Rigidbody2D rb2d;
    Animator animator;
    CapsuleCollider2D myFeetCollider;
    BoxCollider2D myBodyCollider;
    public bool isAlive = true;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myFeetCollider = GetComponent<CapsuleCollider2D>();
        myBodyCollider = GetComponent<BoxCollider2D>();
        playerGravity = rb2d.gravityScale;
    }

    void Update()
    {
        if (!isAlive) return;
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
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        if (value.isPressed)
        {
            rb2d.velocity += new Vector2(0f, jumpSpeed);
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
        Vector2 playerVelocity = new Vector2(runInput.x * moveSpeed, rb2d.velocity.y);
        rb2d.velocity = playerVelocity;

        bool playerHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;    //Check if player is moving horizontally or not
        animator.SetBool("isRunning", playerHorizontalSpeed);

    }

    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rb2d.gravityScale = playerGravity;
            animator.SetBool("isClimbing", false);
            return;
        }
        rb2d.gravityScale = 0f; //player will not fall from ladder when gravity is 0
        rb2d.velocity = new Vector2(rb2d.velocity.x, runInput.y * climbSpeed);

        bool playerVerticalSpeed = Mathf.Abs(rb2d.velocity.y) > Mathf.Epsilon;  //Check if player is moving Vertically or not
        animator.SetBool("isClimbing", playerVerticalSpeed);
    }

    void Flip()
    {
        bool playerHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;    //Check if player is moving horizontally or not

        if (playerHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.velocity.x), 1f);
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
}
