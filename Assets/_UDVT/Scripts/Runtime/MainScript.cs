using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MainScript handles the activities needed at the start of the application.
/// </summary>
public class MainScript : MonoBehaviour
{
    private FileLoadingManager fileLoadingManager;
    private Dictionary<string, double[]> dataSet;

    private Vis vis;

    // Awake is called before Start
    void Awake()
    {
        fileLoadingManager = new FileLoadingManager();
    }

    // Start is called at the beginning of the application
    async void Start()
    {
        LoadAndVisualize();
    }

    void Update()
    {
        // If vis is not null, update the grids
        if (vis != null)
        {
            vis.UpdateGrids();
        }
        
    }

    public async void LoadAndVisualize()
    {
        //## 01: Load Dataset

        string filePath = fileLoadingManager.StartPicker();
        // Application waits for the loading process to finish
        FileType file = await fileLoadingManager.LoadDataset();

        if (file == null) return; //Nothing loaded

        //## 02: Process Dataset

        CsvFileType csvFile = (CsvFileType)file;
        dataSet = csvFile.GetDataSet();


        //## 03: Visualize Dataset

        vis = Vis.GetSpecificVisType(VisType.Scatterplot);
        vis.AppendData(dataSet);
        vis.CreateVis(this.gameObject);
    }
}
