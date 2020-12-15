using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tone 
{
    public float frequency;
    public float gain;
    public float increment;
    public float phase;

    public Tone(float nFrequency, float nGain)
    {
        frequency = nFrequency;
        gain = nGain;
        increment = 0f;
        phase = 0f;
    }
}

public class Synth : MonoBehaviour
{
    private float sampling_frequency = 48000;

    //for tonal part
    public List<Tone> tones;





    void Awake()
    {
        sampling_frequency = AudioSettings.outputSampleRate;
        tones = new List<Tone>();
    }



    void OnAudioFilterRead(float[] data, int channels)
    {

        // update increment in case frequency has changed
        foreach (Tone tone in tones)
        {
            tone.increment = tone.frequency * 2f * Mathf.PI / sampling_frequency;
        }


        for (int i = 0; i < data.Length; i++)
        {
            float tonalPart = 0;

            for (int j = 0; j < tones.Count; j++)
            {
                Tone tone = tones[j];
                tone.phase += tone.increment;
                tone.phase %= 2 * Mathf.PI;
                tonalPart += (float)(tone.gain * Mathf.Sin(tone.phase));
            }

            //Bajar volumen de la suma
            if (tones.Count != 0) tonalPart /= tones.Count;
            //together
            data[i] = tonalPart;

            // if we have stereo, we copy the mono data to each channel
            if (channels == 2)
            {
                data[i + 1] = data[i];
                i++;
            }
        }
    }


    public void TocarNotas(List<float> frequencies)
    {
        tones.Clear();

        foreach (float frequency in frequencies)
        {
            tones.Add(new Tone(frequency, 0.2f));
        }
    }

    public void Silence()
    {
        tones.Clear();
    }



}