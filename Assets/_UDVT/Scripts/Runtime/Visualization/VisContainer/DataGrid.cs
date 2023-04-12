using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Class DataGrid creates a grid between the specified Axes in the Vis Container.
/// </summary>
public class DataGrid
{
    //Define division of the Grid
    public int xGridSize = 4;
    public int yGridSize = 4;

    // If Offset is found, the Grid will be drawn from min to max container position (extend grid at the beginning and end)
    public bool extendGrid = true;

    private Direction[] gridOrientation = { Direction.X, Direction.Y };
    
    private float xLength = 1.0f;
    private float yLength = 1.0f;

    private float[] xMinMaxC = { 0.0f, 1.0f };
    private float[] yMinMaxC = { 0.0f, 1.0f };

    private float[] xMinMax_OffsetC = { 0.0f, 1.0f };
    private float[] yMinMax_OffsetC = { 0.0f, 1.0f };



    private float xAxisTickSpacing;
    private float yAxisTickSpacing;

    private GameObject gridInstance;
    private MeshFilter filter;

    private Mesh mesh;
    private List<Vector3> vertices;
    private List<int> indicies;

    /// <summary>
    /// Create a 2D Grid with the given Min/Max (Start/End) Coordinates and Divisions for the given VisContainer, facing the specified Axis Directions.
    /// </summary>
    /// <param name="visContainer"></param>
    /// <param name="xMinMaxCoordinates"></param>
    /// <param name="yMinMaxCoordinates"></param>
    /// <param name="axisDirections"></param>
    /// <param name="xDivision"></param>
    /// <param name="yDivision"></param>
    public void CreateGrid(Transform visContainer, Bounds containerBounds, float xOffset, float yOffset, Direction[] axisDirections, int xDivision, int yDivision)
    {
        //Init Values
        //Set Start/End Values of the container bounding box
        xMinMaxC = new[] { containerBounds.min[(int)axisDirections[0]], containerBounds.max[(int)axisDirections[0]] };
        yMinMaxC = new[] { containerBounds.min[(int)axisDirections[1]], containerBounds.max[(int)axisDirections[1]] };

        // If no offset found, there is no need to extend the grid
        if (xOffset == 0.0f && yOffset == 0.0f) extendGrid = false;

        //Set Start/End Values of the Axis Line with Offset
        xMinMax_OffsetC = new[] { xMinMaxC[0] + xOffset, xMinMaxC[1] - xOffset };
        yMinMax_OffsetC = new[] { yMinMaxC[0] + yOffset, yMinMaxC[1] - yOffset };

        //Calculate the lentgh of the Line for the Grid Division
        xLength = (xMinMaxC[1] - xOffset) - (xMinMaxC[0] + xOffset);
        yLength = (yMinMaxC[1] - yOffset) - (yMinMaxC[0] + yOffset);

        gridOrientation = axisDirections;
        
        // Reduce by 1 to get the number of divisions
        xGridSize = xDivision-1;
        yGridSize = yDivision-1;
        
        gridInstance = new GameObject("Grid" + axisDirections[0].ToString() + axisDirections[1].ToString());
        gridInstance.transform.parent = visContainer.transform;

        // Add mesh filter if not present
        filter = gridInstance.GetComponent<MeshFilter>();
        filter = filter != null ? filter : gridInstance.AddComponent<MeshFilter>();

        // Add mesh renderer if not present
        MeshRenderer meshRenderer = filter.GetComponent<MeshRenderer>();
        meshRenderer = meshRenderer != null ? meshRenderer : gridInstance.AddComponent<MeshRenderer>();

        mesh = new Mesh();

        meshRenderer.material = new Material(Shader.Find("Diffuse"));
        //meshRenderer.material.color = Color.white;

        if (extendGrid) BuildFromOrigin();
        else Build();
    }

    /// <summary>
    /// Return the Gameobject of the grid
    /// </summary>
    /// <returns></returns>
    public GameObject GetGridObject()
    {
        return gridInstance;
    }

    /// <summary>
    /// Builds a grid mesh with lines. Starts at min x/y Coordinate with included Offset.
    /// If extendGrid is true start grid drawing at min x/y coordinate of container and start spacing at min x/y Coordinate with offset.
    /// </summary>
    private void Build()
    {
        xAxisTickSpacing = GetXAxisTickSpacing();
        yAxisTickSpacing = GetYAxisTickSpacing();

        vertices = new List<Vector3>();
        indicies = new List<int>();

        int currentVertexId = 0;
        
        for (int y = 0; y <= yGridSize; y++)
        {
            for (int x = 0; x <= xGridSize; x++)
            {
                float xPos = x * xAxisTickSpacing + xMinMax_OffsetC[0];
                float yPos = y * yAxisTickSpacing + yMinMax_OffsetC[0];

                vertices.Add(CreateGridInDirection(xPos, yPos));

                if (x != xGridSize)
                {
                    indicies.Add(currentVertexId);
                    indicies.Add(currentVertexId + 1);
                }
                if (y != yGridSize)
                {
                    indicies.Add(currentVertexId);
                    indicies.Add(currentVertexId + xGridSize + 1);
                }

                currentVertexId++;
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.SetIndices(indicies.ToArray(), MeshTopology.Lines, 0);
        filter.mesh = mesh;

    }

    /// <summary>
    /// Builds a grid mesh with lines. Starts grid drawing at min x/y coordinate of container and start spacing/division at min x/y Coordinate with offset.
    /// </summary>
    private void BuildFromOrigin()
    {
        xAxisTickSpacing = GetXAxisTickSpacing();
        yAxisTickSpacing = GetYAxisTickSpacing();

        vertices = new List<Vector3>();
        indicies = new List<int>();

        int xExtendedGridSize = xGridSize + 1;
        int yExtendedGridSize = yGridSize + 1;

        int currentVertexId = 0;

        for (int y = -1; y <= yExtendedGridSize; y++)
        {
            for (int x = -1; x <= xExtendedGridSize; x++)
            {

                float xPos = x * xAxisTickSpacing + xMinMax_OffsetC[0];
                float yPos = y * yAxisTickSpacing + yMinMax_OffsetC[0];

                // ## Additional Vetrtice before the Offset ##
                if (x < 0)
                {
                    xPos = xMinMaxC[0];
                }
                else if (x == xExtendedGridSize)
                {
                    xPos = xMinMaxC[1];
                }

                if (y < 0)
                {
                    yPos = yMinMaxC[0];
                }
                else if (y == yExtendedGridSize)
                {
                    yPos = yMinMaxC[1];
                }
                // ##########################################

                
                vertices.Add(CreateGridInDirection(xPos, yPos));

                if (x != xExtendedGridSize)
                {
                    indicies.Add(currentVertexId);
                    indicies.Add(currentVertexId + 1);
                }
                if (y != yExtendedGridSize)
                {
                    indicies.Add(currentVertexId);
                    indicies.Add(currentVertexId + xExtendedGridSize + 2);
                }

                currentVertexId++;
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.SetIndices(indicies.ToArray(), MeshTopology.Lines, 0);
        filter.mesh = mesh;
    }

    /// <summary>
    /// Methods creates vector for the vertices of the grid. The position depends on the two axes between the grid should be drawn.
    /// </summary>
    /// <param name="xPos"></param>
    /// <param name="yPos"></param>
    /// <returns></returns>
    private Vector3 CreateGridInDirection(float xPos, float yPos)
    {
        Direction axisDir1 = gridOrientation[0];
        Direction axisDir2 = gridOrientation[1];

        //Grid XY
        if (axisDir1 == Direction.X && axisDir2 == Direction.Y)
        {
            return new Vector3(xPos, yPos, 0);
        }
        else if (axisDir2 == Direction.X && axisDir1 == Direction.Y)
        {
            return new Vector3(yPos, xPos, 0);
        }
        
        //Grid XZ
        if (axisDir1 == Direction.X && axisDir2 == Direction.Z)
        {
            return new Vector3(xPos, 0 , yPos);
        }
        else if(axisDir2 == Direction.X && axisDir1 == Direction.Z)
        {
            return new Vector3(yPos, 0, xPos);
        }
        
        //Grid ZY
        if ((axisDir1 == Direction.Z && axisDir2 == Direction.Y))
        {
            return new Vector3(0, yPos, xPos);
        }
        else if (axisDir2 == Direction.Z && axisDir1 == Direction.Y)
        {
            return new Vector3(0, xPos, yPos);
        }

        //Use Grid XY
        return new Vector3(xPos, yPos, 0);
    }


    /// <summary>
    /// Calculates the number of vertices in the mesh where adjacent quads share the same vertex.
    /// </summary>
    /// <returns>Number of vertices in the grid mesh</returns>
    public int GetNumberOfVertices()
    {
        return (xGridSize + 1) * (yGridSize + 1); 
    }

    private float GetXAxisTickSpacing()
    {
        return xLength / (float)xGridSize;
    }

    private float GetYAxisTickSpacing()
    {
        return yLength / (float)yGridSize;
    }

}
