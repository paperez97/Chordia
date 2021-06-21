using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Variant : MonoBehaviour
{
    public Animator variantAnimator;
    public int optionNumber;
    public string message;
    public ChordBlob chordBlob;
    public bool isOn;
    public Text text;
    public Image border;
    public Image glow;


    void Start()
    {
        optionNumber = transform.GetSiblingIndex();
    }

    void Update()
    {
        if (isOn)
        {
            variantAnimator.SetBool("variantActivated", true);
            chordBlob.variantMessage = message;
        }
        else
        {
            variantAnimator.SetBool("variantActivated", false);
        }
    }

    public void UpdateVariant()
    {
        if (message == "Mm" && chordBlob.variantMessage != "Mm")
        {
            if (chordBlob.chord.ChordType() == "")
            {
                text.text = "m";
            }
            else { text.text = "M"; }
        }

        if (message == "7" && chordBlob.variantMessage != "7")
        {
                text.text = chordBlob.chord.SeventhType();
        }

        if (message == "7?" && chordBlob.variantMessage != "7?")
        {
            if (chordBlob.chord.SeventhType() == "maj7")
            {
                text.text = "7";
            }
            else { text.text = "maj7"; }
        }
    }

}
