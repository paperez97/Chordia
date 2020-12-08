using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternEditor : MonoBehaviour
{
    public SavedPattern editingPattern;
    public Slider slider;
    public Transform cellGrid;
    public List<PatternEditorCell> cells;


    private void Start()
    {
        foreach (Transform cell in cellGrid)
        {
            cells.Add(cell.gameObject.GetComponent<PatternEditorCell>());
        }
    }

    private void Update()
    {
        //RefreshCells();
    }

    public void RefreshCells()
    {
            foreach (PatternEditorCell cell in cells)
            {
                cell.RefreshToggle();
            }
    }

    public void ChangeBeats(float newBeats)
    {
        editingPattern.beats = newBeats;
    }

    public void DestroyEditingPattern()
    {
        Destroy(editingPattern.gameObject);
    }
}

