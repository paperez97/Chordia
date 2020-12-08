using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordCreator : MonoBehaviour
{
    public GameObject chordPrefab;
    GameObject newChord;
    public int selectedDegree;
    public GameObject chordsContainer;

    public void CreateChord()
    {
        newChord = Instantiate(chordPrefab, chordsContainer.transform);
        newChord.GetComponent<Chord>().degree = selectedDegree;
        newChord.GetComponent<Chord>().scaleChordType = Music.triad;
    }

    public void SetSelectedDegree(int n)
    {
        selectedDegree = n;
    }
}
