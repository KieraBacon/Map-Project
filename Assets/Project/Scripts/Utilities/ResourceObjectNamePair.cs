public struct ResourceObjectNamePair
{
    public readonly string ResourceName;
    public readonly string ObjectName;

    public ResourceObjectNamePair(string resourceName, string objectName)
    {
        ResourceName = resourceName;
        ObjectName = objectName;
    }

    public ResourceObjectNamePair(string name) : this(name, name)
    {
    }
}