using Marqdouj.DotNet.AzureMaps.Map.Controls;
using Marqdouj.DotNet.AzureMaps.Map.Events;
using Marqdouj.DotNet.AzureMaps.Map.Settings;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop.Core
{
    internal class FactoryJs(AzureMapDotNetRef mapReference)
    {
        private readonly AzureMapDotNetRef mapReference = mapReference;
        private string MapId => mapReference.MapId;
        private IJSRuntime JsRuntime => mapReference.JsRuntime;

        public async Task CreateMap(
            MapConfiguration settings,
            IEnumerable<MapEventType>? events,
            IEnumerable<MapControl>? controls)
        {
            var mapEvents = events?.Select(e => new MapEventDef(e, MapEventTarget.Map));
            await CreateMap(settings, mapEvents, controls);
        }

        public async Task CreateMap(
            MapConfiguration settings,
            IEnumerable<MapEventDef>? events,
            IEnumerable<MapControl>? controls)
        {

            await JsRuntime.InvokeVoidAsync(
                GetJsInteropMethod(),
                mapReference.DotNetRef,
                MapId,
                settings,
                events,
                controls?.Cast<object>().ToList());
        }

        public async Task RemoveMap()
        {
            try
            {
                await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId);
            }
            catch (JSDisconnectedException)
            {
            }
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => MapExtensions.GetJsModuleMethod(JsModule.MapFactory, name);
    }
}
