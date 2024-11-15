using UnityEngine;

public class PlayerMortility : MonoBehaviour
{
    [SerializeField] public int playerLives = 3;
    [SerializeField] float spikeKnockbackForce = 10f;

    public bool isAlive = true;
    Rigidbody2D rb;
    Animator animator;
    GameSession gameSession;
    PlayerController playerController;

    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        gameSession = FindObjectOfType<GameSession>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if ( playerLives <= 0)
        {
            ProcessDeath();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!playerController.knockbacked)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                animator.SetTrigger("Dying");

                playerController.knockbackCounter = playerController.knockbackTime;

                if (other.transform.position.x <= transform.position.x)
                    playerController.knockbackFromRight = true;
                else
                    playerController.knockbackFromRight = false;

                playerLives--;
                gameSession.UpdateLives(playerLives);
            }

            if (other.gameObject.CompareTag("Hazards"))
            {
                animator.SetTrigger("Dying");
                rb.velocity = new Vector2(0f, spikeKnockbackForce);

                playerLives--;
                gameSession.UpdateLives(playerLives);
            }
        }
    }

    void ProcessDeath()
    {
        isAlive = false;
        gameSession.ResetGameSession();
    }
}
