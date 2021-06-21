using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomBar : MonoBehaviour
{
    public Animator animator;

    public void OpenKeyPanel(bool isOn)
    {
        animator.SetBool("Key", isOn);
    }
    public void OpenPatternSmallPanel(bool isOn)
    {
        animator.SetBool("Pattern small", isOn);
    }
    public void OpenPatternBigPanel(bool isOn)
    {
        animator.SetBool("Pattern big", isOn);
    }
    public void OpenTempoPanel(bool isOn)
    {
        animator.SetBool("Tempo", isOn);
    }
    public void OpenInstrumentPanel(bool isOn)
    {
        animator.SetBool("Instrument", isOn);
    }
}
