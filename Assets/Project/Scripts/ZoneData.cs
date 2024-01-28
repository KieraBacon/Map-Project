using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Zone Data", menuName = "Map Project/Zone Data")]
public class ZoneData : ScriptableObject, IScreenData
{
    [SerializeField, TextArea(1, 100)] private string _description;
    [SerializeField] private List<ZoneData> _connectedZones;
    private string ConnectedZonesString =>
        $"{"Connected Zones".InBold()}: {string.Join(", ", _connectedZones.Select(x => x.HeaderText.WithLink(x.HeaderText)))}";
    public string HeaderText =>
        name;
    public string BodyText =>
        $"{_description}\n{ConnectedZonesString}";
    public IEnumerable<IScreenData> Links =>
        _connectedZones;
}