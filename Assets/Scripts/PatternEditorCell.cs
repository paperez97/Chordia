using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternEditorCell : MonoBehaviour
{
    public int column;
    public int row;
    Toggle toggle;
    public Song song;
    public GameObject playing;
    public GameObject lowlight;
    public GameObject milestone;



    public void SetPattern(bool isOn)
    {//Se añade a pattern si no está y se quita si está
        if(isOn)
        {
            if (!song.savedPattern.pattern[column].Exists(element => element == row))
             { song.savedPattern.pattern[column].Add(row); }

        }
        else
        {
            song.savedPattern.pattern[column].Remove(row);
        }


    }

    void Start()
    {
        toggle = GetComponent<Toggle>();
        milestone.SetActive(row % 3 == 0);
    }

    void Update()
    {
        playing.SetActive(song.stepPattern == (column));
        lowlight.SetActive(column > song.savedPattern.beats - 1);
    }

    public void RefreshToggle()
    {

            if (song.savedPattern.pattern[column].Exists(element => element == row))
            {
                toggle.isOn = true;
            }
            else
            {
                toggle.isOn = false;
            }

    }
}
