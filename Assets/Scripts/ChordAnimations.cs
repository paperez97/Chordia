using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordAnimations : MonoBehaviour
{

    Animator chordAnimator;
    public Animator variantAnimator;
    Chord chord;
    
    // Start is called before the first frame update
    void Start()
    {
        chordAnimator = GetComponent<Animator>();
        chord = GetComponent<Chord>();
    }

    // Update is called once per frame
    void Update()
    {
            chordAnimator.SetBool("isActive", chord.activated);
            chordAnimator.SetBool("dragging", chord.dragging);
            chordAnimator.SetBool("deleting", chord.deleting);
            variantAnimator.SetBool("VariantsOpen", chord.isExpanded);
    }
}
