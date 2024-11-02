using UnityEngine;

public class Button : MonoBehaviour
{
    public LevelExit door;
    [SerializeField] bool onePressOn = true;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (door.isOpened == false)
            {
                animator.SetBool("isOn", true);
                door.isOpened = true;
            }

        }

        if (other.gameObject.CompareTag("Box"))
        {
            if (door.isOpened == false)
            {
                animator.SetBool("isOn", true);
                door.isOpened = true;
            }

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!onePressOn)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (door.isOpened == true)
                {
                    animator.SetBool("isOn", false);
                    door.isOpened = false;
                }
            }
        }
    }
}
