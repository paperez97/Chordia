using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordBlobManager : MonoBehaviour
{
    public GameObject chordPrefab;
    GameObject newChord;
    public bool anyChordsDragging;
    public List<ChordBlob> chordBlobsOnTheTable = new List<ChordBlob>();
    public float gridRadius;
    public GameObject socketPrefab;
    public Song song;
    private void Start()
    {
        song = GameObject.FindGameObjectWithTag("Song").GetComponent<Song>();
    }

    public void ToggleChord(int degree)
    {
        foreach (ChordBlob chord in chordBlobsOnTheTable)
        {
            if (chord.degree == degree)
            {
                DeleteChord(degree);
                return;
            }
        }
        
        CreateChord(degree);
    }
    public void DeleteChord(int degree)
    {
        foreach (ChordBlob chord in chordBlobsOnTheTable)
        {
            if (chord.degree == degree)
            {
                chordBlobsOnTheTable.Remove(chord);
                Destroy(chord.gameObject);
                ArrangeChordBlobs();
                return;
            }
        }
    }

    public void CreateChord(int nDegree)
    {
        ChordBlob newChord = Instantiate(chordPrefab, transform).GetComponent<ChordBlob>();
        chordBlobsOnTheTable.Add(newChord);
        newChord.degree = nDegree;
        ArrangeChordBlobs();
    }

    public void ArrangeChordBlobs()
    {
        for(int i = 0; i < chordBlobsOnTheTable.Count; i++)
        {
            chordBlobsOnTheTable[i].rectTransform.anchoredPosition = GetGridPositions(chordBlobsOnTheTable.Count)[i];
        }
    }

    public List<Vector2> GetGridPositions(int n)
    {
        List<Vector2> result;
        List<Vector2>[] gridCoordinates = new List<Vector2>[]
        {
        new List<Vector2> { Vector2.zero},
        new List<Vector2> { Vector2.left * 0.5f, Vector2.right * 0.5f},
        new List<Vector2> { new Vector2(- 0.5f, 0.289f), new Vector2( 0.5f, 0.289f), new Vector2(0, -0.577f)},
        new List<Vector2> { new Vector2(-0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 0.866f), new Vector2(0, -0.866f) },
        new List<Vector2> { Vector2.zero, new Vector2(-0.5f, 0.866f), new Vector2(0.5f, 0.866f), new Vector2(-0.5f, -0.866f),new Vector2(0.5f, -0.866f)},
        new List<Vector2> { new Vector2(0,0.433f), new Vector2(-0.5f, 1.299f), new Vector2(0.5f, 1.299f), new Vector2(-0.5f, -0.433f), new Vector2(0.5f, -0.433f), new Vector2(0, -1.299f) },
        new List<Vector2> { Vector2.zero, new Vector2(-0.5f, 0.866f), new Vector2(0.5f, 0.866f), new Vector2(-0.5f, -0.866f),new Vector2(0.5f, -0.866f),new Vector2(-0, 1.732f),new Vector2(-0, -1.732f) }
        };

        switch (n)
        {
            case 0:
                result = null;
                break;
            case 1:
                result = gridCoordinates[0];
                break;
            case 2:
                result = gridCoordinates[1];
                break;
            case 3:
                result = gridCoordinates[2];
                break;
            case 4:
                result = gridCoordinates[3];
                break;
            case 5:
                result = gridCoordinates[4];
                break;
            case 6:
                result = gridCoordinates[5];
                break;
            case 7:
                result = gridCoordinates[6];
                break;
            default:
                result = null;
                break;
        }

        for (int i = 0; i < result.Count; i++)
        {
            result[i] *= gridRadius;
        }
        return result;

        //for (int i = 0; i < 6; i++)
        //{
        //    float angle = (1.5f - i) * Mathf.PI / 3;
        //    gridPositions.Add((Vector2)rt.position + gridRadius * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)*verticalStretch));
        //}

    }

    public Vector2 ClosestGridPosition(Vector2 pos)
    {
        Vector2 closestGridPosition = GetGridPositions(chordBlobsOnTheTable.Count)[0];
        foreach (Vector2 candidateGridPosition in GetGridPositions(chordBlobsOnTheTable.Count))
        {
            if (Vector2.Distance(candidateGridPosition, pos) < Vector2.Distance(closestGridPosition, pos))
            {
                closestGridPosition = candidateGridPosition;
            }
        }
        return closestGridPosition;
    }

    public void UpdateChordBlobs()
    {
        foreach(ChordBlob cb in chordBlobsOnTheTable)
        {
            cb.UpdateChordBlob();
        }
    }
}
