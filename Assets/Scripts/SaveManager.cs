using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public GameObject loadButtonPrefab;
    public Transform loadArea;
    public Song song;
    public Toggle SaveLoadPanelToggle;

    public string[] saveFiles;
    
    public void GetLoadFiles()
    {
        if(!Directory.Exists(Application.persistentDataPath + "/saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves/");
        }

        saveFiles = Directory.GetFiles(Application.persistentDataPath + "/saves/");
    }

    public void ShowLoadScreen()
    {
        GetLoadFiles();

        foreach(Transform button in loadArea)
        {
            Destroy(button.gameObject);
        }

        for(int i = 0; i <saveFiles.Length; i++)
        {
            GameObject buttonObject = Instantiate(loadButtonPrefab, loadArea);
            var index = i;
            buttonObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                song.Load(saveFiles[index]);
            });

            FindGameObjectInChildWithTag(buttonObject, "TrashButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                File.Delete(saveFiles[index]);
                Destroy(buttonObject);
                ShowLoadScreen();
            });
            buttonObject.GetComponentInChildren<Text>().text = "  " + saveFiles[i].Replace(Application.persistentDataPath + "/saves/", "").Replace(".json", "");
        }
    }

    public static GameObject FindGameObjectInChildWithTag(GameObject parent, string tag)
    {
        Transform t = parent.transform;

        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).gameObject.tag == tag)
            {
                return t.GetChild(i).gameObject;
            }

        }

        return null;
    }
}
