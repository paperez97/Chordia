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
    public static int[] triad = { 0, 2, 4, 6};
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
    public static string[] notes = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
    
    //Colores
    public static Color Icolor = new Color((float)42 / 255, (float)99 / 255, (float)242 / 255);
    public static Color IIcolor = new Color((float)19 / 255, (float)191 / 255, (float)49 / 255);
    public static Color IIIcolor = new Color((float)214 / 255, (float)48 / 255, (float)48 / 255);
    public static Color IVcolor = new Color((float)237 / 255, (float)151 / 255, (float)88 / 255);
    public static Color Vcolor = new Color((float)96 / 255, (float)202 / 255, (float)239 / 255);
    public static Color VIcolor = new Color((float)240 / 255, (float)244 / 255, (float)88 / 255);
    public static Color VIIcolor = new Color((float)198 / 255, (float)93 / 255, (float)188 / 255);


    //Methods
    public static int NoteToInt(string noteName)
    {
        return Array.IndexOf(notes, noteName);
    }
    public static string IntToNote(int note)
    {
        return notes[note];
    }
    public static Note[] NotesOfScale(Note key, int[] mode)
    {
        //Devuelve una lista con las notas del modo mode en la tonalidad key
        //definida por key y scaleType
        Note[] nScale = new Note[mode.Length];
        for (int i = 0; i < mode.Length; i++)
        {
            nScale[i] = new Note((key.number + mode[i]) % 12);
        }
        return nScale;
    }
    public static Chord CalculateChord(int degree, Note[] scale, string message)
    {   //Devuelve una lista con las notas del acorde
        //del degree dado, con la escala dada y del tipo dado
        List<Note> notesForChord = new List<Note>();
        foreach (int item in Music.triad)
        {
            notesForChord.Add(scale[(degree - 1 + item) % 7]);
        }
        Chord nChord = new Chord(notesForChord);
        nChord = nChord.Variant(message, scale);
        return nChord;
    }
    public static List<int> TeclasOfChord(Chord chord, int octaves)
    {   //Devuelve una lista con los ints de todas
        //las teclas de keyboard que pertenecen al acorde
        List<int> result = new List<int>();
        for(int i = 0; i < octaves; i++)
        {
            foreach (Note chordNote in chord.ChordNotes("toPlay"))
            {
                    result.Add(chordNote.number + i * notes.Length);
            }
        }
        result.Sort();
        return result;
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

    public static int Modulo(int a, int b)
    {
        return (Math.Abs(a * b) + a) % b;
    }

    //Classes

    public class Note
    {
        //Properties
        public int number;

        //Constructors
        public Note() { }
        public Note(int nNumber)
        {
            number = nNumber;
        }

        public Note(string noteName)
        {
            number = Music.NoteToInt(noteName);
        }

        //Methods
        public string Name()
        {
            return notes[number];
        }

        public Note Copy()
        {
            return new Note(number);
        }
        public Note Transposed(int semitones)
        {
            Note result = new Note(this.number);
            int nNumber = Modulo(result.number + semitones, notes.Length);
            result.number = nNumber;
            return result;
        }

        public int IntervalWith(Note other)
        {
            int result = Modulo(other.number - number, 12);
            return result;
        }

        public Note MoveInScale(int steps, Note[] scale)
        {
            for (int i = 0; i < scale.Length; i++)
            {
                if(scale[i].number == number) 
                {
                    return scale[Modulo(i + steps, scale.Length)].Copy();
                }
            }
            return null;
        }

        public string BemolName()
        {
            string result = this.Transposed(1).Name() + "♭";
            return result;
        }

    }

    public class Chord
    {
        //Properties
        public Note fundamental;
        public Note third;
        public Note fifth;
        public Note seventh;
        public bool seventhOn = false;
        public string variant;

        //Constructors
        public Chord() 
        {

        }
        public Chord(List<Note> nNotesOfChord)
        {
            fundamental = nNotesOfChord[0];
            third = nNotesOfChord[1];
            fifth = nNotesOfChord[2];
            seventh = nNotesOfChord[3];
        }
        public Chord(int nFundamental, int nThird, int nFifth, int nSeventh = 100)
        {
            fundamental = new Note(nFundamental);
            third = new Note(nThird);
            fifth = new Note(nFifth);
            seventh = new Note(nSeventh);
        }

        public Chord(Note nFundamental, Note nThird, Note nFifth, Note nSeventh = null)
        {
            fundamental = nFundamental;
            third = nThird;
            fifth = nFifth;
            seventh = nSeventh;
        }

        //Methods
        public List<Note> ChordNotes(string message = "all")
        {
            List<Note> result = new List<Note>();
            result.Add(fundamental);
            result.Add(third);
            result.Add(fifth);
            result.Add(seventh);
            switch (message)
            {
                case "toPlay":
                    if (seventhOn)
                    { result.Remove(fundamental); }
                    else
                    { result.Remove(seventh); }
                    break;
                default:
                    break;
            }
            return result;
        }


        public string SeventhType()
        {
                if (fundamental.IntervalWith(seventh) == 11) { return "maj7"; }
                if (fundamental.IntervalWith(seventh) == 10) { return "7"; }
                else { return "7 rara"; }
        }

        public string ChordType()
        {
            int intervalThird = fundamental.IntervalWith(third);
            int intervalFifth = fundamental.IntervalWith(fifth);

            switch (intervalFifth)
            {
                case 7:
                    switch(intervalThird)
                    {
                        case int n when n < 3:
                            return "sus2";
                        case 3:
                            return "m";
                        case 4:
                            return "";
                        case int n when n > 4:
                            return "sus4";
                    }
                    break;
                case int n when n < 7:
                    return "dim";
                case int n when n > 8:
                    return "aug";
            }
            return "NoChordTypeFound";
        }

        public bool Contains(Note other)
        {
            foreach(Note note in ChordNotes())
            {
                if (note == other) { return true; }
            }
            return false;
        }
        public Chord Variant(string variationName, Note[] scale)
        {
            switch (variationName)
            {
                case "triad":
                    return this;
                case "sus2":
                    return new Chord(fundamental, third.MoveInScale(-1, scale), fifth, seventh);
                case "sus4":
                    return new Chord(fundamental, third.MoveInScale(1, scale), fifth, seventh);
                case "Mm":
                    if (ChordType() == "")
                    {
                        return new Chord(fundamental, third.Transposed(-1), fifth, seventh);
                    }
                    else
                    {
                        return new Chord(fundamental, third.Transposed(1), fifth, seventh);
                    }
                case "7":
                    seventhOn = true;
                    return this;
                case "7?":
                    seventhOn = true;
                    if (SeventhType() == "7")
                    {
                        seventh = seventh.Transposed(1);
                    }
                    else
                    {
                        seventh = seventh.Transposed(-1);
                    }
                    return this;
                default:
                    return null;
            }
        }

        public string ChordNoteNames()
        {
            string result = "";
            foreach (Note chordNote in ChordNotes())
            {
                result += chordNote.Name() + ", ";
            }
            return result;
        }

        public string ChordName()
        {
            string result;
            result =  fundamental.Name() + ChordType();
            if(seventhOn) { result += SeventhType(); }
            return result;
        }
    }








    // 0   1   2   3   4   5   6   7   8    9   10  11  12
    // do  do# re  mib mi  fa  fa# sol sol# la  sib si
}
