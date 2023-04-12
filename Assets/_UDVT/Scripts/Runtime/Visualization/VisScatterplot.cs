using System.Linq;
using UnityEngine;

public class VisScatterplot : Vis
{
    public VisScatterplot()
    {
        title = "Scatterplot";

        //Define Data Mark and Tick Prefab
        dataMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/Marks/Sphere");
        tickMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/VisContainer/Tick");
    }

    public override GameObject CreateVis(GameObject container)
    {
        base.CreateVis(container);

        //## 01:  Create Axes and Grids

        // X Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(0).Key, dataSets[0].ElementAt(0).Value, Direction.X);
        visContainer.CreateGrid(Direction.X, Direction.Y);

        // Y Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(1).Key, dataSets[0].ElementAt(1).Value, Direction.Y);

        // Z Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(2).Key, dataSets[0].ElementAt(2).Value, Direction.Z);
        visContainer.CreateGrid(Direction.Y, Direction.Z);
        visContainer.CreateGrid(Direction.Z, Direction.X);

        //## 02: Set Remaining Vis Channels (Color,...)
        visContainer.SetChannel(VisChannel.XPos, dataSets[0].ElementAt(0).Value);
        visContainer.SetChannel(VisChannel.YPos, dataSets[0].ElementAt(1).Value);
        visContainer.SetChannel(VisChannel.ZPos, dataSets[0].ElementAt(2).Value);
        visContainer.SetChannel(VisChannel.Color, dataSets[0].ElementAt(3).Value);

        //## 03: Draw all Data Points with the provided Channels 
        visContainer.CreateDataMarks(dataMarkPrefab);

        //## 04: Rescale Chart
        visContainerObject.transform.localScale = new Vector3(width, height, depth);

        return visContainerObject;
    }

}
