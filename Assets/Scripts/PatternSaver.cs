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
    public Button bin;

    public void AddNewPattern()
    {
        ReturnNewPattern();
    }

    public SavedPattern ReturnNewPattern()
    {
        bin.interactable = true;
        return Instantiate(prefabSavedPattern, parent).GetComponent<SavedPattern>();
    }

    public void AddThisPattern(SavedPattern nPattern)
    {
        Instantiate(nPattern, parent).GetComponent<SavedPattern>();
    }

    public void DestroyAllPatterns()
    {
        foreach(Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}

