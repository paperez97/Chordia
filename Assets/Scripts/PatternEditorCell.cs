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
            if (!song.selectedPattern.pattern[column].Exists(element => element == row))
             { song.selectedPattern.pattern[column].Add(row); }
            if (row >= 0)
            {
                song.selectedPattern.sqPreviewGrid.GetChild((5 - row) * 8 + column).gameObject.GetComponent<PatternPreviewSq>().TurnOn();
            }
            else
            {
                song.selectedPattern.sqPreviewGridBass.GetChild(column).gameObject.GetComponent<PatternPreviewSq>().TurnOn();
            }

        }
        else
        {
            song.selectedPattern.pattern[column].Remove(row);
            if (row >= 0)
            {
                song.selectedPattern.sqPreviewGrid.GetChild((5 - row) * 8 + column).gameObject.GetComponent<PatternPreviewSq>().TurnOff();
            }
            else
            {
                song.selectedPattern.sqPreviewGridBass.GetChild(column).gameObject.GetComponent<PatternPreviewSq>().TurnOff();
            }
        }
    }

    public void SetDrumPattern(bool isOn)
    {//Se añade a pattern si no está y se quita si está
        if (isOn)
        {
            if (!song.selectedPattern.percPattern[column].Exists(element => element == row))
            { song.selectedPattern.percPattern[column].Add(row); }
        }
        else
        {
            song.selectedPattern.percPattern[column].Remove(row);
        }
    }

    void Start()
    {
        milestone.SetActive(row % 3 == 0 || row<0);
    }

    void Update()
    {
        playing.SetActive(song.stepPattern == (column));
        lowlight.SetActive(column > song.selectedPattern.beats - 1);
    }

    public void RefreshNotesToggle()
    {
        toggle.isOn = song.selectedPattern.pattern[column].Exists(element => element == row);
    }
    public void RefreshDrumsToggle()
    {
        toggle.isOn = song.selectedPattern.percPattern[column].Exists(element => element == row);
    }
}
