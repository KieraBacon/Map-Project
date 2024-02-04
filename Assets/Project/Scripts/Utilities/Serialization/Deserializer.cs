using System;
using UnityEngine;
using Newtonsoft.Json;

public class Deserializer
{
    public bool TryDeserialize<T>(string data, out T result) where T : new()
    {
        result = default;
        try
        {
            result = JsonConvert.DeserializeObject<T>(data);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }
}