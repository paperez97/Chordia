using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Instrumento : MonoBehaviour
{
    //Audio
    AudioSource[] audioSources;
    private AudioSource source;

    //Methods
    public void TocarNotas(List<int> notasATocar)
    {
        foreach (int tecla in notasATocar)
        {
            source.PlayOneShot(audioSources[tecla % Music.keyboard.Length].clip, 0.15f);
        }
    }

    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        source = audioSources[0];
    }
}