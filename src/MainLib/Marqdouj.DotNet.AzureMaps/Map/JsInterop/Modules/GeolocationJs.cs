using Marqdouj.DotNet.AzureMaps.Map.Geolocation;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop.Modules
{
    internal class GeolocationJs(AzureMapDotNetRef mapReference)
    {
        private readonly AzureMapDotNetRef mapReference = mapReference;
        private string MapId => mapReference.MapId;
        private IJSRuntime JsRuntime => mapReference.JsRuntime;

        public async Task<GeolocationResult> GetLocation(PositionOptions? options)
        {
            return await JsRuntime.InvokeAsync<GeolocationResult>(GetJsInteropMethod(), options);
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => MapExtensions.GetJsModuleMethod(JsModule.Geolocation, name);
    }
}
