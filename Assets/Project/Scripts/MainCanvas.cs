using UCanvas = UnityEngine.Canvas;

public static class Canvas
{
    private static readonly ResourceObjectNamePair k_Resource = new("Main Canvas");
    private static UCanvas _instance;
    public static UCanvas Main =>
        _instance != null || ResourceInstantiator.TryInstantiateResource(k_Resource, out _instance) ? _instance : null;
}
