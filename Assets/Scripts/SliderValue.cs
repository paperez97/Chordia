using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    public Text text;
    public GameObject song;
    Song songS;


    void Start()
    {
        songS = song.GetComponent<Song>();
        text = GetComponent<Text>();

    }

    void Update()
    {
        text.text = songS.tempo.ToString();
    }
}
