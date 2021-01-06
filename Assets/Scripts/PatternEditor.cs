using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternEditor : MonoBehaviour
{
    public Slider slider;
    public Transform cellGrid;
    public Transform cellGridBass;
    public Transform cellGridDrums;
    public List<PatternEditorCell> patternCells;
    public List<PatternEditorCell> drumCells;
    public Song song;



    private void Start()
    {
        foreach (Transform cell in cellGrid)
        {
            patternCells.Add(cell.gameObject.GetComponent<PatternEditorCell>());
        }
        foreach (Transform cell in cellGridBass)
        {
            patternCells.Add(cell.gameObject.GetComponent<PatternEditorCell>());
        }
        foreach (Transform cell in cellGridDrums)
        {
            drumCells.Add(cell.gameObject.GetComponent<PatternEditorCell>());
        }
        Refresh();
    }

    public void Refresh()
    {
        foreach (PatternEditorCell cell in patternCells)
        {
            cell.RefreshNotesToggle();
        }
        foreach (PatternEditorCell cell in drumCells)
        {
            cell.RefreshDrumsToggle();
        }
        slider.value = song.savedPattern.beats;
    }

    public void DestroyEditingPattern()
    {
        Destroy(song.savedPattern.gameObject);
    }
}

