using System;
using System.IO;
using UnityEngine;

public class FileWriter
{
    public bool TryWriteToFile(string path, string contents, bool allowOverwrite = true)
    {
        if (File.Exists(path))
        {
            if (allowOverwrite)
            {
                Debug.LogWarning($"File at path {path} already exists and will be overwritten.");
            }
            else
            {
                Debug.LogError($"File at path {path} already exists.");
                return false;
            }
        }

        try
        {
            File.WriteAllText(path, contents);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }
}