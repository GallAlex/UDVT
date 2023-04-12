using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Concrete implementation of FileLoader for CSV Files
/// </summary>
public class CsvLoader : FileLoader
{
    private CsvFileType csvFile;
    private List<List<string>> csvValues;
        
    private Encoding encoding;              // Text encoding of the file
    private char splitChar = ',';           // Character used to split the values in the file
    private int skipRows = 0;               // Number of rows to skip at the beginning of the file

    /// <summary>
    /// Default constructor
    /// </summary>
    public CsvLoader()
    {

    }

    /// <summary>
    /// Constructor with rows to skip at the beginning of the file
    /// </summary>
    /// <param name="skipRows"></param>
    public CsvLoader(int skipRows)
    {
        this.skipRows = skipRows;
    }

    /// <summary>
    /// Loads a CSV file by skipping meata infos, checking if a header exists and stores the values in a List.
    /// Each entry in the List corresponds to an Attribute (column) with a List of all values (rows) for this attribute.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public override async Task LoadData(string filePath)
    {
        Task<StreamReader> streamReaderTask = GetStreamReader(filePath);
        using var reader = await streamReaderTask;
        encoding = reader.CurrentEncoding;

        csvValues = new List<List<string>>();

        // Skip first rows
        for (int skip = 0; skip < skipRows; skip++)
        {
            await reader.ReadLineAsync();
        }

        string headerLine = await reader.ReadLineAsync();
        string[] headerNames = headerLine.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);

        if (headerNames == null || headerNames.Length < 1)
        {
            Debug.LogError("CSV File Header row is empty");
        }

        // Get header names from first row
        foreach (var name in headerNames)
        {
            var trimmedName = name.Trim(' '); //Remove leading and trailing spaces
            csvValues.Add(new List<string> { trimmedName });
        }


        // Get next rows and assign value to specific column (header)
        while (!reader.EndOfStream)
        {
            string line = await reader.ReadLineAsync();
            string[] values = line.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);

            if (values == null || values.Length < 1)
            {
                Debug.LogError("CSV File has no value row");
            }

            for (int feature = 0; feature < csvValues.Count; feature++)
            {
                csvValues[feature].Add(values[feature]);
            }

        }


        Debug.Log("CSV File loaded");
        // Prints the read csv file
        PrintCsv();
        
        csvFile = new CsvFileType(csvValues);
    }

    public override FileType GetFile()
    {
        return csvFile;
    }

    /// <summary>
    /// Prints the csv with the header as as first row
    /// </summary>
    public void PrintCsv()
    {
        string csvOutput = "";

        for (int rowIndex = 0; rowIndex < csvValues[0].Count; rowIndex++)
        {
            csvOutput += "| ";

            for (int columnsIndex = 0; columnsIndex < csvValues.Count; columnsIndex++)
            {
                csvOutput += csvValues[columnsIndex][rowIndex] + "\t | ";
            }

            csvOutput += " \n";
        }

        Debug.Log("CSV Output [" + encoding + "]: \n" + csvOutput);
    }

}