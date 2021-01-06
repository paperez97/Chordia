using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Music 
{

    //Acordes por semitonos
    public static int[] major = { 0, 4, 7};
    public static int[] minor = { 0, 3, 7 };
    public static int[] dim = { 0, 3, 6 };
    public static int[] aug = { 0, 4, 8 };

    //Acordes por escala
    public static int[] triad = { 0, 2, 4 };
    public static int[] sus2 = { 0, 1, 4 };
    public static int[] sus4 = { 0, 3, 4 };
    public static int[] seventh = { 0, 2, 4, 6 };

    //Escalas
    public static int[] escalaJonica = { 0, 2, 4, 5, 7, 9, 11 };
    public static int[] escalaEolica = { 0, 2, 3, 5, 7, 8, 10 };
    public static int[] escalaLidia = { 0, 2, 4, 6, 7, 9, 11 };
    public static int[] escalaMixolidia = { 0, 2, 4, 5, 7, 9, 10 };
    public static int[] escalaDorica = { 0, 2, 3, 5, 7, 9, 10 };
    public static int[] escalaFrigia = { 0, 1, 3, 5, 7, 8, 10 };
    public static int[] escalaLocria = { 0, 1, 3, 5, 6, 8, 10 };

    //Notas
    public static char[] notes = { 'C', 'c', 'D', 'd', 'E', 'F', 'f', 'G', 'g', 'A', 'a', 'B' };
    public static string[] bassKeyboard = { "C1", "c1", "D1", "d1", "E1", "F1", "f1", "G1", "g1", "A1", "a1", "B1"};
    public static string[] keyboard = {"C2", "c2", "D2", "d2", "E2", "F2", "f2", "G2", "g2", "A2", "a2", "B2",
                                   "C3", "c3", "D3", "d3", "E3", "F3", "f3", "G3", "g3", "A3", "a3", "B3"};
    //Colores
    public static Color Icolor = new Color((float)42 / 255, (float)99 / 255, (float)242 / 255);
    public static Color IIcolor = new Color((float)19 / 255, (float)191 / 255, (float)49 / 255);
    public static Color IIIcolor = new Color((float)214 / 255, (float)48 / 255, (float)48 / 255);
    public static Color IVcolor = new Color((float)237 / 255, (float)151 / 255, (float)88 / 255);
    public static Color Vcolor = new Color((float)96 / 255, (float)202 / 255, (float)239 / 255);
    public static Color VIcolor = new Color((float)240 / 255, (float)244 / 255, (float)88 / 255);
    public static Color VIIcolor = new Color((float)198 / 255, (float)93 / 255, (float)188 / 255);


    //Methods
    public static int NoteToInt(char note)
    {
        return Array.IndexOf(notes, note);
    }
    public static char IntToNote(int note)
    {
        return notes[note];
    }
    public static int[] NotesOfScale(int key, int[] scaleType)
    {
        //Devuelve una lista con las notas que pertenecen a la escala
        //definida por key y scaleType
        int[] nScale = new int[scaleType.Length];
        for (int i = 0; i < scaleType.Length; i++)
        {
            nScale[i] = (key + scaleType[i]) % 12;
        }
        return nScale;
    }
    public static List<int> NotesOfChord(int degree, int[] scale, int[] scaleChordType)
    {   //Devuelve una lista con las notas del acorde
        //del degree dado, con la escala dada y del tipo dado
        List<int> notesOfChord = new List<int>();

        foreach (int scaleNote in scaleChordType)
        {
            notesOfChord.Add(scale[(degree - 1 + scaleNote) % 7]);
        }

        return notesOfChord;

    }
    public static List<int> TeclasOfChord(List<int> chord)
    {   //Devuelve una lista con los ints de todas
        //las teclas de keyboard que pertenecen al acorde

        List<int> result = new List<int>();
        if (chord.Count == 3)
        {
            foreach (string tecla in Music.keyboard)
            {
                if (chord.Contains(Music.NoteToInt(tecla[0])))
                {
                    result.Add(Array.IndexOf(Music.keyboard, tecla));
                }
            }
        }
        if (chord.Count == 4)
        {
            //PUES CAMBIAMOS LA NOTA FUNDAMENTAL  POR LA SÉPTIMA, PORQUE NO QUEREMOS QUE LA SÉPTIMA SUENE en el bajo PORQUE QUEDA MAAAL
            //Tecla es nombre + octava ("E3"), tecla[0] coge solo el nombre de la nota ("E")
            int highestFirst = 0;
            foreach (string tecla in Music.keyboard)
            {
                if (chord.Contains(Music.NoteToInt(tecla[0])) && Music.NoteToInt(tecla[0]) != chord[3])
                {
                    result.Add(Array.IndexOf(Music.keyboard, tecla));
                    if (Music.NoteToInt(tecla[0]) == chord[0])
                    {
                        highestFirst = result.Count - 1;
                    }
                }
            }

            if(Modulo(chord[3] - chord[0], 12) == 10)
            {
                result[highestFirst] -= 2;
                if (result[highestFirst - 3] > 2)
                { result[highestFirst - 3] -= 2; }
            }
            if (Modulo(chord[3] - chord[0], 12) == 11)
            {
                result[highestFirst] -= 1;
                if (result[highestFirst - 3] > 1)
                { result[highestFirst - 3] -= 1; }

            }
        }
        return result;
    }

    public static string TypeOfChord(List<int> notesOfChord, int[] scaleChordType)
    {   //Devuelve el tipo del acorde introducido --> (str) "m", "sus2", "M7" ...

        if (Music.AreArraysEqual(scaleChordType, Music.triad))
        {
            int[] semitones = new int[notesOfChord.Count];
            for (int i = 0; i < semitones.Length; i++)
            {
                if (notesOfChord[i] < notesOfChord[0])
                {
                    semitones[i] = (notesOfChord[i] + 12 - notesOfChord[0]);
                }
                else
                {
                    semitones[i] = ((notesOfChord[i]) - notesOfChord[0]);
                }
            }
            if (AreArraysEqual(semitones, major)) { return ""; }
            else if (AreArraysEqual(semitones, minor)) { return "m"; }
            else if (AreArraysEqual(semitones, dim)) { return "dim"; }
            else if (AreArraysEqual(semitones, aug)) { return "aug"; }
            else { return "TriadButNotFound"; }
        }
        else if (Music.AreArraysEqual(scaleChordType, Music.sus2)) { return "sus2"; }
        else if (Music.AreArraysEqual(scaleChordType, Music.sus4)) { return "sus4"; }
        else if (Music.AreArraysEqual(scaleChordType, Music.seventh)) { return " 7"; }
        else { return "NoScaleTypeFound"; }
    }
    public static string ToRomanNumerals(int n)
    {   //Devuelve un int del 1 al 7 en números romanos

        string[] romanNumerals = { "I", "II", "III", "IV", "V", "VI", "VII" };
        if (n >= 1 && n <= 7) { return romanNumerals[n - 1]; }
        else { return "Not in range"; }
    }
    public static Color DegreeColor(int degree)
    {
        switch(degree)
        {
            case 1:
                return Icolor;
            case 2:
                return IIcolor;
            case 3:
                return IIIcolor;
            case 4:
                return IVcolor;
            case 5:
                return Vcolor;
            case 6:
                return VIcolor;
            case 7:
                return VIIcolor;
        }
        return Color.white;
    }
    public static bool AreArraysEqual(int[] first, int[] second)
    {
        if(first.Length != second.Length) { return false; }
        for (int i = 0; i < first.Length; i++)
        {
            if (first[i] != second[i])
            {
                return false;
            }
        }
        return true;
    }
    public static float IntToFreq(int note, float referenceFreq)
    {
        return referenceFreq  * Mathf.Pow(Mathf.Pow(2, 1f / 12f), note);
    }

    static int Modulo(int a, int b)
    {
        return (Math.Abs(a * b) + a) % b;
    }

    // 0   1   2   3   4   5   6   7   8    9   10  11  12
    // do  do# re  mib mi  fa  fa# sol sol# la  sib si
}
