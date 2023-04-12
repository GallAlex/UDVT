
/// <summary>
/// Abstract class for various file types and their parameters
/// </summary>
public abstract class FileType
{

    private string filePath = "";

    #region Getter/Setter
    public string FilePath { get => filePath; set => filePath = value; }
    #endregion

    public override string ToString()
    {
        string values = "filePath = " + FilePath.ToString() + "\n";

        return base.ToString() + ": \n" + values;
    }
}
