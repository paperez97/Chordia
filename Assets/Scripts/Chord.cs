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
    public UIElementDragger dragger;
    public bool activated = false;
    public bool deleting = false;

    //Chord
    public Song song;
    public List<int> chord;
    public int degree;
    public int[] scaleChordType;
    public int[][] scaleChordTypes;
    public int[] defaultScaleChordType;

    //Methods
    void Start()
    {
        //Chord
        defaultScaleChordType = Music.triad;
        song = GameObject.FindGameObjectWithTag("GameController").GetComponent<Song>();
        song.chordsOnTheTable.Add(this);
        scaleChordTypes = new int[][] { Music.sus4, Music.seventh, Music.sus2, Music.triad };
        UpdateChord();
    }


    void Update()
    {
        if (!dragger.dragging && deleting)
        {
            Destroy(gameObject);
            song.chordsOnTheTable.Remove(this);
        }

        //Design
        color = Music.DegreeColor(degree);
        nucleus.color = color;
        border1.color = color;
        border2.color = color;
    }

    public void SetOn(int option)
    {
        if (activated)
        {
            SetOff();
        }

        else
        {
            activated = true;
            scaleChordType = scaleChordTypes[option];
            UpdateChord();
            song.ChangeActiveChord(this);
        }


    }
    

    public void SetOff()
    {
        activated = false;
        scaleChordType = defaultScaleChordType;
        UpdateChord();
        song.activeChord = null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Bin")
        {
            deleting = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bin")
        {
            deleting = false;
        }
    }

    void UpdateChord()
    {
        //Qué grado es? (I, III, IV...)
        degreeText.text = Music.ToRomanNumerals(degree);

        //Sacamos las notas para esta tonalidad
        chord = Music.NotesOfChord(degree, song.scale, scaleChordType);

        //es mayor o menor o qué leches
        string chordType = Music.TypeOfChord(chord, scaleChordType);

        //Qué nota es la fundamental? (C, E, A...)
        chordNameText.text = Music.IntToNote(chord[0]).ToString();

        //Es #?
        if (chordNameText.text.ToLower() == chordNameText.text)
        {
            chordNameText.text = chordNameText.text.ToUpper() + '#';
        }

        //Añadimos el menor o mayor al nombre de la fundamental
        chordNameText.text += chordType;
    }
}


