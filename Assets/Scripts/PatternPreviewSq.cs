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
        image.color = new Color(1, 1, 1, 0);
        if (savedPattern.pattern[column].Exists(element => element == row))
        {
            TurnOn();
        }
    }

    public void TurnOn()
    {
        image.color = new Color(1, 1, 1, 0.5f);
    }

    public void TurnOff()
    {
        image.color = new Color(1, 1, 1, 0);
    }
}
