using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PatternPreviewSq : MonoBehaviour
{

    public int column;
    public int row;
    public SavedPattern savedPattern;
    public Image image;
    public Song song;

    void Start()
    {
        song = FindObjectOfType<Song>();
        song.OnRefreshUI += Song_OnRefreshUI;
        Song_OnRefreshUI(this, EventArgs.Empty);

    }

    private void Song_OnRefreshUI(object sender, System.EventArgs e)
    {
        if(savedPattern.pattern[column].Contains(row))
        {
            TurnOn();
        }
        else { TurnOff(); }
    }

    public void TurnOn()
    {
        image.enabled = true;
    }

    public void TurnOff()
    {
        image.enabled = false;
    }
    private void OnDestroy()
    {
        song.OnRefreshUI -= Song_OnRefreshUI;
    }
}
