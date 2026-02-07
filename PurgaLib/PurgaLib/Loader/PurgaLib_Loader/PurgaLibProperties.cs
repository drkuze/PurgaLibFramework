namespace PurgaLib.Loader.PurgaLib_Loader;

public static class PurgaLibProperties
{
    public static string CurrVersion { get; } = Loader.Instance.Version.ToString();
    public static string ApiVersion { get; } = Loader.ApiVersion.ToString();
}