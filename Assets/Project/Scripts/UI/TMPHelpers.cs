public static class TMPHelpers
{
    public static string InItalic(this string str) =>
        str.WithTag("i");

    public static string InBold(this string str) =>
        str.WithTag("b");

    public static string WithTag(this string str, string tag) =>
        string.IsNullOrWhiteSpace(tag) ? str : $"<{tag}>{str}</{ClosingTag(tag)}>";

    private static string ClosingTag(string tag)
    {
        string[] split = tag.Split('=');
        return split.Length == 2 ? split[0] : tag;
    }
    
    public static string WithStyle(this string str, string style) =>
        string.IsNullOrWhiteSpace(style) ? str : $"<style={style}>{str}</style>";

    public static string WithLink(this string str, string link = null) =>
        string.IsNullOrWhiteSpace(link) ? $"<link>{str}</link>" : $"<link={link}>{str}</link>";

    public static string WithTagAndLink(this string str, string tag, string link = null) =>
        str.WithLink(link).WithTag(tag);
}