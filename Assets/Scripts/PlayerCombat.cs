using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] GameObject arrow;
    [SerializeField] Transform bow;

    Animator animator;
    PlayerController playerController;
    PlayerMortility playerMortility;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
        playerMortility = FindObjectOfType<PlayerMortility>();
    }

    void OnFire(InputValue value)
    {
        if (!playerMortility.isAlive) return;
        if (value.isPressed)
        {
            animator.SetTrigger("shootArrow");
        }
    }

    public void ShootArrowThroughAnimationEvent()
    {
        if (playerController.isFacingRight)
        {
            Instantiate(arrow, bow.position, transform.rotation);
        }
        else
        {
            Instantiate(arrow, bow.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
        }

    }
}
