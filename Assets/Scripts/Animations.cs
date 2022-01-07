using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    public Animator animator;
    public void OpenHome(bool isOn)
    {
        if (isOn) animator.SetTrigger("Home");
    }
    public void OpenSettings(bool isOn)
    {
        if (isOn) animator.SetTrigger("Settings");
    }
    public void OpenSongs(bool isOn)
    {
        if (isOn) animator.SetTrigger("Songs");
    }

    public void PatternEditor(bool isOn)
    {
        if(isOn) animator.SetTrigger("PatternEditor");
        else animator.SetTrigger("Home");
    }
}
