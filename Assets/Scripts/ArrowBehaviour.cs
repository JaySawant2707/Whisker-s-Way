using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    [SerializeField] float arrowSpeed;
    [SerializeField] float flyTime = 3f;
    [SerializeField] float arrowDestroyDelay = 5f;
    Rigidbody2D rb;
    PlayerController playerController;
    bool isFlying = true;
    float xSpeed;
    float stoptime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = FindObjectOfType<PlayerController>();
        xSpeed = arrowSpeed * playerController.transform.localScale.x;
        stoptime = Time.time + flyTime;
    }

    void Update()
    {
        if (isFlying && stoptime <= Time.time)
        {
            Destroy(gameObject);
        }

        if (isFlying)
        {
            rb.velocity = new Vector2(xSpeed, 0f);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (isFlying)
        {
            isFlying = false;
            rb.velocity = Vector2.zero;
            transform.position = other.contacts[0].point;
            FindObjectOfType<CapsuleCollider2D>().isTrigger = true;
            Destroy(rb);
            Destroy(gameObject, arrowDestroyDelay);
        }
    }
}
