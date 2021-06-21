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
    public Transform cellGridDrums2;
    public Transform cellGridDrums3;
    public List<PatternEditorCell> patternCells;
    public List<PatternEditorCell> drumCells;
    public Song song;
    public Button bin;
    public PatternSaver patternSaver;



    private void Start()
    {
        bin.interactable = false;
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
        foreach (Transform cell in cellGridDrums2)
        {
            drumCells.Add(cell.gameObject.GetComponent<PatternEditorCell>());
        }
        foreach (Transform cell in cellGridDrums3)
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
        if (patternSaver.transform.childCount < 3)
        { bin.interactable = false; }
        song.playingPattern = patternSaver.transform.GetChild(0).GetComponent<SavedPattern>();
    }
}

