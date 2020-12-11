using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Variant : MonoBehaviour
{
    public Chord chord;
    Image image;
    public int angle;

    void Start()
    {
        image = GetComponent<Image>();
    }
    void Update()
    {
        if (chord.isExpanded)
        {

            float radAngle = 2 * Mathf.PI / 360 * angle;
            float farness = (Vector2.Angle(chord.swipe, new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle)))) / 180f;
            if (farness < 1f / chord.numOptions)
            {
                transform.localScale = Vector2.one;
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.6f);
            }
            else
            {
                transform.localScale = Vector2.one * (0.5f + (1-farness) / 3);
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
            }
            
            
        }
    }
}
