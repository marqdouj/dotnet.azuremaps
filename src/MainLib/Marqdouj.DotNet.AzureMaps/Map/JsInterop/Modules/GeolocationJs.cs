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

        public async Task<int?> WatchPosition(PositionOptions? options)
        {
            return await JsRuntime.InvokeAsync<int?>(GetJsInteropMethod(), mapReference.DotNetRef, nameof(AzureMap.NotifyGeolocationWatch), options);
        }

        public async Task ClearWatch(int id)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), id);
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => MapExtensions.GetJsModuleMethod(JsModule.Geolocation, name);
    }
}
