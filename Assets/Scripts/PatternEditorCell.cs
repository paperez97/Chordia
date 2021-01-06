using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternEditorCell : MonoBehaviour
{
    public int column;
    public int row;
    public Toggle toggle;
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
            if (row >= 0)
            {
                song.savedPattern.sqPreviewGrid.GetChild((5 - row) * 8 + column).gameObject.GetComponent<PatternPreviewSq>().TurnOn();
            }
            else
            {
                song.savedPattern.sqPreviewGridBass.GetChild(column).gameObject.GetComponent<PatternPreviewSq>().TurnOn();
            }

        }
        else
        {
            song.savedPattern.pattern[column].Remove(row);
            if (row >= 0)
            {
                song.savedPattern.sqPreviewGrid.GetChild((5 - row) * 8 + column).gameObject.GetComponent<PatternPreviewSq>().TurnOff();
            }
            else
            {
                song.savedPattern.sqPreviewGridBass.GetChild(column).gameObject.GetComponent<PatternPreviewSq>().TurnOff();
            }
        }
    }

    public void SetDrumPattern(bool isOn)
    {//Se añade a pattern si no está y se quita si está
        if (isOn)
        {
            if (!song.savedPattern.percPattern[column].Exists(element => element == row))
            { song.savedPattern.percPattern[column].Add(row); }
        }
        else
        {
            song.savedPattern.percPattern[column].Remove(row);
        }
    }

    void Start()
    {
        milestone.SetActive(row % 3 == 0 || row<0);
    }

    void Update()
    {
        playing.SetActive(song.stepPattern == (column));
        lowlight.SetActive(column > song.savedPattern.beats - 1);
    }

    public void RefreshNotesToggle()
    {
        toggle.isOn = song.savedPattern.pattern[column].Exists(element => element == row);
    }
    public void RefreshDrumsToggle()
    {
        toggle.isOn = song.savedPattern.percPattern[column].Exists(element => element == row);
    }
}
