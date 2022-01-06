using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChordCreator : MonoBehaviour
{
    public GameObject chordPrefab;
    GameObject newChord;
    public GameObject chordsContainer;
    public bool anyChordsDragging;
    public Animator animator;
    public Toggle chordCreatorToggle;
    public bool isOpen;
    Song song;

    void Start()
    {
        song = GameObject.FindGameObjectWithTag("Song").GetComponent<Song>();
    }

    void Update()
    {
        animator.SetBool("Dragging", anyChordsDragging);
    }
    public void CreateChord(int nDegree)
    {
        foreach(ChordBlob chord in song.chordBlobsOnTheTable)
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
            bool isTaken = false;
            foreach (ChordBlob chordBlob in song.chordBlobsOnTheTable)
            {
                if (chordBlob.rectTransform.position.x == pos.x && chordBlob.rectTransform.position.y == pos.y) isTaken = true;
            }
            if (!isTaken)
            {
                newChord.PlaceOnGrid(pos, chordsContainer.GetComponent<HexGrid>().gridPositions);
                break;
            }
        }
        song.chordBlobsOnTheTable.Add(newChord);
    }

    public void SetIsOpen(bool nIsOpen)
    {
        animator.SetBool("ChordCreatorOpen", nIsOpen);
    }
}
