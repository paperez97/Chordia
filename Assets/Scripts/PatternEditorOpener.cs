using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternEditorOpener : MonoBehaviour
{
    public Toggle toggle;
    public RectTransform rectTransform;
    public RectTransform patternEditorRectTransform;


    public void ChangePositionWhenOn(bool isOn)
    {
        rectTransform.Translate(new Vector3(0,
                                            isOn ?
                                            -(patternEditorRectTransform.sizeDelta.y-20) :
                                            patternEditorRectTransform.sizeDelta.y-20 ));
    }
}
