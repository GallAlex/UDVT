using System.IO;
using System;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;


/// <summary>
/// Abstract class for loader of various files
/// </summary>
public abstract class FileLoader
{
    public abstract Task LoadData(string filePath);
    public abstract FileType GetFile();

    
    public override string ToString()
    {
        return base.ToString() + ": \n";
    }


    /// <summary>
    /// Returns a StreamReader for the given file path
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    protected static async Task<StreamReader> GetStreamReader(string filePath)
    {
        Stream stream = File.OpenRead(filePath);
        StreamReader reader = new StreamReader(filePath, Encoding.GetEncoding(DetectFileEncoding(stream)));

        return reader;
    }

    /// <summary>
    /// Returns a BinaryReader for the given file path
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    protected static async Task<BinaryReader> GetBinaryReader(string filePath)
    {
        BinaryReader reader = new BinaryReader(new FileStream(filePath, FileMode.Open));
        return reader;
    }

    /// <summary>
    /// IO function tpo checks if the given file exists
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    protected static async Task<bool> CheckIfFileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    /// <summary>
    /// Detects the encoding of the given file stream (utf-8, ISO-8859-1)
    /// </summary>
    /// <param name="fileStream"></param>
    /// <returns></returns>
    protected static string DetectFileEncoding(Stream fileStream)
    {
        var Utf8EncodingVerifier = Encoding.GetEncoding("utf-8", new EncoderExceptionFallback(), new DecoderExceptionFallback());
        using (var reader = new StreamReader(fileStream, Utf8EncodingVerifier,
                   detectEncodingFromByteOrderMarks: true, leaveOpen: true, bufferSize: 1024))
        {
            string detectedEncoding;
            try
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                }
                detectedEncoding = reader.CurrentEncoding.BodyName;
            }
            catch (Exception e)
            {
                // Failed to decode the file using the BOM/UT8. 
                // Assume it's local ANSI
                detectedEncoding = "ISO-8859-1";
            }
            // Rewind the stream
            fileStream.Seek(0, SeekOrigin.Begin);
            return detectedEncoding;
        }
    }

}
