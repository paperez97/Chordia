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
    public bool dragged = false;

    //Chord
    public Song song;
    public int degree;
    public List<int> chord;
    public int[] scaleChordType;
    public string chordType;

    //Methods
    public void SetOn()
    {
        if (dragged)
        {
            dragged = false;
        }
        else
        {
            if (activated)
            {
                SetOff();
            }
            else
            {
                activated = true;
                song.chord = chord;
                foreach (Transform chordTransform in transform.parent)
                {
                    if (chordTransform != transform)

                    {
                        chordTransform.gameObject.GetComponent<Chord>().SetOff();
                    }
                }
            }
        }
    }
    

    public void SetOff()
    {
        activated = false;
        song.chord = new List<int> { };
    }

    void Start()
    {
        //Behaviour

        //Chord
        song = GameObject.FindGameObjectWithTag("GameController").GetComponent<Song>();
    }


    void Update()
    {
        //Dragging
        if (dragger.dragging)
        {
            dragged = true;
        }
        if(!dragger.dragging && deleting)
        {
            Destroy(gameObject);
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
        song.chord = chord;

        //Design
        color = Music.DegreeColor(degree);
        nucleus.color = color;
        border1.color = color;
        border2.color = color;
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
}


