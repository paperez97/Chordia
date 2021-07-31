using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ChordBlob : MonoBehaviour
{
    //Design
    public Image nucleus;
    public Image border1;
    public Image border2;
    public Sprite circle1;
    public Sprite circle2;
    public Sprite circle3;
    public Sprite circle4;
    public Sprite circle5;
    public Sprite circle6;
    public Sprite circle7;
    public Sprite resetCircle;
    public Sprite borderSprite;
    Color degreeColor;
    public Text degreeText;
    public Text degreeText2;
    public Text chordNameText;
    public Text chordNameText2;
    public Transform variants;
    public ChordCreator chordCreator;

    //Behaviour
    public bool activated = false;
    public bool deleting = false;
    private float lastPressedTime;
    float lag = 0.3f;
    Vector2 offset;
    public Vector2 swipe;
    public bool dragging;
    public bool dragged;
    public RectTransform rectTransform;
    public int numOptions;
    public bool pressed;
    Vector3 mouseDownPos;
    public bool isExpanded;
    public int option;
    public HexGrid hexGrid;

    //Chord
    public Song song;
    public Music.Chord chord;
    public Music.Note bass;
    public int degree;
    public string variantMessage;

    //Animation
    public Animator animator;

    //Methods
    void Start()
    {
        //Chord
        song = GameObject.FindGameObjectWithTag("Song").GetComponent<Song>();
        song.OnRefreshUI += Song_OnRefreshUI;
        chordCreator = GameObject.FindGameObjectWithTag("ChordCreator").GetComponent<ChordCreator>();
        numOptions = variants.childCount;

        //Design
        degreeColor = Music.DegreeColor(degree);
        switch(degree)
        {
            case 1:
                borderSprite = circle1;
                break;
            case 2:
                borderSprite = circle2;
                break;
            case 3:
                borderSprite = circle3;
                break;
            case 4:
                borderSprite = circle4;
                break;
            case 5:
                borderSprite = circle5;
                break;
            case 6:
                borderSprite = circle6;
                break;
            case 7:
                borderSprite = circle7;
                break;
            default:
                borderSprite = circle1;
                break;
        }
        border1.sprite = borderSprite;
        border2.color = degreeColor;
        degreeText.color = degreeColor + Color.white * 0.3f;
        chordNameText.color = degreeText.color;

        foreach (Transform variant in variants)
        {
            variant.gameObject.GetComponent<Variant>().border.sprite = borderSprite;
            variant.gameObject.GetComponent<Variant>().glow.color = degreeColor;
            variant.gameObject.GetComponent<Variant>().text.color = degreeText.color;
        }
        UpdateChordBlob();

        //Behaviour
        hexGrid = transform.parent.GetComponent<HexGrid>();

        
    }

    private void Song_OnRefreshUI(object sender, EventArgs e)
    {
        UpdateChordBlob();
    }

    void Update()
    {
        //Dragging
        if (pressed && (Time.time - lastPressedTime > lag) && Input.mousePosition == mouseDownPos)
        {
            dragging = true;
            animator.SetBool("dragging", true);
            chordCreator.anyChordsDragging = true;
            offset = - mouseDownPos + transform.position;
        }

        
        if (dragging)
        {
            dragged = true;
            transform.SetAsLastSibling();
            PlaceOnGrid(new Vector2(Input.mousePosition.x, Input.mousePosition.y) + offset, hexGrid.gridPositions);
            
        }
        else
        {
            if (deleting)
            {
                Destroy(gameObject);
                song.chordBlobsOnTheTable.Remove(this);
            }
        }

        if(pressed)
        {
            swipe = Input.mousePosition - mouseDownPos;
        }

        //Variantes
        isExpanded = pressed && swipe.magnitude > 40 && !dragging;
        animator.SetBool("isExpanded", isExpanded);

        if (isExpanded)
        {
            option = SwipeOption(swipe, variants.childCount);
            for(int i = 0; i < variants.childCount; i++)
            {
                Variant variant = variants.GetChild(i).GetComponent<Variant>();
                variant.isOn = variant.optionNumber == option;
            }
        }

    }

    public void PlaceOnGrid(Vector2 supposedPosition, List<Vector2> gridPositions)
    {
        Vector2 closestGridPosition = gridPositions[0];
        foreach (Vector2 candidateGridPosition in gridPositions)
        {
            if (Vector2.Distance(candidateGridPosition, supposedPosition) < Vector2.Distance(closestGridPosition, supposedPosition))
            {
                closestGridPosition = candidateGridPosition;
            }
        }
        transform.position = closestGridPosition;
    }

    public void UpdateChordBlob()
    {
        //Qué grado es? (I, III, IV...)
        degreeText.text = Music.ToRomanNumerals(degree);
        degreeText2.text = Music.ToRomanNumerals(degree);

        //Sacamos las notas para esta tonalidad
        chord = Music.CalculateChord(degree, song.scale, variantMessage);
        bass = song.key.MoveInScale(degree-1, song.scale);


        //Cifrado americano
        chordNameText.text = chord.ChordName();
        chordNameText2.text = chord.ChordName();

        //Actualizamos textos en variantes
        for (int i = 0; i < variants.childCount; i++)
        {
            Variant variant = variants.GetChild(i).GetComponent<Variant>();
            variant.UpdateVariant();
        }
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
                UpdateChordBlob();
                song.ChangeActiveChord(this);
                animator.SetBool("isActive", true);
                
            }
        }
        //si hay swipe
        else
        {
            song.ChangeActiveChord(this);
            activated = true;
            animator.SetBool("isActive", true);
            UpdateChordBlob();
        }
    }
    
    public void SetOff()
    {
        activated = false;
        variantMessage = "triad";
        UpdateChordBlob();
        song.activeChordBlob = null;
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
        foreach (ChordBlob chordBlob in song.chordBlobsOnTheTable)
        {
            if (chordBlob.rectTransform.position == rectTransform.position && chordBlob != this)
            {
                chordBlob.PlaceOnGrid(mouseDownPos, hexGrid.gridPositions);
            }
        }
    }

    public int SwipeOption(Vector2 swipe, int options)
    {
        //Swipes chiquiticos no
        if(swipe.magnitude < 40)
        {
            return -1;
        }

        //variable para el mejor que luego devolveremos. .x es el número de opción y .y el ángulo
        Vector2 best = new Vector2(0, 360);

        //para cada posible opción, calculamos su vector y lo metemos a best si supera al que estaba
        for (int i = 0; i < options; i++)
        {
            float alpha = (2*Mathf.PI / options *i) + Mathf.PI/6;
            Vector2 candidate = new Vector2(Mathf.Cos(alpha), Mathf.Sin(alpha));
            float angle = Vector2.Angle(candidate, swipe);
            if (angle < best.y)
            {
                best = new Vector2(i, angle);
            }
        }

        //devolvemos best
        return (int)best[0];
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
    private void OnDestroy()
    {
        song.chordBlobsOnTheTable.Remove(this);
        song.OnRefreshUI -= Song_OnRefreshUI;
    }
}


