using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Debugging : MonoBehaviour
{


    string[] notes = new string[] { "C", "c", "D", "d", "E", "F", "f", "G", "g", "A", "a", "B" };
    string[] keyboard = new string[] { "C1", "c1", "D1", "d1", "E1", "F1", "f1", "G1", "g1", "A1", "a1", "B1",
                                              "C2", "c2", "D2", "d2", "E2", "F2", "f2", "G2", "g2", "A2", "a2", "B2" };


    //Devuelve una lista con los ints de todas las notas de keyboard que pertenecen al acorde
    List<int> allIntsOfChord(string[] chord)
    {
        List<int> result = new List<int>();
        foreach (string nota in keyboard)
        {
            if (Array.Exists(chord, element => element == ("" + nota[0])))
            {
                result.Add(Array.IndexOf(keyboard, nota));
                //Debug.Log(Array.IndexOf(keyboard, nota));
                //Debug.Log(keyboard[Array.IndexOf(keyboard, nota)]);
            }
        }
        return result;
    }

    //Devuelve la nota más cercana a note que forma parte del acorde
    int ChangeNoteToChord(int note, string[] chord)
    {
        List<int> chordInts = allIntsOfChord(chord);
        int candidate = chordInts[0];
        foreach (int chordKey in chordInts)
        {
            if (Math.Abs(note - chordKey) < Math.Abs(candidate - chordKey))
            {
                candidate = chordKey;
            }
        }
        return candidate;
    }



    // Start is called before the first frame update
    void Start()
    {
        int[] acordeInts = { 0, 7, 16 };
        string[] acorde = new string[] { "D", "F", "A" };

        //List<int> result = new List<int>();
        //string[] lista = new string[] { "A1", "B1", "b1", "C1", "D1" };
        //string[] pares = new string[] { "B", "D" };
        //foreach (string num in lista)
        //{
        //    if (Array.Exists(pares, element => element == ("" + num[0])))
        //    {
        //        result.Add(Array.IndexOf(lista, num));
        //    }
        //}


        int[] arpegio = new int[acordeInts.Length];
        for (int i = 0; i < acordeInts.Length; i++)
        {
            arpegio[i] = ChangeNoteToChord(acordeInts[i], acorde);
        }

        foreach (int item in arpegio)
        {
            Debug.Log(item);
            Debug.Log(keyboard[item]);
        }



        //int[] newChordInts = new int[acordeInts.Length];
        //for (int i = 0; i < acordeInts.Length; i++)
        //{
        //    newChordInts[i] = ChangeNoteToChord(acordeInts[i], acorde);
        //}
        //foreach (int item in newChordInts)
        //{
        //    Debug.Log(item);
        //}






















    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
