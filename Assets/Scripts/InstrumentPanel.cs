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
