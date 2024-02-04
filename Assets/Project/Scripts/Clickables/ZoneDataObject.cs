using UnityEngine;

[CreateAssetMenu(fileName = "New Zone Data", menuName = "Map Project/Zone Data")]
public class ZoneDataObject : ScriptableObject
{
    [ContextMenuItem("Serialize", nameof(Serialize))]
    public ZoneData _zoneData;

    [ContextMenu("Serialize")] public void Serialize()
    {
        FileManager.Save($"{name}{ZoneData.k_FileExtension}", _zoneData);
    }
}