using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
        song.OnRefreshUI += Song_OnRefreshUI; ;
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
    }

    private void Song_OnRefreshUI(object sender, EventArgs e)
    {
        Refresh();
    }

    public void Refresh()
    {
        foreach (PatternEditorCell cell in patternCells)
        {
            cell.RefreshCell();
        }
        foreach (PatternEditorCell cell in drumCells)
        {
            cell.RefreshDrumsToggle();
        }
        slider.value = song.selectedPattern.beats;
    }

    public void DestroySelectedPattern()
    {
        Destroy(song.selectedPattern.gameObject);
        if (patternSaver.transform.childCount < 3)
        { bin.interactable = false; }
    }
}

