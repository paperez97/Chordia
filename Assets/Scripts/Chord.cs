using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class Chord : MonoBehaviour
{
    //Design
    public Image nucleus;
    public Image border1;
    public Image border2;
    private Color color;
    public Text degreeText;
    public Text chordNameText;

    //Behaviour
    Toggle toggle;
    public UIElementDragger dragger;
    public bool finishedDragging = true;
    public bool activated = false;

    //Chord
    public enum DegreeList { I, II, III, IV, V, VI, VII };
    public DegreeList enumDegree;
    public enum ScaleChordTypeList { Triad, sus2, sus4, seventh};
    public ScaleChordTypeList enumScaleChordType;
    public Song song;
    public int degree;
    public List<int> chord;
    public int[] scaleChordType;
    public string chordType;

    //Methods
    public void OnOff(bool isOn)
    {

        if (isOn)
        {
            activated = true;
            song.chord = chord;
        }
        else
        {
            activated = false;
            song.chord = new List<int> { };
        }
    }

    void Start()
    {
        //Behaviour
        toggle = GetComponent<Toggle>();

        //Chord
        song = GameObject.FindGameObjectWithTag("GameController").GetComponent<Song>();
    }


    void Update()
    {
        //Behaviour
        if (dragger.dragging) { finishedDragging = false; }
        if (!dragger.dragging && !finishedDragging )
        {
            toggle.isOn = !toggle.isOn;
            finishedDragging = true;
        }

        //Chord
        chord = Music.NotesOfChord(degree, song.scale, scaleChordType);
        chordType = Music.TypeOfChord(chord, scaleChordType);
        degreeText.text = Music.ToRomanNumerals(degree);
        chordNameText.text = Music.IntToNote(chord[0]).ToString();
        if (chordNameText.text.ToLower() == chordNameText.text)
        {
            chordNameText.text = chordNameText.text.ToUpper() + '#';
        }
        chordNameText.text += chordType;
        if(activated)
        {
            song.chord = chord;
        }

        //Design
        color = Music.DegreeColor(degree);
        nucleus.color = color;
        border1.color = color;
        border2.color = color;
    }

    //I don't know why this is needed but i can't seem to change degree directly from another script
    public void SetDegree(int nDegree)
    {
        degree = nDegree;
    }
}


