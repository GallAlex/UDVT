using UnityEngine;

public enum Direction
{
    X, // From 0 to +x
    Y, // From 0 to +y 
    Z, // From 0 to +z 
}

/// <summary>
/// Class represents an Axis of a VisContainer with its prefab, scale and ticks
/// </summary>
public class DataAxis
{
    [SerializeField]
    private GameObject axisLinePrefab;  // Prefab of the axis

    [SerializeField]
    private GameObject axisInstance;    // The concrerte instance of the axis
    [SerializeField]
    private AxisTicks axisTicks;        // The ticks of the axis
    [SerializeField]
    private TextMesh axisLabel;         // The label of the axis

    public Direction axisDirection = Direction.X;   // The direction of the axis
    public Scale dataScale;                         // The calculated scale of the axis

    private float tickOffset;
    private float tickLabelOffset = 0.03f;

    public DataAxis(float tickOffset)
    {
        this.tickOffset = tickOffset;
    }

    /// <summary>
    /// Main method to create an axis. The axis is instatiated with the given prefab and added to the visContainer as child.
    /// The axis then creates the ticks with the provided scale. The axis is then rotated to the given direction (X/Y/Z).
    /// </summary>
    /// <param name="visContainer"></param>
    /// <param name="axisTitle"></param>
    /// <param name="dataScale"></param>
    /// <param name="direction"></param>
    /// <param name="numberOfTicks"></param>
    /// <returns></returns>
    public GameObject CreateAxis(Transform visContainer, string axisTitle, Scale dataScale, Direction direction, int numberOfTicks)
    {
        axisLinePrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/VisContainer/Axis");

        axisInstance = GameObject.Instantiate(axisLinePrefab, visContainer.position, Quaternion.identity, visContainer);

        axisLabel = axisInstance.GetComponentInChildren<TextMesh>();
        axisLabel.text = axisTitle;

        this.dataScale = dataScale;

        axisDirection = direction;
        axisInstance.name = axisDirection.ToString() + " Axis";

        CreateAxisTicks(axisInstance.transform, dataScale, numberOfTicks);

        ChangeAxisDirection(axisInstance, direction);

        return axisInstance;
    }

    /// <summary>
    /// Method creates the set amount of ticks on the Axis based on the given scale.
    /// If numberOfTicks is zero no tick will be created, otherwise the set amount of ticks (>=2) will be drawn
    /// </summary>
    /// <param name="axisTransform"></param>
    /// <param name="dataScale"></param>
    /// <param name="numberOfTicks">Amount of ticks inbetween min/max tick</param>
    /// <returns></returns>
    private AxisTicks CreateAxisTicks(Transform axisTransform, Scale dataScale, int numberOfTicks)
    {
        axisTicks = new AxisTicks(tickOffset);
        if (numberOfTicks == 1) numberOfTicks = 2;

        axisTicks.CreateTicks(axisTransform, dataScale, numberOfTicks);

        return axisTicks;
    }

    /// <summary>
    /// Method changes the axis and its ticks to the new given parameters (scaling).
    /// </summary>
    /// <param name="axisTitle"></param>
    /// <param name="dataScale"></param>
    /// <param name="numberOfTicks"></param>
    public void ChangeAxis(string axisTitle, Scale dataScale, int numberOfTicks)
    {
        this.dataScale = dataScale;

        axisLabel.text = axisTitle;
        if (numberOfTicks == 1) numberOfTicks = 2;
        axisTicks.ChangeTicks(this.dataScale, numberOfTicks);
    }

    /// <summary>
    /// The method rotates the axis, its labels and its ticks to the desired the direction.
    /// Default Orientation of the prefab is a X-Axis.
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="dir"></param>
    private void ChangeAxisDirection(GameObject axis, Direction dir)
    {
        switch (dir)
        {
            case Direction.X:
                //Nothing To do
            break;

            case Direction.Y:
                axis.transform.Rotate(0, 0, 90);    //Rotate 90 degree in z
                Transform yAxisTitle = axis.transform.Find("Title");
                yAxisTitle.localPosition = new Vector3(yAxisTitle.localPosition.x, (-yAxisTitle.localPosition.y), yAxisTitle.localPosition.z);

                foreach (var tickObject in axisTicks.tickList)
                {
                    Transform tickLabel = tickObject.transform.Find("TickLabel");
                    tickLabel.localPosition = new Vector3(tickLabel.localPosition.x, -tickLabel.localPosition.y, tickLabel.localPosition.z);

                    TextMesh yTickLabelsTextMesh = tickObject.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
                    yTickLabelsTextMesh.anchor = TextAnchor.MiddleRight;
                }
                break;

            case Direction.Z:
                axis.transform.Rotate(0, -90, 0);    //Rotate 90 degree in y
                //Transform zAxisTitle = axis.transform.Find("Title");
                //zAxisTitle.localPosition = new Vector3(-zAxisTitle.localPosition.x, zAxisTitle.localPosition.y, zAxisTitle.localPosition.z);
                //zAxisTitle.Rotate(0, 180, 0);

                TextMesh titleTextMesh = axis.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
                titleTextMesh.transform.Rotate(0, 180, 0);
                titleTextMesh.anchor = TextAnchor.MiddleRight;

                
                foreach (var tickObject in axisTicks.tickList)
                {
                    Transform tickLabel = tickObject.transform.Find("TickLabel");
                    tickLabel.Rotate(0, 180, 0);
                }
                break;

            default:
                //Use X
                break;
        }
    }

}
