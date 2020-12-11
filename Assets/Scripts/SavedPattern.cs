using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedPattern : MonoBehaviour
{

    public List<int>[] pattern;
    public float beats;
    Song song;
    Toggle toggle;
    public bool isDefault;

    public void SetBeats(float newBeats)
    {
        beats = newBeats;
    }

    // Start is called before the first frame update
    void Start()
    {
        song = GameObject.FindGameObjectWithTag("Song").GetComponent<Song>();

        toggle = GetComponent<Toggle>();
 
        pattern = new List<int>[] { new List<int>(),
                                    new List<int>(),
                                    new List<int>(),
                                    new List<int>(),
                                    new List<int>(),
                                    new List<int>(),
                                    new List<int>(),
                                    new List<int>() };
        beats = 8;

        if(isDefault)
        {
            AddToPattern(1, 0);
            AddToPattern(1, 3);
            AddToPattern(2, 4);
            AddToPattern(3, 5);
            AddToPattern(4, 6);
            AddToPattern(5, 7);
            AddToPattern(6, 5);
            AddToPattern(7, 6);
            AddToPattern(8, 7);
            beats = 8;
        }
        toggle.group = transform.parent.gameObject.GetComponent<ToggleGroup>();
        toggle.isOn = true;
    }

    private void Update()
    {
        if(toggle.isOn)
        {
            song.ChangePattern(this);
            song.ChangeBeats(beats);
        }
    }

    public void AddToPattern(int beat, int note)
    {
        pattern[beat-1].Add(note);
    }
}
