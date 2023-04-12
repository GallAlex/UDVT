using UnityEngine;

/// <summary>
/// The class DataMark represents a single data point in the visualization space.
/// It is defined by the channels which desribe its geometric properties and a prefab that defines it visual appearance.
/// </summary>
public class DataMark
{
    private readonly int markID = 0;    // Unique ID of the data mark
    
    public struct Channel
    {
        public Vector3 position;        // Local position
        public Vector3 rotation;        // Local rotation
        public Vector3 size;            // width, height, depth;
        public Vector3 facing;          // Direction the object is facing
        public Vector4 color;           // Color of the object

    }

    public bool selected = false;       // is the object selected through an interaction?

    private Channel dataChannel;        // Properties of the data mark
    
    [SerializeField]
    private GameObject dataMarkPrefab;
    private GameObject dataMarkInstance;
    private MeshRenderer meshRenderer;

    /// <summary>
    /// Creates a new DataMark instance with a default prefab and an unique ID
    /// </summary>
    /// <param name="iD"></param>
    public DataMark(int iD)
    {
        if (this.dataMarkPrefab == null) this.dataMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/Marks/Sphere");

        markID = iD;
        dataChannel = DefaultDataChannel();
    }

    /// <summary>
    /// Creates a new DataMark instance with a given prefab and an unique ID
    /// </summary>
    /// <param name="iD"></param>
    /// <param name="dataMarkPrefab"></param>
    public DataMark(int iD, GameObject dataMarkPrefab)
    {
        if (dataMarkPrefab == null)
        {
            this.dataMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/Marks/Sphere");
        }
        else this.dataMarkPrefab = dataMarkPrefab;

        markID = iD;
        dataChannel = DefaultDataChannel();
    }

    /// <summary>
    /// Returns the unique ID of the data mark
    /// </summary>
    /// <returns></returns>
    public int GetDataMarkId()
    {
        return markID;
    }

    /// <summary>
    /// Returns a prefilled channel instance
    /// </summary>
    /// <returns></returns>
    public static Channel DefaultDataChannel()
    {
        DataMark.Channel channel = new DataMark.Channel
        {
            position = new Vector3(0, 0, 0),
            rotation = new Vector3(0, 0, 0),
            facing = new Vector3(0, 0, -1),
            color = new Vector4(1, 0, 0, 1),
            size = new Vector3(0.03f, 0.03f, 0.03f)
        };

        return channel;
    }

    /// <summary>
    /// Main method to create a new data mark instance with the provided properties (channel) as a child of the given visContainer.
    /// </summary>
    /// <param name="visContainer"></param>
    /// <param name="channel"></param>
    /// <returns></returns>
    public GameObject CreateDataMark(Transform visContainer, Channel channel)
    {
        dataMarkInstance = GameObject.Instantiate(dataMarkPrefab, channel.position, Quaternion.Euler(channel.rotation), visContainer);
        dataMarkInstance.name = dataMarkPrefab.name+ "_" + markID;
        meshRenderer = dataMarkInstance.GetComponent<MeshRenderer>();

        // Get initial data of object
        this.dataChannel = channel;

        SetColor(dataChannel.color);
        SetSize(dataChannel.size);
        SetRot(dataChannel.rotation);

        return dataMarkInstance;
    }

    public Channel GetDataMarkChannel()
    {
        return dataChannel;
    }

    public GameObject GetDataMarkInstance()
    {
        return dataMarkInstance;
    }
    
    public void ChangeDataMark(Channel channel)
    {
        SetPos(channel.position);
        SetSize(channel.size);
        //SetFacing(channel.facing);
        SetRot(channel.rotation);
        SetColor(channel.color);
    }

    public void SetPos(Vector3 position)
    {
        dataChannel.position = position;
        dataMarkInstance.transform.localPosition = dataChannel.position;
    }

    public void SetFacing(Vector3 facingDir)
    {
        dataChannel.facing = facingDir;
    }

    public void SetRot(Vector3 rotation)
    {
        dataChannel.rotation = rotation;
    }

    /// <summary>
    /// Sets the size of the data mark and adjusts the y position to the bottom of the object.
    /// </summary>
    /// <param name="size"></param>
    public void SetSize(Vector3 size)
    {
        dataChannel.size = size;
        dataMarkInstance.transform.localScale = dataChannel.size;

        //Pivot is in Center of Object
        float adjustedHeight = dataChannel.size.y / 2.0f;
        dataMarkInstance.transform.localPosition = new Vector3(dataChannel.position.x, dataChannel.position.y + adjustedHeight, dataChannel.position.z);
    }

    public void SetColor(Vector4 color)
    {
        dataChannel.color = color;
        meshRenderer.material.SetColor("_Color", color);
    }
}
