using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LinksManager
{
    private Dictionary<string, string> _categoryTypes = new Dictionary<string, string>()
    {
        {"Zone", typeof(ZoneData).AssemblyQualifiedName},
        {"Creature", typeof(ZoneData).AssemblyQualifiedName},
    };
    
    private static LinksManager _instance;
    public static LinksManager Main =>
        _instance ??= new LinksManager();
    private Dictionary<string, ILinkable> _links = new();
    private List<string> _checkedLinks = new();

    bool TryGetLinkFromFile(string path, out ILinkable result)
    {
        // Fail early if previously checked.
        result = null;
        if (_checkedLinks.Contains(path)) return false;
        _checkedLinks.Add(path);

        // Find, read, and deserialize the file.
        FileManager fileManager = FileManager.Instance;
        string filePath = fileManager.CombinePath(path);
        if (!fileManager.TryDeserializeFromFile(filePath, out JObject jObj)) return false;

        // Convert to appropriate type.
        if (!jObj.TryGetValue("Type", out JToken category) || string.IsNullOrWhiteSpace(category.ToString())) return false;
        if (!_categoryTypes.TryGetValue(category.ToString(), out string typeName)) return false;
        Type type = Type.GetType(typeName);
        if (type == null) return false;

        // Determine if the resulting type is ILinkable.
        string str = jObj.ToString();
        object vvv = JsonConvert.DeserializeObject(str, type);
        result = vvv as ILinkable;
        if (result == null) return false;

        // Register it!
        RegisterLink(result);
        return true;
    }

    public bool TryGetLinkAtPath(string path, out ILinkable result) =>
        _links.TryGetValue(path, out result) || TryGetLinkFromFile(path, out result);

    public IEnumerable<ILinkable> GetLinksAtPaths(IEnumerable<string> paths) =>
        paths.Select(path => TryGetLinkAtPath(path, out ILinkable result) ? result : null);

    public void RegisterLink([NotNull] ILinkable value) =>
        _links[value.Path] = value;
}