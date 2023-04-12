using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UDVTMenu : MonoBehaviour
{

    void Awake()
    {

    }

    [MenuItem("UDVT/Load Data")]
    static void LoadData()
    {
        MainScript main = FindFirstObjectByType<MainScript>();
        main.LoadAndVisualize();
    }
}
