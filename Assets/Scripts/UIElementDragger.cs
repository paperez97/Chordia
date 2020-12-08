using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIElementDragger : EventTrigger
{
    private float time;

    public bool dragging;

    private bool pressed;

    Vector3 mousePos;

    public void Update()
    {
        if (pressed && (Time.time - time > 0.5) && mousePos == Input.mousePosition)
        {
            dragging = true;
        }

        if (dragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    //public override void OnPointerDown(PointerEventData eventData)
    //{
    //    dragging = true;
    //}


    public override void OnPointerDown(PointerEventData eventData)
    {
        time = Time.time;
        pressed = true;
        mousePos = Input.mousePosition;
    }


    public override void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
        pressed = false;
    }
}