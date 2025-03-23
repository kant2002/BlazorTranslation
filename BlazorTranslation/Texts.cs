using Karambolo.PO;
using System.Globalization;
using System.Reflection;

namespace BlazorTranslation;

public static class Texts
{
    static Dictionary<string, POCatalog> catalog = new();

    static POCatalog? GetCatalogForCulture(string culture)
    {
        if (catalog.TryGetValue(culture, out var catalog1))
        {
            return catalog1;
        }

        var parser = new POParser();
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"BlazorTranslation.Texts.po");
        var result = parser.Parse(stream);
        if (!result.Success)
        {
            throw new InvalidOperationException("Failed to parse Texts.en.po");
        }

        catalog[culture] = result.Catalog;
        return result.Catalog;
    }

    public static string GetSingularItem(string key)
    {
        {
            var culture = CultureInfo.CurrentCulture.Name;
            var catalog = GetCatalogForCulture(culture);
            if (catalog == null)
            {
                catalog = GetCatalogForCulture("en") ?? throw new InvalidOperationException("There no fallback resource");
            }

            return catalog[new(key)][0];
        }
    }

    public static string Home => GetSingularItem("Home");
    public static string HelloWorld => GetSingularItem("HelloWorld");
    public static string WelcomeText => GetSingularItem("WelcomeText");

}
