using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Song : MonoBehaviour
{
    //Harmony
    public List<int> chord;
    public int[] scaleType;
    public int[] scale;
    public int key;

    //Tempo
    public int stepPattern;
    public List<int>[] pattern;
    public float beats;
    public float tempo;
    float refDuration;
    private float next = 0f;
    public bool swing;
    float duration1;
    float duration2;
    float usingDuration;
    public List<int> first;

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
    }
    public void RefreshScale()
    {
        scale = Music.NotesOfScale(key, scaleType);
    }
    public void ChangeSwing (bool nSwing)
    {
        swing = nSwing;
    }
    public void ChangeInterprete (string nInterprete)
    {
        interprete = nInterprete;
    }
    public void ChangePattern (List<int>[] nPattern)
    {
        pattern = nPattern;
    }

    void Start()
    {
        stepPattern = 0;
        pattern = new List<int>[] { new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>() };
        ChangeKey(0);
        ChangeTempo(200);
        ChangeScaleType(0);
        ChangeSwing(false);
        ChangeInterprete("Synth");
        chord = new List<int> { };
        scale = Music.NotesOfScale(key, scaleType);
        swing = false;

    }

    void Update()
    {
        first = pattern[0];
        //Cada pulso
        if (Time.time > next && chord.Count != 0)
        {
            //Incrementamos pattern
            stepPattern ++;
            if (stepPattern >= beats) { stepPattern = 0; }

            //Segundo necesitamos los ints de las teclas del telcado que tocar
            List<int> teclasAcorde = Music.TeclasOfChord(chord);

            //Metemos a teclasATocar las teclas que requiere este stepPattern
            teclasATocar.Clear();
            foreach (int note in pattern[stepPattern])
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
        else if(chord.Count == 0) synth.Silence();
    }
}
