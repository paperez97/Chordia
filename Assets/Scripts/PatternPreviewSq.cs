using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternPreviewSq : MonoBehaviour
{

    public int column;
    public int row;
    public SavedPattern savedPattern;
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
        image.color = new Color(1, 1, 1, 0);

    }

    void Update()
    {
        if (savedPattern.pattern[column].Exists(element => element == row))
        {
            image.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            image.color = new Color(1, 1, 1, 0);
        }

    }
}
