using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class ZoneData : IScreenData
{
    public const string k_FileExtension = ".json";

    [HideInInspector] public string Name;
    [SerializeField, TextArea(1, 100)] public string Description;
    [JsonIgnore] private List<IScreenData> _connectedZones = new ();

    [JsonIgnore] private string ConnectedZonesString =>
        _connectedZones.Any() ? 
            $"{"Connected Zones".InBold()}: {string.Join(", ", _connectedZones.Select(x => x.HeaderText.WithLink(x.HeaderText)))}" : 
            "";
    [JsonIgnore] public string HeaderText =>
        Name;
    [JsonIgnore] public string BodyText =>
        $"{Description}\n{ConnectedZonesString}";
    [JsonIgnore] public IEnumerable<IScreenData> Links =>
        _connectedZones;
}
