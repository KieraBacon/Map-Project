using System.Linq;
using System.Collections.Generic;

public class LinkParser
{
    public const string k_DefaultLinkTag = "style=link";
    public const string k_DefaultFallbackTag = null;
    public const string k_DefaultCategoryTag = "b";
    public const string k_DefaultFallbackCategory = "Additional Links";

    public string GetName(ILinkable link) =>
        link is IDescribable describable ? describable.Name : link.Path;

    public string GetFormattedName(ILinkable link, string tag = k_DefaultLinkTag) =>
        GetName(link).WithTagAndLink(tag, link.Path);

    public string GetCategory(ILinkable link, string fallbackCategory = k_DefaultFallbackCategory) =>
        link is ICategorizable categorizable ? categorizable.Category : fallbackCategory;

    public (string formattedName, string category) GetFormattedNameAndCategory(ILinkable link, string tag = k_DefaultLinkTag, string fallbackCategory = k_DefaultFallbackCategory) =>
        (GetFormattedName(link, tag), GetCategory(link, fallbackCategory));

    public (string formattedName, string category) GetFormattedNameAndCategory(string path, LinksManager linksManager, string linkTag = k_DefaultLinkTag, string fallbackTag = null, string fallbackCategory = k_DefaultFallbackCategory) =>
        !linksManager.TryGetLinkAtPath(path, out ILinkable link) ? (path.WithTag(fallbackTag), fallbackCategory) : GetFormattedNameAndCategory(link, linkTag, fallbackCategory);

    public string GetFormattedLinksString(IEnumerable<string> links, LinksManager linksManager, string categoryTag = k_DefaultCategoryTag, string linkTag = k_DefaultLinkTag, string fallbackTag = k_DefaultFallbackTag, string fallbackCategory = k_DefaultFallbackCategory) =>
        string.Join("\n", links.Select(path => GetFormattedNameAndCategory(path, linksManager, linkTag, fallbackTag, fallbackCategory))
            .ToLookup(item => item.category, item => item.formattedName)
            .OrderBy(grouping => grouping.Key == fallbackCategory)
            .Select(grouping => $"{grouping.Key.WithTag(categoryTag)}: {string.Join(", ", grouping)}"));
}