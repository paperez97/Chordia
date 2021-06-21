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
            newChord.rectTransform.anchoredPosition += Vector2.right * (newChord.degree - 1) * 60 + Vector2.down * 100;
    }

    public void SetIsOpen(bool nIsOpen)
    {
        animator.SetBool("ChordCreatorOpen", nIsOpen);
    }
}
