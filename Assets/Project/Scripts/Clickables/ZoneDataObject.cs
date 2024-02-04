using UnityEngine;

[CreateAssetMenu(fileName = "New Zone Data", menuName = "Map Project/Zone Data")]
public class ZoneDataObject : ScriptableObject, ISerializableObjectContainer
{
    [ContextMenuItem("Serialize", nameof(Serialize))]
    public ZoneData _zoneData;

    [ContextMenu("Serialize")] public void Serialize()
    {
        _zoneData._name = name;
        FileManager.Save(name, _zoneData);
    }

    public object InnerObject
    {
        get => _zoneData;
        set => _zoneData = (ZoneData)value;
    }
}