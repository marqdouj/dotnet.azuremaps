namespace Sandbox.Components.Pages.Maps
{
    internal enum HRefCodeSource
    {
        AzureMaps,
        Sandbox,
        AzureDocs,
    }

    internal static class HRefGenerator
    {
        public static string CodeUrl(this HRefCodeSource source, string path)
        {
            string? url;

            switch (source)
            {
                case HRefCodeSource.AzureDocs:
                    url = "https://learn.microsoft.com/en-us/azure/azure-maps";
                    break;
                case HRefCodeSource.AzureMaps:
                    url = "https://github.com/marqdouj/dotnet.azuremaps/blob/master/src/Marqdouj.DotNet.AzureMaps/Marqdouj.DotNet.AzureMaps";
                    break;
                case HRefCodeSource.Sandbox:
                    url = "https://github.com/marqdouj/dotnet.azuremaps/blob/master/demo/Sandbox/Sandbox/Components/Pages/Maps";
                    break;
                default:
                    throw new NotImplementedException();
            }

            return Path.Combine(url, path);
        }
    }
}
