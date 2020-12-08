using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordAnimations : MonoBehaviour
{

    Animator animator;
    UIElementDragger dragger;
    Chord chord;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        dragger = GetComponent<UIElementDragger>();
        chord = GetComponent<Chord>();
    }

    // Update is called once per frame
    void Update()
    {
        if (chord.activated)
        {
            animator.SetBool("isActive", true);

        }
        else
        {
            animator.SetBool("isActive", false);
        }

        if (dragger.dragging)
        {
            animator.SetBool("dragging", true);
        }
        else
        {
            animator.SetBool("dragging", false);
        }
   }
}
