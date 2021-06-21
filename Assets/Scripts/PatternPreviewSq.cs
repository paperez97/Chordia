using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternPreviewSq : MonoBehaviour
{

    public int column;
    public int row;
    public SavedPattern savedPattern;
    public Image image;

    void Start()
    {
        if (savedPattern.pattern[column].Exists(element => element == row))
        {
            TurnOn();
        }
    }

    public void TurnOn()
    {
        image.enabled = true;
    }

    public void TurnOff()
    {
        image.enabled = false;
    }
}
