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
    private float time;
    float lag = 0.3f;
    Vector2 offset;
    Vector2 swipe;
    public bool dragging;
    bool dragged;
    RectTransform rectTrasnform;
    int nOptions = 4;
    private bool pressed;
    Vector3 mouseDownPos;

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
        rectTrasnform = gameObject.GetComponent<RectTransform>();
        UpdateChord();
    }

    void Update()
    {
        //Dragging
        if (pressed && (Time.time - time > lag) && mouseDownPos == Input.mousePosition)
        {
            dragging = true;
            offset = - mouseDownPos + transform.position;
        }

        if (dragging)
        {
            dragged = true;
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y) + offset;
        }

        if (!dragging && deleting)
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

    public void SetOn(int option)
    {
        //Si no hay swipe
        if (option<0)
        {
            if (activated)
            {
                SetOff();
            }
            else
            {
            activated = true;
            scaleChordType = defaultScaleChordType;
            UpdateChord();
            song.ChangeActiveChord(this);
            }
        }
        //si hay swipe
        else
        {
            scaleChordType = scaleChordTypes[option];
            song.ChangeActiveChord(this);
            scaleChordType = scaleChordTypes[option];
            activated = true;
            UpdateChord();
        }
    }
    
    public void SetOff()
    {
        activated = false;
        scaleChordType = defaultScaleChordType;
        UpdateChord();
        song.activeChord = null;
    }

    public void ChordPressed()
    {
        time = Time.time;
        pressed = true;
        mouseDownPos = Input.mousePosition;
    }

    public void ChordLifted()
    {
        dragging = false;
        pressed = false;
        swipe = Input.mousePosition - mouseDownPos;
        if (dragged) { dragged = false; }
        else { SetOn(SwipeOption(swipe, nOptions)); }
    }

    public int SwipeOption(Vector2 swipe, int options)
    {
        if(swipe.magnitude < 10)
        {
            return -1;
        }

        //variable para el mejor que luego devolveremos. .x es el número de opción y .y el ángulo
        Vector2 best = new Vector2(0, 360);

        //para cada posible opción, calculamos su vector y lo metemos a best si supera al que estaba
        for (int i = 0; i < options; i++)
        {

            float alpha = 2*Mathf.PI / options *i;
            Vector2 candidate = new Vector2(Mathf.Cos(alpha), Mathf.Sin(alpha));
            float angle = Vector2.Angle(candidate, swipe);
            if (angle < best.y)
            {
                best = new Vector2(i, angle);
            }
        }
        //devolvemos best
        //Debug.Log(best[0].ToString() + " " +  best[1].ToString());
        return (int)best[0];
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


