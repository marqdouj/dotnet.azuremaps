using Microsoft.JSInterop;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop
{
    public class MapInterop(IJSRuntime jsRuntime, string mapId)
    {
        public string MapId { get; } = mapId;

        public MapInteropLayers Layers { get; } = new MapInteropLayers(jsRuntime, mapId);
        public MapInteropMap Map { get; } = new MapInteropMap(jsRuntime, mapId);
        public MapInteropConfiguration Configuration { get; } = new MapInteropConfiguration(jsRuntime, mapId);
    }
}
