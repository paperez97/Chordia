using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedPattern : MonoBehaviour
{

    public List<int>[] pattern;
    public float beats;
    Song song;
    public PatternEditor patternEditor;
    Toggle toggle;
    public bool isDefault;

    

    public void ActivatePattern(bool isOn)
    {
        if (isOn)
        {
            song.pattern = pattern;
            song.beats = beats;
            patternEditor.editingPattern = this;
            patternEditor.slider.value = beats;
            patternEditor.RefreshCells();
        }

    }

    public void SetBeats(float newBeats)
    {
        beats = newBeats;
    }

    public void RefreshEditor()
    {
        patternEditor.RefreshCells();
    }

    // Start is called before the first frame update
    void Start()
    {
        song = GameObject.FindGameObjectWithTag("Song").GetComponent<Song>();
        patternEditor = GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<PatternEditor>();

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

        AddToPattern(1, 6);
        RefreshEditor();

        toggle.group = transform.parent.gameObject.GetComponent<ToggleGroup>();
        toggle.isOn = true;
        ActivatePattern(true);

    }

    private void Update()
    {
        if(toggle.isOn)
        {
            song.pattern = pattern;
            song.beats = beats;
        }
    }

    public void AddToPattern(int beat, int note)
    {
        pattern[beat-1].Add(note);
        RefreshEditor();
    }
}
