using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    Rigidbody2D rb;
    PlayerController playerController;
    float xSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = FindObjectOfType<PlayerController>();
        xSpeed = bulletSpeed * playerController.transform.localScale.x;
    }

    void Update()
    {
        rb.velocity =  new Vector2 (xSpeed, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
