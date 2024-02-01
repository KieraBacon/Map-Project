using System;
using UnityEngine;
using Newtonsoft.Json;

public class Serializer
{
    public bool TrySerialize<T>(T obj, out string result)
    {
        result = null;
        try
        {
            result = JsonConvert.SerializeObject(obj);
            return !string.IsNullOrWhiteSpace(result);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }
}