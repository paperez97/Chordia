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

    public void AddNewPattern()
    {
        Instantiate(prefabSavedPattern, parent);
        patternEditorOpener.isOn = true;
        
    }

    void Start()
    {
    }
}
