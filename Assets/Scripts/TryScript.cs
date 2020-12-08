using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryScript : MonoBehaviour
{

    List<int>[] listaDeListas;
    // Start is called before the first frame update
    void Start()
    {
        listaDeListas = new List<int>[] { new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>() };
        Debug.Log(listaDeListas[0]);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
