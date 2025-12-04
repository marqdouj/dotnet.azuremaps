using Marqdouj.DotNet.AzureMaps.Map.Controls;
using Marqdouj.DotNet.AzureMaps.Map.Events;
using Marqdouj.DotNet.AzureMaps.Map.Settings;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop
{
    internal class MapFactory(IJSRuntime jsRuntime, string mapId)
    {
        private readonly IJSRuntime jsRuntime = jsRuntime;

        public string MapId { get; } = mapId;

        internal async Task CreateMap(
            DotNetObjectReference<AzureMap> dotNetRef,
            MapConfiguration settings, 
            IEnumerable<MapEventType>? events, 
            IEnumerable<MapControl>? controls)
        {
            await jsRuntime.InvokeVoidAsync(
                GetMapFactoryMethod(),
                dotNetRef,
                MapId,
                settings,
                events?.EnumToJson(),
                controls?.ToJson());
        }

        internal async Task RemoveMap()
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