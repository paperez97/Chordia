using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordDeleter : MonoBehaviour
{
    public Animator animator;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Chord")
        {
            animator.SetBool("Deleting", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Chord")
        {
            animator.SetBool("Deleting", false);
        }
    }
}
