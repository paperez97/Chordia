using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordBlobManager : MonoBehaviour
{
    public GameObject chordPrefab;
    GameObject newChord;
    public GameObject chordsContainer;
    public bool anyChordsDragging;
    Song song;
    public HexGrid hexGrid;

    void Start()
    {
        song = GameObject.FindGameObjectWithTag("Song").GetComponent<Song>();
    }

    public void ToggleChord(int degree)
    {
        foreach (ChordBlob chord in song.chordBlobsOnTheTable)
        {
            if (chord.degree == degree)
            {
                DeleteChord(degree);
                return;
            }
        }
        
        CreateChord(degree);
    }
    public void DeleteChord(int degree)
    {
        foreach (ChordBlob chord in song.chordBlobsOnTheTable)
        {
            if (chord.degree == degree)
            {
                Destroy(chord.gameObject);
                return;
            }
        }
    }

    public void CreateChord(int nDegree)
    {
        foreach (ChordBlob chord in song.chordBlobsOnTheTable)
        {
            if (chord.degree == nDegree)
            {
                chord.gameObject.GetComponent<Animator>().SetTrigger("alreadyThere");
                return;
            }
        }
        ChordBlob newChord = Instantiate(chordPrefab, chordsContainer.transform).GetComponent<ChordBlob>();
        newChord.degree = nDegree;
        foreach (Vector2 pos in chordsContainer.GetComponent<HexGrid>().gridPositions)
        {
            bool isAvailable = true;
            newChord.transform.position = hexGrid.ClosestGridPosition(pos);
            foreach (ChordBlob chordBlob in song.chordBlobsOnTheTable)
            {
                if (newChord.GetComponent<Collider2D>().IsTouching(chordBlob.GetComponent<Collider2D>()))
                {
                    isAvailable = false;
                    break;
                }
            }
            if (isAvailable) break;     
        }
        song.chordBlobsOnTheTable.Add(newChord);
    }
}
