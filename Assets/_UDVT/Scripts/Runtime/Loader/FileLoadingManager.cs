using System;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Dataset types which can be loaded
/// </summary>
public enum DatasetType
{
    Csv,
    Unknown
}

/// <summary>
/// Concrete class for loading a file based on its extension and selects the appropriate loader (factory) for it.
/// Loader depends on respective System FilePicker (UWP, Linux,...) and dataset type (raw, csv,...).
/// </summary>
public class FileLoadingManager
{
    private bool loadingSucceded = false;
    private FileLoader loaderFactory;

    private string filePath = "";

    #region Getter/Setter
    public FileLoader LoaderFactory { get => loaderFactory; set => loaderFactory = value; }
    #endregion


    /// <summary>
    /// Asynchronous method for loading a dataset based on a previous loaded filepath.
    /// The method decides which concrete loader to use based on the file extension found in the path.
    /// </summary>
    /// <returns></returns>
    public async Task<FileType> LoadDataset()
    {
        try
        {
            if (filePath == "")
            {
                filePath = "No Data";
                Debug.LogError("Failed to import dataset");
                return null;
            }

            //Checks the DatasetType based on the file extension
            DatasetType fileTyp = GetDatasetType(filePath);

            //Choose concrete Loader here...
            switch (fileTyp)
            {
                default:
                case DatasetType.Csv:
                    loaderFactory = new CsvLoader();
                    loadingSucceded = true;
                    break;
                case DatasetType.Unknown:
                    loadingSucceded = false;
                    break;
            }

            if (!loadingSucceded) return null;

            //.. if DatasetType implemented then load the data
            Debug.Log("LoadData...");
            await Task.Run(() => loaderFactory.LoadData(filePath));

        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            return null;
        }
        
        return loaderFactory.GetFile();
    }

    /// <summary>
    /// Returns the detected extension type of the file
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static DatasetType GetDatasetType(string filePath)
    {
        DatasetType datasetType;

        // Get .* extension
        string extension = Path.GetExtension(filePath);

        switch (extension)
        {
            case ".csv":
                datasetType = DatasetType.Csv;
                break;
            default:
                datasetType = DatasetType.Unknown;
                throw new NotImplementedException("Data extension format [" + extension + "] not supported");
        }

        return datasetType;
    }


    #region FilePickerMethods

    /// <summary>
    /// Starts the file picker based on the current platform (UWP, Linux,...)
    /// </summary>
    /// <returns></returns>
    public string StartPicker()
    {
        #if UNITY_EDITOR
        Debug.Log("UNITY_STANDALONE PICKER");
        return FilePicker_Win();
        #endif

        #if !UNITY_EDITOR
        throw new NotImplementedException("Picker outside of Unity not implemented yet.");
        #endif

    }


#if UNITY_EDITOR
    /// <summary>
    /// Filepicker for Windows/Unity
    /// </summary>
    /// <returns></returns>
    private string FilePicker_Win()
    {
        string path = EditorUtility.OpenFilePanel("Open File...", "", "");
        if (path.Length != 0)
        {
            filePath = path;
        }

        Debug.Log("WIN Picker Path = " + filePath);

        return filePath;
    }

#endif

    #endregion

}
