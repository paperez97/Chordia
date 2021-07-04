using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempoPanel : MonoBehaviour
{
    public Toggle swing;
    public Slider bpm;
    public Song song;

    private void Start()
    {
        song.OnRefreshUI += Song_OnRefreshUI;
    }

    private void Song_OnRefreshUI(object sender, System.EventArgs e)
    {
        swing.isOn = song.swing;
        bpm.value = song.tempo;
    }
}
