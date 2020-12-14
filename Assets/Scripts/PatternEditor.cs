using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternEditor : MonoBehaviour
{
    public Slider slider;
    public Transform cellGrid;
    public List<PatternEditorCell> cells;
    public Song song;



    private void Start()
    {
        foreach (Transform cell in cellGrid)
        {
            cells.Add(cell.gameObject.GetComponent<PatternEditorCell>());
        }
        Refresh();
    }

    public void Refresh()
    {
        foreach (PatternEditorCell cell in cells)
        {
            cell.RefreshToggle();
        }
        slider.value = song.savedPattern.beats;
    }

    public void DestroyEditingPattern()
    {
        Destroy(song.savedPattern.gameObject);
    }
}

