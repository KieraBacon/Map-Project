public static class TMPHelpers
{
    public static string InItalic(this string str) =>
        $"<i>{str}</i>";
    public static string InBold(this string str) =>
        $"<b>{str}</b>";
    public static string WithStyle(this string str, string style) =>
        $"<style={style}>{str}</link>";
    public static string WithLink(this string str, string link, bool includeStyle = true) =>
        $"<link={link}>{str}</link>".WithStyle("link");
}