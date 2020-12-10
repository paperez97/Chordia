using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternSaver : MonoBehaviour
{

    public GameObject prefabSavedPattern;
    public Transform parent;
    public Toggle patternEditorOpener;
    public PatternEditor patternEditor;

    public void AddNewPattern(bool isDefault)
    {
        SavedPattern nPattern = Instantiate(prefabSavedPattern, parent).GetComponent<SavedPattern>();
        if (isDefault)
        {
            nPattern.isDefault = true;
        }
        else{patternEditorOpener.isOn = true;}
    }

    void Start()
    {
        AddNewPattern(false);

    }

}
