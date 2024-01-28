using UnityEngine;

public static class ResourceInstantiator
{
    public static bool TryInstantiateResource<T>(ResourceObjectNamePair resource, out T result) where T : Object =>
        TryInstantiateResource(resource, null, out result);

    public static bool TryInstantiateResource<T>(ResourceObjectNamePair resource, Component parent, out T result) where T : Object =>
        TryInstantiateResource(resource, parent.transform, out result);

    public static bool TryInstantiateResource<T>(ResourceObjectNamePair resource, Transform parent, out T result) where T : Object
    {
        result = null;
        T loaded = Resources.Load<T>(resource.ResourceName);
        if (loaded == null) return false;

        result = Object.Instantiate(loaded, parent);
        if (result == null) return false;

        result.name = resource.ObjectName;
        return true;
    }
}