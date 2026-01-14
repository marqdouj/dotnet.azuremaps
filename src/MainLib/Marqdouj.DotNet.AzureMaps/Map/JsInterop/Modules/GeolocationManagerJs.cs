using Marqdouj.DotNet.AzureMaps.Map.Geolocation;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop.Modules
{
    internal class GeolocationManagerJs(AzureMapDotNetRef mapReference)
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
            return await JsRuntime.InvokeAsync<int?>(GetJsInteropMethod(), mapReference.DotNetRef, MapId, nameof(AzureMap.NotifyGeolocationWatch), options);
        }

        public async Task ClearWatch()
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId);
        }

        public async Task<bool> IsWatched()
        {
            return await JsRuntime.InvokeAsync<bool>(GetJsInteropMethod(), MapId);
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => MapExtensions.GetJsModuleMethod(JsModule.Geolocation, name);
    }
}
