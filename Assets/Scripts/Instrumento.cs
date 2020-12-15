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

    //Methods
    public void TocarNotas(List<int> notasATocar)
    {
        foreach (int tecla in notasATocar)
        {
            transform.GetChild(tecla % Music.keyboard.Length).GetComponent<AudioSource>().Play();
        }
    }
}