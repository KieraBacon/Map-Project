using System.IO;
using System.Text;
using UnityEngine;

public class FileReader
{
    public bool TryReadFromFile(string path, out string result)
    {
        result = null;
        if (!File.Exists(path))
        {
            Debug.LogError($"File at path \"{path}\" does not exist.");
            return false;
        }

        result = File.ReadAllText(path, Encoding.UTF8);
        return true;
    }
}