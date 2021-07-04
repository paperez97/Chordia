using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class KeyPanel : MonoBehaviour
{

    public Transform keyboard;
    public Text keyText;
    public Dropdown dropdown;
    public Song song;

    void Start()
    {
        song.OnRefreshUI += Song_OnRefreshUI;
    }

    public void PressKey(bool isOn)
    {
        if(isOn)
        {
            song.ChangeKey(Music.NoteToInt(keyboard.GetComponent<ToggleGroup>().GetFirstActiveToggle().gameObject.name));
        }
    }
    private void Song_OnRefreshUI(object sender, EventArgs e)
    {
        Debug.Log("song.key = "+ song.key.Name() + 
            "\nActive keyboard key = " + keyboard.GetComponent<ToggleGroup>().GetFirstActiveToggle().gameObject.name);

            //Que se active el Toggle que corresponda a song.key
            foreach (Transform key in keyboard)
            {
                if (key.gameObject.name == song.key.Name())
                {
                    key.gameObject.GetComponent<Toggle>().isOn = true;
                }
            }
            //Cambiamos el texto del panel
            keyText.text = song.key.Name();
            //Cambiamos la escala
            foreach (Dropdown.OptionData option in dropdown.options)
            {
                if (option.text == Music.NameOfScale(song.scaleType)) { dropdown.value = dropdown.options.IndexOf(option); }
            }
    }
}
