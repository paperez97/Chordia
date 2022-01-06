using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstrumentPanel : MonoBehaviour
{
    public Instrumento piano;
    public Instrumento synth;
    public Instrumento banjo;
    public Instrumento guitar;
    public Song song;

    private void Start()
    {
        song.OnRefreshUI += Song_OnRefreshUI;
    }

    private void Song_OnRefreshUI(object sender, System.EventArgs e)
    {
        //Que se active el Toggle que corresponda a song.interprete
        foreach (Transform instrumentToggle in transform)
        {
            if (instrumentToggle.gameObject.name == song.interprete.name)
            {
                instrumentToggle.gameObject.GetComponent<Toggle>().isOn = true;
            }
        }
    }

    public void PressInstrument(bool isOn)
    {
        if (isOn)
        {
            switch (GetComponent<ToggleGroup>().GetFirstActiveToggle().gameObject.name)
            {
                case "Piano":
                    song.ChangeInterprete(piano);
                    break;
                case "Synth":
                    song.ChangeInterprete(synth);
                    break;
                case "Banjo":
                    song.ChangeInterprete(banjo);
                    break;
                case "Guitar":
                    song.ChangeInterprete(guitar);
                    break;
                default:
                    return;
            }
        }
    }
}
