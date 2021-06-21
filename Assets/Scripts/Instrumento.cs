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
    public float delay;
    List<AudioSource[]> notasParaAcorde;
    List<AudioSource[]> notasParaBajo;
    public Song song;
    public AudioClip[] audioClips = new AudioClip[36];
    public float volume;
    

    void Start()
    {
        song = GameObject.FindGameObjectWithTag("Song").GetComponent<Song>();
        notasParaAcorde = new List<AudioSource[]>();
        notasParaBajo = new List<AudioSource[]>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (i < 12)
            {
                AudioSource audio1 = transform.GetChild(i).GetChild(0).GetComponent<AudioSource>();
                AudioSource audio2 = transform.GetChild(i).GetChild(1).GetComponent<AudioSource>();
                audio1.clip = audioClips[i];
                audio1.volume = volume;
                audio2.clip = audioClips[i];
                audio2.volume = volume;
                if(name == "Guitar" || name == "Banjo")
                {
                    audio1.volume *= 1.3f;
                    audio2.volume *= 1.3f;
                }
                notasParaBajo.Add(new AudioSource[2] { audio1, audio2 });
            }
            else
            {
                AudioSource audio1 = transform.GetChild(i).GetChild(0).GetComponent<AudioSource>();
                AudioSource audio2 = transform.GetChild(i).GetChild(1).GetComponent<AudioSource>();
                audio1.clip = audioClips[i];
                audio1.volume = volume;
                audio2.clip = audioClips[i];
                audio2.volume = volume;
                notasParaAcorde.Add(new AudioSource[2] { audio1, audio2 });
            }
        }
        
    }

    //Methods
    public void TocarNotas(List<int> notasATocar)
    {
        for (int i = 0; i < notasATocar.Count; i++)
        {
            notasParaAcorde[notasATocar[i]][song.StepPatternRemainder()].PlayDelayed(delay*i);
        }
    }
    public void TocarBajos(List<int> bajosATocar)
    {
        for (int i = 0; i < bajosATocar.Count; i++)
        {
            notasParaBajo[bajosATocar[i]][song.StepPatternRemainder()].PlayDelayed(delay*i);
        }
    }
}