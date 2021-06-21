using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ChordEvents : EventTrigger
{
    public ChordBlob chordBlob;


    public override void OnPointerDown(PointerEventData eventData)
    {
        chordBlob.ChordPressed();
    }


    public override void OnPointerUp(PointerEventData eventData)
    {
        chordBlob.ChordLifted();
    }
}
