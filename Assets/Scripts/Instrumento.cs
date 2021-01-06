using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Instrumento : MonoBehaviour
{
    //Audio
    AudioSource[] audioSources;
    private AudioSource source;
    public string name;
    List<AudioSource> notasParaAcorde;
    List<AudioSource> notasParaBajo;
    

    void Start()
    {
        notasParaAcorde = new List<AudioSource>();
        notasParaBajo = new List<AudioSource>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i < 12)
            {
                notasParaBajo.Add(transform.GetChild(i).GetComponent<AudioSource>());
            }
            else
            {
                notasParaAcorde.Add(transform.GetChild(i).GetComponent<AudioSource>());
            }
        }
    }

    //Methods
    public void TocarNotas(List<int> notasATocar)
    {
        foreach (int tecla in notasATocar)
        {
                notasParaAcorde[tecla].Play();
        }
    }
    public void TocarBajos(List<int> bajosATocar)
    {
        foreach (int tecla in bajosATocar)
        {
                notasParaBajo[tecla].Play();
        }
    }
}