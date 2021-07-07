using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedPattern : MonoBehaviour
{

    public List<int>[] pattern = new List<int>[] { new List<int>(),
                                    new List<int>(),
                                    new List<int>(),
                                    new List<int>(),
                                    new List<int>(),
                                    new List<int>(),
                                    new List<int>(),
                                    new List<int>()
};
public List<int>[] percPattern;
    public float beats;
    public Song song;
    Toggle toggle;
    public bool isDefault;
    public Transform sqPreviewGrid;
    public Transform sqPreviewGridBass;
    
    public void SetBeats(float newBeats)
    {
        beats = newBeats;
    }

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("pattern started");
        song = GameObject.FindGameObjectWithTag("Song").GetComponent<Song>();

        toggle = GetComponent<Toggle>();
 


        percPattern = new List<int>[] { new List<int>(),
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
            AddToPattern(1, -1);
            AddToPattern(1, 0);
            AddToPattern(2, 1);
            AddToPattern(3, 2);
            AddToPattern(4, 3);
            AddToPattern(5, 4);
            AddToPattern(6, 2);
            AddToPattern(7, 3);
            AddToPattern(8, 4);
            beats = 8;
        }
        toggle.group = transform.parent.gameObject.GetComponent<ToggleGroup>();
        toggle.isOn = true;
        ActivatePattern();

    }

    public void ActivatePattern()
    {
         song.ChangePattern(this); 
    }

    public void AddToPattern(int beat, int note)
    {
        pattern[beat-1].Add(note);
    }

}
