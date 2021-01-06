using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class Chord : EventTrigger
{
    //Design
    public Image nucleus;
    public Image border1;
    public Image border2;
    Color color;
    public Text degreeText;
    public Text chordNameText;
    public GameObject variants;
    public ChordCreator chordCreator;

    //Behaviour
    public bool activated = false;
    public bool deleting = false;
    public float redness = 0;
    private float lastPressedTime;
    float lag = 0.3f;
    Vector2 offset;
    public Vector2 swipe;
    public bool dragging;
    public bool dragged;
    public RectTransform rectTransform;
    public int numOptions = 4;
    public bool pressed;
    Vector3 mouseDownPos;
    public bool isExpanded;

    //Chord
    public Song song;
    public List<int> chord;
    public int degree;
    public int[] scaleChordType;
    public int[][] scaleChordTypes;
    public int[] defaultScaleChordType;

    //Animation
    public Animator animator;

    //Methods
    void Start()
    {
        //Chord
        defaultScaleChordType = Music.triad;
        scaleChordType = defaultScaleChordType;
        song = GameObject.FindGameObjectWithTag("Song").GetComponent<Song>();
        chordCreator = GameObject.FindGameObjectWithTag("ChordCreator").GetComponent<ChordCreator>();
        song.chordsOnTheTable.Add(this);
        scaleChordTypes = new int[][] { Music.sus4, Music.seventh, Music.sus2, Music.triad };

        //Design
        color = Music.DegreeColor(degree);
        UpdateChord();
    }

    void Update()
    {
        //Dragging
        if (pressed && (Time.time - lastPressedTime > lag) && mouseDownPos == Input.mousePosition)
        {
            dragging = true;
            animator.SetBool("dragging", true);
            chordCreator.anyChordsDragging = true;
            offset = - mouseDownPos + transform.position;
        }

        
        if (dragging)
        {
            dragged = true;
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y) + offset;

        }
        else
        {
            if (deleting)
            {
                Destroy(gameObject);
                song.chordsOnTheTable.Remove(this);
            }
        }

        color = Music.DegreeColor(degree) + (Color.red - Music.DegreeColor(degree)) * redness;
        ChangeColor(color);

        if(pressed)
        {
            swipe = Input.mousePosition - mouseDownPos;
        }

        //Variantes
        isExpanded = pressed && swipe.magnitude > 40 && !dragging;
        animator.SetBool("isExpanded", isExpanded);
       

        //Que no se salga
        rectTransform.anchoredPosition = new Vector2(Mathf.Clamp(rectTransform.anchoredPosition.x, 0, transform.parent.GetComponent<RectTransform>().rect.width), Mathf.Clamp(rectTransform.anchoredPosition.y, -transform.parent.GetComponent<RectTransform>().rect.height, 0));

    }

    public void UpdateChord()
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
        song.bajoATocar = chord[0];
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
                animator.SetBool("isActive", true);
            }
        }
        //si hay swipe
        else
        {
            scaleChordType = scaleChordTypes[option];
            song.ChangeActiveChord(this);
            scaleChordType = scaleChordTypes[option];
            activated = true;
            animator.SetBool("isActive", true);
            UpdateChord();
        }
    }
    
    public void SetOff()
    {
        activated = false;
        scaleChordType = defaultScaleChordType;
        UpdateChord();
        song.activeChord = null;
        animator.SetBool("isActive", false);
        song.synth.Silence();
    }

    public void ChordPressed()
    {
        lastPressedTime = Time.time;
        pressed = true;
        mouseDownPos = Input.mousePosition;
    }

    public void ChordLifted()
    {
        pressed = false;
        dragging = false;
        animator.SetBool("dragging", false);
        chordCreator.anyChordsDragging = false;
        if (dragged) { dragged = false; }
        else { SetOn(SwipeOption(swipe, numOptions)); }
    }

    public int SwipeOption(Vector2 swipe, int options)
    {
        if(swipe.magnitude < 40)
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

    void ChangeColor(Color color)
    {
        nucleus.color = color;
        border1.color = color;
        border2.color = color;
        foreach(Transform variant in variants.transform)
        {
            variant.gameObject.GetComponent<Image>().color = new Color(color.r, color.g, color.b, variant.gameObject.GetComponent<Image>().color.a);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "ChordCreator")
        {
            deleting = true;
            animator.SetBool("deleting", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "ChordCreator")
        {
            deleting = false;
            animator.SetBool("deleting", false);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ChordPressed();
    }


    public override void OnPointerUp(PointerEventData eventData)
    {
        ChordLifted();
    }
}


