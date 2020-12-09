using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIElementDragger : EventTrigger
{
    Chord chord;

    void Start()
    {
        chord = gameObject.GetComponent<Chord>();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        chord.ChordPressed();
    }


    public override void OnPointerUp(PointerEventData eventData)
    {
        chord.ChordLifted();
    } 
}