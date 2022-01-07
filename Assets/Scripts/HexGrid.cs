using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public float gridRadius;
    public float verticalStretch;
    public List<Vector2> gridPositions = new List<Vector2>();
    RectTransform rt;
    public GameObject socketPrefab;
    private void Start()
    {
        rt = GetComponent<RectTransform>() ;
        GetGridPositions();
    }

    public void GetGridPositions()
    {
        gridPositions.Add(rt.position);
        //GameObject socket1 = Instantiate(socketPrefab, transform.parent);
        //socket1.GetComponent<RectTransform>().position = rt.position;
        //socket1.transform.SetSiblingIndex(1);
        for (int i = 0; i < 6; i++)
        {
            float angle = (1.5f - i) * Mathf.PI / 3;
            gridPositions.Add((Vector2)rt.position + gridRadius * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)*verticalStretch));
            //GameObject socket2 = Instantiate(socketPrefab, transform.parent);
            //socket2.GetComponent<RectTransform>().position = (Vector2)rt.position + gridRadius * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle) * verticalStretch);
            //socket2.transform.SetSiblingIndex(1);
        }
    }

    public Vector2 ClosestGridPosition(Vector2 pos)
    {
        Vector2 closestGridPosition = gridPositions[0];
        foreach (Vector2 candidateGridPosition in gridPositions)
        {
            if (Vector2.Distance(candidateGridPosition, pos) < Vector2.Distance(closestGridPosition, pos))
            {
                closestGridPosition = candidateGridPosition;
            }
        }
        return closestGridPosition;
    }

    private void OnDrawGizmos()
    {
        foreach(Vector2 pos in gridPositions)
        {
            Gizmos.DrawSphere(pos, 50);
        }
    }
}