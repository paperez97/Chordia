using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Song : MonoBehaviour
{
    //Harmony
    public Chord activeChord;
    public SavedPattern savedPattern;
    public List<Chord> chordsOnTheTable = new List<Chord>();
    public int[] scaleType;
    public int[] scale;
    public int key;

    //Tempo
    public int stepPattern;
    public float tempo;
    private float tempoUnit = 0;
    public bool play = true;


    //Instrument
    public Synth synth;
    public Instrumento interprete;
    public Instrumento drums;
    public List<int> teclasATocar = new List<int>();
    public List<int> bajosATocar = new List<int>();
    public List<int> percATocar = new List<int>();
    public int bajoATocar;

    //Behaviour
    public PatternEditor patternEditor;

    //Events
    public UnityEvent OnPatternChange;

    //Methods

    public void ChangePlay(bool isPlay)
    {
        play = isPlay;
    }
    public void ChangeKey(int newKey)
    {
        key = newKey;
        scale = Music.NotesOfScale(key, scaleType);
        foreach(Chord chord in chordsOnTheTable)
        {
            chord.UpdateChord();
        }
    }
    public void ChangeTempo(float newTempo)
    {
        tempo = newTempo;
        tempoUnit = 1f / (newTempo / 60);
        Time.fixedDeltaTime = tempoUnit;
    }
    public void ChangeScaleType(int nScaleType)
    { 
        switch (nScaleType)
        {
            case 0:
                scaleType = Music.escalaJonica;
                break;
            case 1:
                scaleType = Music.escalaEolica;
                break;
            case 2:
                scaleType = Music.escalaLidia;
                break;
            case 3:
                scaleType = Music.escalaMixolidia;
                break;
            case 4:
                scaleType = Music.escalaDorica;
                break;
            case 5:
                scaleType = Music.escalaFrigia;
                break;
            case 6:
                scaleType = Music.escalaLocria;
                break;
        }
        scale = Music.NotesOfScale(key, scaleType);
        foreach (Chord chord in chordsOnTheTable)
        {
            chord.UpdateChord();
        }
    }

    public void ChangeInterprete (Instrumento nInterprete)
    {
        interprete = nInterprete;
    }
    public void ChangePattern (SavedPattern nPattern)
    {
        savedPattern = nPattern;
        patternEditor.Refresh();
    }
    public void ChangeActiveChord(Chord nChord)
    {
        if (activeChord != null)
        {
            activeChord.SetOff();
        }
        activeChord = nChord;
        bajoATocar = activeChord.chord[0];
    }
    public void ChangeBeats(float nBeats)
    {
        savedPattern.beats = nBeats;
    }
    public static float Mod(float a, float b)
    {
        float c = a % b;
        if ((c < 0 && b > 0) || (c > 0 && b < 0))
        {
            c += b;
        }
        return c;
    }

    void Start()
    {
        stepPattern = -1;
        key = 0;
        ChangeTempo(200);
        ChangeScaleType(0);
        ChangeInterprete(interprete);
        scale = Music.NotesOfScale(key, scaleType);

    }

    void FixedUpdate()
    {
        //Avanza stepPattern, y lo vuelve a 0 si se pasa de beats
        if (play)
        {
            stepPattern++;
            if (stepPattern >= savedPattern.beats) { stepPattern = 0; }
            percATocar.Clear();
            foreach (int perc in savedPattern.percPattern[stepPattern])
            {
                percATocar.Add(perc);
            }
            drums.TocarBajos(percATocar);

            //Cada pulso en play
            if (activeChord != null)
            {
                //necesitamos los ints de las teclas del telcado que tocar
                List<int> teclasAcorde = Music.TeclasOfChord(activeChord.chord);

                //Metemos a teclasATocar las teclas que requiere este stepPattern
                teclasATocar.Clear();
                bajosATocar.Clear();
                foreach (int note in savedPattern.pattern[stepPattern])
                {
                    if (note < 0) { bajosATocar.Add(bajoATocar); }
                    else { teclasATocar.Add(teclasAcorde[note]); }
                }

                //Tocamos las notas que tocan en esta parte del pattern
                synth.Silence();
                if (interprete.name == "Synth")
                {
                    synth.TocarNotas(teclasATocar);
                    synth.TocarBajos(bajosATocar);
                }
                else
                {
                    interprete.TocarNotas(teclasATocar);
                    interprete.TocarBajos(bajosATocar);
                }
            }
        }
    }
}
