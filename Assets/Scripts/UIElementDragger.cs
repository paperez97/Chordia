using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIElementDragger : EventTrigger
{
    private float time;
    float lag = 0.3f;
    Vector2 offset;
    Vector2 swipe;
    public bool dragging;
    bool dragged;
    RectTransform rectTrasnform;
    Chord chord;
    int nOptions = 4;

    private bool pressed;

    Vector3 mouseDownPos;

    void Start()
    {
        rectTrasnform = gameObject.GetComponent<RectTransform>();
        chord = gameObject.GetComponent<Chord>();
    }

    public void Update()
    {
        if (pressed && (Time.time - time > lag) && mouseDownPos == Input.mousePosition)
        {
            dragging = true;
            offset = - mouseDownPos + transform.position;
        }

        if (dragging)
        {
            dragged = true;
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y) + offset;
        }

        Debug.Log(swipe[0].ToString() + " " + swipe[1].ToString());

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ChordPressed();
        time = Time.time;
        pressed = true;
        mouseDownPos = Input.mousePosition;
    }


    public override void OnPointerUp(PointerEventData eventData)
    {
        ChordLifted();
        dragging = false;
        pressed = false;
        swipe = Input.mousePosition - transform.position;
        if (dragged) { dragged = false; }
        else { chord.SetOn(SwipeOption(swipe, nOptions)); }
    } 

    public int SwipeOption(Vector2 swipe, int options)
    {
        if(swipe.magnitude < 10)
        {
            return 0;
        }
        //variable para el mejor que luego devolveremos. .x es el número de opción y .y el ángulo
        Vector2 best = new Vector2(0, 360);

        //para cada posible opción, calculamos su vector y lo metemos a best si supera al que estaba
        for (int i = 0; i < options; i++)
        {

            float alpha = 2*Mathf.PI / options *i;
            Vector2 candidate = new Vector2(Mathf.Cos(alpha), Mathf.Sin(alpha));
            float angle = Vector2.Angle(candidate, swipe);
            if (angle < best.y)
            {
                best = new Vector2(i, angle);
            }
        }

        //devolvemos best
        //Debug.Log(best[0].ToString() + " " +  best[1].ToString());
        return (int)best[0];
     }
}