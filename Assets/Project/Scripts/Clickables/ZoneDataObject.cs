using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Zone Data", menuName = "Map Project/Zone Data")]
public class ZoneDataObject : ScriptableObject
{
    [SerializeField, TextArea(1, 100)] private string _description;
    [SerializeField] private List<ZoneDataObject> _connectedZones;

    [ContextMenuItem("Serialize", nameof(Serialize))]
    public ZoneData _zoneData;

    private void OnEnable()
    {
        _zoneData.Name = name;
    }

    [ContextMenu("Serialize")] public void Serialize()
    {
        _zoneData.Name = name;
        FileManager.Save($"{name}{ZoneData.k_FileExtension}", _zoneData);
    }
}