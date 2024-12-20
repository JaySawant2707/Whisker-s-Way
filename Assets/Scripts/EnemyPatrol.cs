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
        rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);//Moves enemy to left
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
            FlipEnemy();
    }

    private void FlipEnemy()
    {
        moveSpeed = -moveSpeed;
        transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
    }
}
