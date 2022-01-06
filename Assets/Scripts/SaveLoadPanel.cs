using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadPanel : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetIsOpen(bool isOpen)
    {
        animator.SetBool("isSave&LoadOpen", isOpen);
    }
}
