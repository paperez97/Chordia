using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class Song : MonoBehaviour
{
    //Harmony
    public ChordBlob activeChordBlob;
    public SavedPattern selectedPattern;
    public SavedPattern playingPattern;
    public List<ChordBlob> chordBlobsOnTheTable = new List<ChordBlob>();
    public int[] scaleType = Music.major;
    public Music.Note[] scale;
    public Music.Note key = new Music.Note(0);


    //Tempo
    public int stepPattern;
    public int tempo;
    public float tempoUnit = 0;
    public float swingTempoUnit1;
    public float swingTempoUnit2;
    public bool swing;


    //Instrument
    public Synth synth;
    public Instrumento interprete;
    public Instrumento drums;
    public List<int> teclasATocar = new List<int>();
    public List<int> bajosATocar = new List<int>();
    public List<int> drumsATocar = new List<int>();
    public int octaves = 2;

    //Behaviour
    public PatternEditor patternEditor;
    public List<int> teclasAcorde;
    public ChordCreator chordCreator;
    public PatternSaver patternSaver;
    public Instrumento ins;
    SavedInfo savedInfo;
    string thisShouldBeAFile;

    //Events
    public event EventHandler OnRefreshUI;

    //Methods

    public void GenerateSong(int nKey, int nTempo, int nScaleType, Instrumento nInterprete, int[] nChords)
    {
        ChangeKey(nKey);
        ChangeScaleType(nScaleType);
        ChangeInterprete(nInterprete);
        ChangeTempo(nTempo);
        foreach (int nChord in nChords) chordCreator.CreateChord(nChord);
        playingPattern = patternSaver.ReturnNewPattern();
        ChangePattern(playingPattern);
    }

    public void Save()
    {
        //guardamos los grados que tenemos en una variable
        int[] degreesOnTheTable = new int[chordBlobsOnTheTable.Count];
        for(int i = 0; i < chordBlobsOnTheTable.Count; i++)
        {
            degreesOnTheTable[i] = chordBlobsOnTheTable[i].degree;
        }
        //Copiamos la info de los patterns que hay, todo en una variable
        List<int>[,] patternsOnTheTable = new List<int>[patternSaver.transform.childCount, 8];
        for(int i = 0; i < patternSaver.transform.childCount; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                patternsOnTheTable[i,j] = patternSaver.transform.GetChild(i).GetComponent<SavedPattern>().pattern[j];
            }
        }
        savedInfo = new SavedInfo(key.number, tempo, degreesOnTheTable, patternsOnTheTable);
        thisShouldBeAFile = JsonUtility.ToJson(savedInfo);
        
    }

    public void Load()
    {
        patternSaver.DestroyAllPatterns();
        foreach (ChordBlob chord in chordBlobsOnTheTable)
        {
            chordBlobsOnTheTable.Remove(chord);
            Destroy(chord.gameObject);
        }
        SavedInfo loadedInfo = savedInfo;
        key = new Music.Note(loadedInfo.key);
        tempo = loadedInfo.tempo;
        foreach(int chordDegree in loadedInfo.chords)
        {
            chordCreator.CreateChord(chordDegree);
            Debug.Log("Acorde creado " + chordDegree);
        }
        for (int i = 0; i < loadedInfo.pattern.GetLength(0); i++)
        {
            List<int>[] copiedPattern = new List<int>[8];
            for (int j = 0; j < loadedInfo.pattern.GetLength(1); j++)
            {
                copiedPattern[j] = loadedInfo.pattern[i, j];
            }
            SavedPattern loadedPattern = patternSaver.ReturnNewPattern();
            loadedPattern.pattern = copiedPattern;
        }
        OnRefreshUI?.Invoke(this, EventArgs.Empty);
    }


    public void ChangeSwing(bool isSwing)
    {
        swing = isSwing;
        Time.fixedDeltaTime = tempoUnit;
    }
    public void ChangeKey(int newKey)
    {
        Debug.Log("Key changed: " + key.Name() + " to " + new Music.Note(newKey).Name()); ;
        key = new Music.Note(newKey);
        scale = Music.NotesOfScale(key, scaleType);
        OnRefreshUI?.Invoke(this, EventArgs.Empty);
    }
    public void ChangeTempo(int newTempo)
    {
        tempo = newTempo;
        tempoUnit = 1f / (newTempo / 60) / 2;
        swingTempoUnit1 = 4f / 3f * tempoUnit;
        swingTempoUnit2 = 2f / 3f * tempoUnit;
        Time.fixedDeltaTime = tempoUnit;
        OnRefreshUI?.Invoke(this, EventArgs.Empty);
    }
    public void ChangeScaleType(int nScaleType)
    { 
        switch (nScaleType)
        {
            case 0:
                scaleType = Music.escalaJonica;
                break;
            case 1:
                scaleType = Music.escalaEolica;
                break;
            case 2:
                scaleType = Music.escalaLidia;
                break;
            case 3:
                scaleType = Music.escalaMixolidia;
                break;
            case 4:
                scaleType = Music.escalaDorica;
                break;
            case 5:
                scaleType = Music.escalaFrigia;
                break;
            case 6:
                scaleType = Music.escalaLocria;
                break;
        }
        OnRefreshUI?.Invoke(this, EventArgs.Empty);
    }
    public void ChangeInterprete (Instrumento nInterprete)
    {
        interprete = nInterprete;
        OnRefreshUI?.Invoke(this, EventArgs.Empty);
    }
    public void ChangePattern (SavedPattern nPattern)
    {
        selectedPattern = nPattern;
        OnRefreshUI?.Invoke(this, EventArgs.Empty);
    }
    public void ChangeActiveChord(ChordBlob nChord)
    {
        if (activeChordBlob != null && activeChordBlob != nChord)
        {
            activeChordBlob.SetOff();
        }
        activeChordBlob = nChord;
    }
    public void ChangeBeats(float nBeats)
    {
        selectedPattern.beats = nBeats;
        OnRefreshUI?.Invoke(this, EventArgs.Empty);
    }
    public int StepPatternRemainder()
    {
        return stepPattern % 2;
    } 
    public void InvokeRefresh()
    {
        OnRefreshUI?.Invoke(this, EventArgs.Empty);
    }


    private void Start()
    {
        GenerateSong(0, 120, 0, ins, new int[] { 1, 6, 7 });
    }

    void FixedUpdate()
    {
        //Avanza stepPattern, y lo vuelve a 0 si se pasa de beats
        stepPattern++;
        if (stepPattern >= playingPattern.beats) { stepPattern = 0; }
        if (playingPattern == null) playingPattern = selectedPattern;
        if (selectedPattern != playingPattern)
        {
            if (stepPattern == 0)
            {
                playingPattern.GetComponent<Animator>().SetBool("Blinking", false);
                playingPattern = selectedPattern;
            }
            else { playingPattern.GetComponent<Animator>().SetBool("Blinking", true); }
        }

        //ponemos el swing que toque en función de stepPattern
        if(swing)
        {
            if(stepPattern%2 == 0)
            {
                Time.fixedDeltaTime = swingTempoUnit1;
            }
            else
            {
                Time.fixedDeltaTime = swingTempoUnit2;
            }
        }

        //Percusión
        //drumsATocar.Clear();
        //foreach (int drum in playingPattern.percPattern[stepPattern])
        //{
        //    drumsATocar.Add(drum);
        //}
        //drums.TocarBajos(drumsATocar);
            
        if (activeChordBlob != null)
        {
            //necesitamos los ints de las teclas del telcado que tocar
            teclasAcorde = Music.TeclasOfChord(activeChordBlob.chord, octaves);

            //Metemos a teclasATocar las teclas que requiere este stepPattern
            teclasATocar.Clear();
            bajosATocar.Clear();
            foreach (int note in playingPattern.pattern[stepPattern])
            {
                if (note < 0) { bajosATocar.Add(activeChordBlob.bass.number); }
                else { teclasATocar.Add(teclasAcorde[note]); }
            }

            //Tocamos las notas que tocan en esta parte del pattern
            synth.Silence();
            if (interprete.nombre == "Synth")
            {
                synth.TocarNotas(teclasATocar);
                synth.TocarBajos(bajosATocar);
            }
            else
            {
                interprete.TocarNotas(teclasATocar);
                interprete.TocarBajos(bajosATocar);
            }
        }
        
    }

    private class SavedInfo
    {
        public int key;
        public int tempo;
        public int[] chords;
        public List<int>[,] pattern;
        public List<int> beats;
        public SavedInfo(int nKey, int nTempo, int[] nChords, List<int>[,] nPattern)
        {
            key = nKey;
            tempo = nTempo;
            chords = nChords;
            pattern = nPattern;
        }

    }
}
