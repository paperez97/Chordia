using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    float refDuration;
    private float next = 0;
    public bool swing;
    float duration1;
    float duration2;
    float usingDuration;

    //Instrument
    public Instrumento instrumento;
    public Synth synth;
    public string interprete;
    public List<int> teclasATocar = new List<int>();
    List<float> frecuenciasATocar = new List<float>();


    //Methods
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
        refDuration = 1f / (newTempo / 60);
        duration1 = refDuration * 0.66f;
        duration2 = refDuration * 1.33f;
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
    public void ChangeSwing (bool nSwing)
    {
        swing = nSwing;
    }
    public void ChangeInterprete (string nInterprete)
    {
        interprete = nInterprete;
    }
    public void ChangePattern (SavedPattern nPattern)
    {
        savedPattern = nPattern;
    }
    public void ChangeActiveChord(Chord nChord)
    {
        if (activeChord != null)
        {
            activeChord.SetOff();
        }
        activeChord = nChord;
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

    public void TocaNotaRandom()
    {
        instrumento.TocarNotas(new List<int> { 15 });
    }

    void Start()
    {
        stepPattern = -1;
        key = 0;
        ChangeTempo(200);
        ChangeScaleType(0);
        ChangeSwing(false);
        ChangeInterprete("Piano");
        scale = Music.NotesOfScale(key, scaleType);
        swing = false;

    }

    void FixedUpdate()
    {
        //Cada pulso

        if (Time.time > next && activeChord != null && activeChord.chord.Count != 0)
        {
            //Avanza stepPattern, y lo vuelve a 0 si se pasa de beats
            stepPattern++;
            if (stepPattern >= savedPattern.beats) { stepPattern = 0; }

            //necesitamos los ints de las teclas del telcado que tocar
            List<int> teclasAcorde = Music.TeclasOfChord(activeChord.chord);

            //Metemos a teclasATocar las teclas que requiere este stepPattern
            teclasATocar.Clear();
            foreach (int note in savedPattern.pattern[stepPattern])
            {
                teclasATocar.Add(teclasAcorde[note]);
            }


            //Tocamos las notas que tocan en esta parte del pattern
            if (interprete == "Synth")
            {
                frecuenciasATocar.Clear();
                foreach (int tecla in teclasATocar)
                {
                    frecuenciasATocar.Add(Music.IntToFreq(tecla, 130f));
                }
                synth.TocarNotas(frecuenciasATocar);
            }
            if (interprete == "Piano")
            {
                synth.Silence();
                instrumento.TocarNotas(teclasATocar);
            }
            //Calculamos cuándo será el siguiente paso de stepPattern
            if (swing)
            {
                usingDuration = usingDuration == duration2 ? duration1 : duration2;
                next = Time.time + usingDuration;
            }
            else
            {
                next = Time.time + refDuration;
            }
        }
    }
}
