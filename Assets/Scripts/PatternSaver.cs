using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternSaver : MonoBehaviour
{

    public GameObject prefabSquare;
    public Transform parent;
    public Toggle patternEditorOpener;

    public void SavePattern()
    {
        patternEditorOpener.isOn = true;
        Instantiate(prefabSquare, parent);

    }

}
