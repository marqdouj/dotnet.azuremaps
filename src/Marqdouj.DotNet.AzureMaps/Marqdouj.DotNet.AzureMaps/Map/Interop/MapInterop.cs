using Marqdouj.DotNet.AzureMaps.Map.Interop.Layers;
using Microsoft.JSInterop;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop
{
    public class MapInterop(IJSRuntime jsRuntime, string mapId)
    {
        public string MapId { get; } = mapId;

        public MapInteropLayers Layers { get; } = new MapInteropLayers(jsRuntime, mapId);
        public MapInteropMap Map { get; } = new MapInteropMap(jsRuntime, mapId);
        public MapInteropConfiguration Configuration { get; } = new MapInteropConfiguration(jsRuntime, mapId);

        /// <summary>
        /// Removes a layer and its associated datasource from the map.
        /// </summary>
        /// <param name="layerDef"></param>
        /// <returns></returns>
        public async ValueTask RemoveLayer(MapLayerDef layerDef)
        {
            if (!string.IsNullOrWhiteSpace(layerDef.Id))
                await Layers.RemoveLayer(layerDef.Id);

            if (!string.IsNullOrWhiteSpace(layerDef.SourceId))
                await Map.RemoveDatasource(layerDef.SourceId);
        }
    }
}
