using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternEditor : MonoBehaviour
{
    public SavedPattern savedPattern;
    public Slider slider;
    public Transform cellGrid;
    public List<PatternEditorCell> cells;
    Song song;



    private void Start()
    {
        song = GameObject.FindGameObjectWithTag("Song").GetComponent<Song>();
        foreach (Transform cell in cellGrid)
        {
            cells.Add(cell.gameObject.GetComponent<PatternEditorCell>());
        }
    }

    void Update()
    {
        savedPattern = song.savedPattern;
        slider.value = song.savedPattern.beats;
        RefreshCells();
    }

    public void RefreshCells()
    {
        foreach (PatternEditorCell cell in cells)
        {
            cell.RefreshToggle();
        }
    }

    public void DestroyEditingPattern()
    {
        Destroy(savedPattern.gameObject);
    }
}

