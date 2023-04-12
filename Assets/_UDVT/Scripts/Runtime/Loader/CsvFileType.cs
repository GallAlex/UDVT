using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

/// <summary>
/// Conrete FileType for CSV Files
/// </summary>
public class CsvFileType : FileType
{
    // First row consists of the header
    private List<List<string>> csvValues;
    //Stores for every dimension (i.e., attributes or header names) an array of its values
    private Dictionary<string, double[]> dataSet;

    private int dimensions = 0;

    #region Getter/Setter
    public List<List<string>> CsvValues
    {
        get => csvValues;
        set => csvValues = value;
    }
    #endregion

    /// <summary>
    /// List of possible value types of the data
    /// </summary>
    public enum DataValueType
    {
        Undefined,
        Int,
        Double,
        Float,
        Bool,
        String,
        NumberOfDataValueTypes
    }

    public CsvFileType(List<List<string>> csvValues)
    {
        this.csvValues = csvValues;
    }

    public Dictionary<string, double[]> GetDataSet()
    {
        if (dataSet != null)
        {
            return dataSet;
        }
        else
        {
            return ParseCsvFileAsDouble();
        }
        
    }

    /// <summary>
    /// Parse File and store all numerical Values as double in Dictionary
    /// Removes all non numerical values!
    /// </summary>
    private Dictionary<string, double[]> ParseCsvFileAsDouble()
    {
        dimensions = csvValues.Count;
        int numberOfValuesInFirstColumn = 0;
        List<int> columnsWithoutNumbers = new List<int>();
        
        dataSet = new Dictionary<string, double[]>(dimensions);
        
        for (int column = 0; column < dimensions; column++)
        {
            string[] valuesWithoutHeader = csvValues[column].GetRange(1, csvValues[column].Count - 1).ToArray();

            // Get amount of values in first column
            if (column == 0) numberOfValuesInFirstColumn = valuesWithoutHeader.Length;

            if (valuesWithoutHeader.Length <= 0 || valuesWithoutHeader.Length - numberOfValuesInFirstColumn != 0)
            {
                Debug.LogError("Number of data values do not match (Missing Values!)");
                return null;
            }

            //Check first value if it can be represented by double number
            if (double.TryParse(valuesWithoutHeader[0], out double doubleValue))
            {
                double[] valuesOfColumn = Array.ConvertAll(valuesWithoutHeader, s => double.Parse(s, CultureInfo.InvariantCulture));
                dataSet.Add(csvValues[column][0], valuesOfColumn);
            }

        }

        return dataSet;
    }

    /// <summary>
    /// Method checks the type of the given value (String) from the data in the csv file
    /// Returns the identified data typ as DataValueType
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private DataValueType CheckDataValueType(string value)
    {
        if (int.TryParse(value, out int intValue))
        {
            return DataValueType.Int;
        }
        else if (double.TryParse(value, out double doubleValue))
        {
            return DataValueType.Double;
        }
        else if (float.TryParse(value, out float floatValue))
        {
            return DataValueType.Float;
        }
        else if (bool.TryParse(value, out bool boolValue))
        {
            return DataValueType.Bool;
        }
        else if (!String.IsNullOrEmpty(value))
        {
            return DataValueType.String;
        }
        else
        {
            return DataValueType.Undefined;
        }
    }
}
