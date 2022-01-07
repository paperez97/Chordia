using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Song : MonoBehaviour
{
    //Harmony
    public ChordBlob activeChordBlob;
    public SavedPattern selectedPattern;
    public SavedPattern playingPattern;
    public List<ChordBlob> chordBlobsOnTheTable = new List<ChordBlob>();
    public int[] scaleType = Music.escalaJonica;
    public Music.Note key = new Music.Note(0);
    public Music.Note[] scale;

    //Tempo
    public int stepPattern;
    public float tempo;
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
    public ChordBlobManager chordManager;
    public PatternSaver patternSaver;
    public Instrumento ins;
    SavedInfo savedInfo;
    string json;

    //Events
    public event EventHandler OnRefreshUI;

    //Saving
    public static string saveDirectory = "/saves/";
    public static string fileName = "";

    //Methods

    public void GenerateSong(int nKey, int nTempo, int nScaleType, Instrumento nInterprete, int[] nChords)
    {
        ChangeKey(nKey);
        ChangeScaleType(nScaleType);
        ChangeInterprete(nInterprete);
        ChangeTempo(nTempo);
        foreach (int nChord in nChords) chordManager.CreateChord(nChord);
        playingPattern = patternSaver.ReturnNewPattern();
        ChangePattern(playingPattern);
    }

    public void Save()
    {
        if(fileName == "")
        {
            Debug.Log("Write a name for the save file");
            return;
        }
        //guardamos los grados que tenemos en una variable
        int[] degreesOnTheTable = new int[chordBlobsOnTheTable.Count];
        for(int i = 0; i < chordBlobsOnTheTable.Count; i++)
        {
            degreesOnTheTable[i] = chordBlobsOnTheTable[i].degree;
        }
        //Copiamos la info de los patterns que hay, todo en una variable
        List<List<int>[]> patternsOnTheTable = new List<List<int>[]>();
        List<float> beats = new List<float>();
        for(int i = 0; i < patternSaver.transform.childCount; i++)
        {
            patternsOnTheTable.Add(patternSaver.transform.GetChild(i).GetComponent<SavedPattern>().pattern);
            beats.Add(patternSaver.transform.GetChild(i).GetComponent<SavedPattern>().beats);
        }
        savedInfo = new SavedInfo(key.number, tempo, degreesOnTheTable, ExportPatternListToStrings(patternsOnTheTable), beats);
        json = JsonUtility.ToJson(savedInfo);

        //File management
        string dir = Application.persistentDataPath + saveDirectory;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        File.WriteAllText(dir + fileName, json);
        Debug.Log(json);
    }

    public void Load(string fullPath)
    {

        //File management
        if (!File.Exists(fullPath))
        {
            Debug.Log("Save file does not exist");
            return;
        }
        string json = File.ReadAllText(fullPath);
        Debug.Log(json);
        SavedInfo loadedInfo = JsonUtility.FromJson<SavedInfo>(json);

        //Destruimos todo lo que hay
        patternSaver.DestroyAllPatterns();
        foreach(ChordBlob chordBlob in chordBlobsOnTheTable)
        {
            Destroy(chordBlob.gameObject);
        }
        chordBlobsOnTheTable = new List<ChordBlob>();

        //Establecemos las variables con los nuevos valores
        key = new Music.Note(loadedInfo.key);
        tempo = loadedInfo.tempo;

        //Creamos nuevos acordes con los degrees guardados
        foreach (int chordDegree in loadedInfo.chords)
        {
            chordManager.CreateChord(chordDegree);
        }

        //Creamos nuevos patterns con la info guardada
        for (int i = 0; i < loadedInfo.patterns.Count; i++)
        {
            //Variable temporal donde creo el pattern a partir de la info lodeada
            List<int>[] copiedPattern = ExportStringsToPatternList(loadedInfo.patterns)[i];
            SavedPattern loadedPattern = patternSaver.ReturnNewPattern();
            loadedPattern.pattern = copiedPattern;
            loadedPattern.beats = loadedInfo.beats[i];
        }
        OnRefreshUI?.Invoke(this, EventArgs.Empty);

    }


    public void ChangeFileName(string nFileName)
    {
        fileName = nFileName + ".json";
    }
    public void ChangeSwing(bool isSwing)
    {
        swing = isSwing;
        Time.fixedDeltaTime = tempoUnit;
    }
    public void ChangeKey(int newKey)
    {
        key = new Music.Note(newKey);
        Debug.Log("Scale should be made now");
        scale = Music.NotesOfScale(key, scaleType);
        Debug.Log("Scale should have been made now. Size: " + scale.Length);
        OnRefreshUI?.Invoke(this, EventArgs.Empty);
    }
    public void ChangeTempo(float newTempo)
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


    public List<string> ExportPatternListToStrings(List<List<int>[]> patternList)
    {
        List<string> result = new List<string>();
        foreach (List<int>[] pattern in patternList)
        {
            string thisPattern = "";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < pattern[i].Count; j++)
                {
                    thisPattern += pattern[i][j];
                }
                thisPattern += "-";
            }
            Debug.Log(thisPattern);
            result.Add(thisPattern);
        }
        return result;
    }

    public List<List<int>[]> ExportStringsToPatternList(List<string> patternStrings)
    {
        List<List<int>[]> result = new List<List<int>[]>();
        foreach (string patternString in patternStrings)
        {
            List<int>[] pattern = new List<int>[]
            {   new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>()
            };
            int patternBeat = 0;
            foreach (char character in patternString)
            {
                if ( patternBeat >= 8) break;
                if (character == '-')
                {
                    patternBeat++;
                }
                else
                {
                    pattern[patternBeat].Add((int)Char.GetNumericValue(character));
                }
            }
            result.Add(pattern);
        }
        return result;
    }

    private void Start()
    {
        ChangeKey(key.number);
        GenerateSong(0, 120, 0, ins, new int[] {});

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
    [Serializable]
    private class SavedInfo
    {
        public int key;
        public float tempo;
        public int[] chords;
        public List<string> patterns;
        public List<float> beats;
        public SavedInfo(int nKey, float nTempo, int[] nChords, List<string> nPattern, List<float> nBeats)
        {
            key = nKey;
            tempo = nTempo;
            chords = nChords;
            patterns = nPattern;
            beats = nBeats;
        }

    }
}
