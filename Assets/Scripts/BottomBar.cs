using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomBar : MonoBehaviour
{
    public Animator animator;

    public void OpenHome(bool isOn)
    {
        animator.SetBool("Home", isOn);
    }
    public void OpenSettings(bool isOn)
    {
        animator.SetBool("Settings", isOn);
    }
    public void OpenSongs(bool isOn)
    {
        animator.SetBool("Songs", isOn);
    }

    public void PatternEditor(bool isOn)
    {
        animator.SetBool("PatternEditor", isOn);
    }
}
