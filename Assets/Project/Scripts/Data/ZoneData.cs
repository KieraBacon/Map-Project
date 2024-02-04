using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

[Serializable] public class ZoneData : IDescribable, ILinkable, ICategorizable
{
    public const string k_FileExtension = ".json";
    [JsonIgnore] public string Category => "Zones";

    [JsonProperty(PropertyName = "Name")] private string _name;
    [JsonIgnore] public string Name =>
        _name;
    public string Path =>
        _name;

    [JsonProperty(PropertyName = "Description")] [SerializeField, TextArea(1, 100)]
    public string _description;

    [JsonIgnore] public string Description =>
        string.Join("\n", new List<string>() { _description, LinksString });

    [JsonProperty(PropertyName = "Links")] [SerializeField] private List<string> _links;
    [JsonIgnore] public IEnumerable<string> Links => _links ?? Enumerable.Empty<string>();
    private string _linksString;
    private LinkParser _linkParser;
    private string LinksString => _linksString ??= !Links.Any() ? "" : (_linkParser ??= new LinkParser()).GetFormattedLinksString(Links, LinksManager.Main);
}
