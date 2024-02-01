using System;
using System.IO;
using UnityEngine;

public class FileManager : IDisposable
{
    public static readonly FileManager Instance = new FileManager();
    public FileWriter FileWriter = new FileWriter();
    public FileReader FileReader = new FileReader();
    public Serializer Serializer = new Serializer();
    public Deserializer Deserializer = new Deserializer();
    public string DefaultFilePath = Path.Combine("Project", "Data");
    
    public string CombinePath(string filename) =>
        Path.Combine(Application.dataPath, DefaultFilePath, filename);

    public bool TryDeserializeFromFile<T>(string path, out T result)
    {
        result = default;
        if (!FileReader.TryReadFromFile(path, out string data)) return false;
        if (!Deserializer.TryDeserialize(data, out result)) return false;
        return true;
    }

    public static T Load<T>(string fileName, bool throwFailure = true)
    {
        string filePath = Instance.CombinePath(fileName);
        if (!Instance.TryDeserializeFromFile(filePath, out T result) && throwFailure) throw new Exception($"Unable to load {typeof(T)} object from file at path {filePath}.");
        return result;
    }

    public bool TrySerializeToFile<T>(string path, T obj)
    {
        if (!Serializer.TrySerialize(obj, out string data)) return false;
        if (!FileWriter.TryWriteToFile(path, data)) return false;
        return true;
    }

    public static void Save<T>(string fileName, T obj, bool throwFailure = true)
    {
        string filePath = Instance.CombinePath(fileName);
        if (!Instance.TrySerializeToFile(filePath, obj) && throwFailure) throw new Exception($"Unable to save object of type {typeof(T)} to file at path {filePath}.");
    }

    public void Dispose()
    {
        FileWriter = null;
        FileReader = null;
        Serializer = null;
        Deserializer = null;
    }
}