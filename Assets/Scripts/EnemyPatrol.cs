using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Rigidbody2D rb;
    BoxCollider2D gyroCollider;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gyroCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(-moveSpeed, 0f);//Moves enemy to left
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ground")
            FlipEnemy();
    }

    private void FlipEnemy()
    {
        moveSpeed = -moveSpeed;
        transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
    }
}
