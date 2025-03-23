using Karambolo.PO;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace BlazorTranslation;

public static class Texts
{
    static Dictionary<string, POCatalog> catalog = new();

    static POCatalog? GetCatalogForCulture(CultureInfo? culture)
    {
        var cultureName = culture?.Name ?? "--";
        if (catalog.TryGetValue(cultureName, out var catalog1))
        {
            return catalog1;
        }

        var parser = new POParser();
        var assembly = Assembly.GetExecutingAssembly();
        assembly = culture is null ? assembly : assembly.GetSatelliteAssembly(culture);
        using var stream = assembly.GetManifestResourceStream($"BlazorTranslation.Texts.po");
        var result = parser.Parse(stream);
        if (!result.Success)
        {
            throw new InvalidOperationException("Failed to parse Texts.en.po");
        }

        catalog[cultureName] = result.Catalog;
        return result.Catalog;
    }

    private static POCatalog GetCurrentCatalog()
    {
        var culture = CultureInfo.CurrentCulture;
        var catalog = GetCatalogForCulture(culture);
        if (catalog == null)
        {
            catalog = GetCatalogForCulture(null) ?? throw new InvalidOperationException("There no fallback resource");
        }
        return catalog;
    }

    public static string GetSingularItem(string key)
    {
        var catalog = GetCurrentCatalog();
        return catalog.GetTranslation(new(key));
    }

    public static string GetPluralItem(string key, int value)
    {
        var catalog = GetCurrentCatalog();
        var si = catalog.GetTranslation(new(key));
        var format = catalog.GetTranslation(new(key, key), value);
        var ii = catalog.GetPluralFormIndex(value);
        return format == null ? null : string.Format(format, value);
    }

    public static string Home => GetSingularItem("Home");
    public static string HelloWorld => GetSingularItem("HelloWorld");
    public static string WelcomeText => GetSingularItem("WelcomeText");
    public static string CounterTitle => GetSingularItem("CounterTitle");
    public static string ClickMe => GetSingularItem("ClickMe");
    public static string CurrentCount(int value) => GetPluralItem("CurrentCount", value);
    public static string WeatherTitle => GetSingularItem("WeatherTitle");

}
