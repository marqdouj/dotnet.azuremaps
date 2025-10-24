namespace DemoApp.Components.Pages.Maps
{
    internal enum HRefCodeSource
    {
        AzureMaps,
        DemoApp,
        AzureDocs,
        Examples,
    }

    internal static class HRefGenerator
    {
        public static string CodeUrl(this HRefCodeSource source, string path)
        {
            string? url = source switch
            {
                HRefCodeSource.AzureDocs => "https://learn.microsoft.com/en-us/azure/azure-maps",
                HRefCodeSource.AzureMaps => "https://github.com/marqdouj/dotnet.azuremaps/blob/master/src/Marqdouj.DotNet.AzureMaps/Marqdouj.DotNet.AzureMaps",
                HRefCodeSource.DemoApp => "https://github.com/marqdouj/dotnet.azuremaps/blob/master/demo/DemoApp/DemoApp/Components/Pages/Maps",
                HRefCodeSource.Examples => "https://raw.githubusercontent.com/marqdouj/dotnet.azuremaps/master/docs/examples/",
                _ => throw new NotImplementedException(),
            };
            return Path.Combine(url, path);
        }
    }
}
