using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellGrid : MonoBehaviour
{
    public RectTransform rectTrasnform;
    public GridLayoutGroup grid;
    float height;
    float width;

    void Start()
    {
        //rectTrasnform = GetComponent<RectTransform>();
        //grid = GetComponent<GridLayoutGroup>();
        width = rectTrasnform.rect.width;
        height = rectTrasnform.rect.height;
        grid.cellSize = new Vector2((width - grid.constraintCount * grid.spacing.x) / grid.constraintCount, (height - 9 * grid.spacing.y) / 9);
    }

}
