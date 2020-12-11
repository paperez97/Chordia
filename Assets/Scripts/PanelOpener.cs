using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PanelOpener : MonoBehaviour
{
    public GameObject panel;
    public Toggle toggle;

    public void OpenPanel(bool isOpen)
    {
        if (!isOpen)
        { panel.SetActive(false); }
        else { panel.SetActive(true); }
    }
}
  

