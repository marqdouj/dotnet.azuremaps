using Marqdouj.DotNet.AzureMaps.Map.Controls;
using Marqdouj.DotNet.AzureMaps.Map.Events;
using Marqdouj.DotNet.AzureMaps.Map.Interop;
using Marqdouj.DotNet.AzureMaps.Map.Options;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map
{
    internal class MapFactory(IJSRuntime jsRuntime, string mapId)
    {
        private readonly IJSRuntime jsRuntime = jsRuntime;

        public string MapId { get; } = mapId;

        internal async ValueTask AddMap(
            DotNetObjectReference<AzureMap> dotNetRef,
            MapConfiguration config,
            MapOptions? mapOptions,
            IEnumerable<MapEventType>? events,
            IEnumerable<MapControl>? controls = null)
        {
            var jsEvents = events?.EnumToJson();
            var options = config.Merge(mapOptions);

            await jsRuntime.InvokeVoidAsync(
                GetMapFactoryMethod(),
                dotNetRef,
                MapId,
                config.AuthOptions,
                options,
                jsEvents,
                controls?.ToJson());
        }

        internal async ValueTask RemoveMap()
        {
            try
            {
                await jsRuntime.InvokeVoidAsync(GetMapFactoryMethod(), MapId);
            }
            catch (JSDisconnectedException)
            {
            }
        }

        internal static string GetMapFactoryMethod([CallerMemberName] string name = "")
        {
            return $"{MapExtensions.LIBRARY_NAME}.MapFactory.{name.ToJsonName()}";
        }
    }
}
