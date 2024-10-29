using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMortility : MonoBehaviour
{
    public bool isAlive = true;
    Rigidbody2D rb;
    Animator animator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    GameSession gameSession;

    void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            gameSession.ProcessPlayerDeath();
        }

        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            gameSession.ProcessPlayerDeath();
        }
    }
}
