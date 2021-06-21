using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class Song : MonoBehaviour
{
    //Harmony
    public ChordBlob activeChordBlob;
    public SavedPattern savedPattern;
    public SavedPattern playingPattern;
    public List<ChordBlob> chordBlobsOnTheTable = new List<ChordBlob>();
    public int[] scaleType;
    public Music.Note[] scale;
    public Music.Note key;
    public Text keyText;

    //Tempo
    public int stepPattern;
    public float tempo;
    public float tempoUnit = 0;
    public float swingTempoUnit1;
    public float swingTempoUnit2;
    public bool play = true;
    public bool swing;


    //Instrument
    public Synth synth;
    public Instrumento interprete;
    public Instrumento drums;
    public List<int> teclasATocar = new List<int>();
    public List<int> bajosATocar = new List<int>();
    public List<int> drumsATocar = new List<int>();
    public int octaves = 2;

    //Behaviour
    public PatternEditor patternEditor;
    public List<int> teclasAcorde;

    //Events
    public UnityEvent OnPatternChange;

    //Methods

    public void ChangePlay(bool isPlay)
    {
        play = isPlay;
    }

    public void ChangeSwing(bool isSwing)
    {
        swing = isSwing;
        Time.fixedDeltaTime = tempoUnit;
    }
    public void ChangeKey(int newKey)
    {
        key = new Music.Note(newKey);
        scale = Music.NotesOfScale(key, scaleType);
        keyText.text = key.Name();
        foreach(ChordBlob chord in chordBlobsOnTheTable)
        {
            chord.UpdateChordBlob();
        }
    }
    public void ChangeTempo(float newTempo)
    {
        tempo = newTempo;
        tempoUnit = 1f / (newTempo / 60) / 2;
        swingTempoUnit1 = 4f / 3f * tempoUnit;
        swingTempoUnit2 = 2f / 3f * tempoUnit;
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
        foreach (ChordBlob chord in chordBlobsOnTheTable)
        {
            chord.UpdateChordBlob();
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
    public void ChangeActiveChord(ChordBlob nChord)
    {
        if (activeChordBlob != null && activeChordBlob != nChord)
        {
            activeChordBlob.SetOff();
        }
        activeChordBlob = nChord;
    }
    public void ChangeBeats(float nBeats)
    {
        savedPattern.beats = nBeats;
    }

    public int StepPatternRemainder()
    {
        return stepPattern % 2;
    }
    void Start()
    {
        stepPattern = -1;
        key = new Music.Note(0);
        ChangeTempo(120);
        ChangeScaleType(0);
        ChangeInterprete(interprete);
        scale = Music.NotesOfScale(key, scaleType);
    }

    void FixedUpdate()
    {
        //Cada pulso en play
        if (play)
        {
            //Avanza stepPattern, y lo vuelve a 0 si se pasa de beats
            stepPattern++;
            if (stepPattern >= savedPattern.beats) { stepPattern = 0; }
            if (savedPattern != playingPattern)
            {
                if (stepPattern == 0)
                {
                    playingPattern.GetComponent<Animator>().SetBool("Blinking", false);
                    playingPattern = savedPattern;
                }
                else { playingPattern.GetComponent<Animator>().SetBool("Blinking", true); }
            }

            //ponemos el swing que toque en función de stepPattern
            if(swing)
            {
                if(stepPattern%2 == 0)
                {
                    Time.fixedDeltaTime = swingTempoUnit1;
                }
                else
                {
                    Time.fixedDeltaTime = swingTempoUnit2;
                }
            }

            //Percusión
            drumsATocar.Clear();
            foreach (int drum in playingPattern.percPattern[stepPattern])
            {
                drumsATocar.Add(drum);
            }
            drums.TocarBajos(drumsATocar);
            
            if (activeChordBlob != null)
            {
                //necesitamos los ints de las teclas del telcado que tocar
                teclasAcorde = Music.TeclasOfChord(activeChordBlob.chord, octaves);

                //Metemos a teclasATocar las teclas que requiere este stepPattern
                teclasATocar.Clear();
                bajosATocar.Clear();
                foreach (int note in playingPattern.pattern[stepPattern])
                {
                    if (note < 0) { bajosATocar.Add(activeChordBlob.bass.number); }
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
