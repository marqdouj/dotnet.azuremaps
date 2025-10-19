using Marqdouj.DotNet.AzureMaps.Map.Options;

namespace Marqdouj.DotNet.AzureMaps.Map
{
    public class MapConfiguration
    {
        public AuthOptions AuthOptions { get; set; } = new();
        public MapOptions? Options { get; set; }

        internal bool IsValid => AuthOptions?.IsValid() ?? false;

        internal string ValidationMessage => GetValidateMessage();

        private string GetValidateMessage()
        {
            return AuthOptions == null
                ? "Map Authentication configuration is missing."
                : AuthOptions.InValidMessage();
        }
    }
}
