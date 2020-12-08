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
        switch (enumDegree)
        {
            case DegreeList.I:
                degree = 1;
                break;
            case DegreeList.II:
                degree = 2;
                break;
            case DegreeList.III:
                degree = 3;
                break;
            case DegreeList.IV:
                degree = 4;
                break;
            case DegreeList.V:
                degree = 5;
                break;
            case DegreeList.VI:
                degree = 6;
                break;
            case DegreeList.VII:
                degree = 7;
                break;
            default:
                break;
        }

        switch (enumScaleChordType)
        {
            case ScaleChordTypeList.Triad:
                scaleChordType = Music.triad;
                break;
            case ScaleChordTypeList.sus2:
                scaleChordType = Music.sus2;
                break;
            case ScaleChordTypeList.sus4:
                scaleChordType = Music.sus4;
                break;
            case ScaleChordTypeList.seventh:
                scaleChordType = Music.seventh;
                break;
            default:
                break;
        }
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

}


