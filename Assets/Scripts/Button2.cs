using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button2 : MonoBehaviour
{
    [SerializeField] Animator door;
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
            
                animator.SetBool("isOn", true);
                door.SetBool("isOpened", true);
            

        }

        if (other.gameObject.CompareTag("Box"))
        {
            if (door.GetBool("isOpened") == false)
            {
                animator.SetBool("isOn", true);
                door.SetBool("isOpened", true);
            }

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!onePressOn)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (door.GetBool("isOpened") == true)
                {
                    animator.SetBool("isOn", false);
                    door.SetBool("isOpened", false);
                }
            }
        }
    }
}
