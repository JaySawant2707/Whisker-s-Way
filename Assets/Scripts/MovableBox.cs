using UnityEngine;

public class MovableBox : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D cd;

    void Awake()
    {
       rb = GetComponent<Rigidbody2D>(); 
       cd = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!cd.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            rb.velocity  = new Vector2(rb.velocity.x * 0f, rb.velocity.y);
        }
    }
}
