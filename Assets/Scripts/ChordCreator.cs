using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChordCreator : MonoBehaviour
{
    public GameObject chordPrefab;
    GameObject newChord;
    public GameObject chordsContainer;
    public GameObject chordDeleter;
    public bool anyChordsDragging;
    public Animator crossGraphic;
    public Animator addChordButtons;
    public Toggle chordCreatorToggle;

    void Update()
    {
        //Ver si hay algún chord dragging
        bool accum = false;
        foreach(Transform chord in chordsContainer.transform)
        {
            if(chord.gameObject.GetComponent<Chord>().dragging)
            {
                accum = true;
            }
        }
        if (accum) { anyChordsDragging = true; }
        else { anyChordsDragging = false; }

        //Si lo hay, modo asesino
        if(anyChordsDragging)
        {
            chordDeleter.SetActive(true);
        }
        else { chordDeleter.SetActive(false); }
    }
    public void CreateChord(int nDegree)
    {
        Chord newChord = Instantiate(chordPrefab, chordsContainer.transform).GetComponent<Chord>();
        newChord.degree = nDegree;
        newChord.rectTransform.anchoredPosition += Vector2.right * (newChord.degree - 1) * (newChord.rectTransform.rect.width + 20) + Vector2.down * 100;
    }

    public void AnimateCross(bool isOn)
    {
        crossGraphic.SetBool("ChordCreatorOpen", isOn);
    }

    public void AnimateAddChordButtons(bool isOn)
    {
        addChordButtons.SetBool("ChordPanelOpen", isOn);
    }
}
