using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject panel;

    public void OpenPanel(bool isOpen)
    {
        if (!isOpen)
        { panel.SetActive(false); }
        else { panel.SetActive(true); }
    }
}
  

