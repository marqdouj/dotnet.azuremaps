using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop
{
    internal enum MapInteropModule
    {
        Configuration,
        Layers,
        MapFactory,
        Maps
    }

    [Obsolete("This class is no longer being maintained and is kept for backward compatibility. It may be removed in a future version. Use the AzureMap.OnMapReady event and the IAzureMapContainer.")]
    public class MapInterop(IJSRuntime jsRuntime, string mapId)
    {
        public string MapId { get; } = mapId;

        public MapInteropConfiguration Configuration { get; } = new MapInteropConfiguration(jsRuntime, mapId);
        public MapInteropLayers Layers { get; } = new MapInteropLayers(jsRuntime, mapId);
        public MapInteropMaps Map { get; } = new MapInteropMaps(jsRuntime, mapId);

        internal static string GetMapInteropMethod(MapInteropModule module, [CallerMemberName] string name = "")
            => $"{MapExtensions.LIBRARY_NAME}.{module}.{name.ToJsonName()}";
    }
}
