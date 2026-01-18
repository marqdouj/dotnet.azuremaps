using Marqdouj.DotNet.AzureMaps.Map.Events;
using Marqdouj.DotNet.AzureMaps.Map.Layers;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop.Modules
{
    internal class LayerManagerJs(AzureMapDotNetRef mapReference)
    {
        private readonly AzureMapDotNetRef mapReference = mapReference;
        private string MapId => mapReference.MapId;
        private IJSRuntime JsRuntime => mapReference.JsRuntime;

        public async Task Add(IEnumerable<MapLayerDef> items, IEnumerable<MapEventDef>? events = null)
        {
            items.EnsureHasId();
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, items?.Cast<object>().ToList(), events?.Cast<object>().ToList());
        }

        public async Task Remove(IEnumerable<MapLayerDef> items)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, items?.Cast<object>().ToList());
        }

        public async Task RemoveById(IEnumerable<string> layers)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, layers);
        }

        public async Task<LayerOptions?> GetOptions(MapLayerDef layerDef)
        {
            return layerDef.LayerType switch
            {
                MapLayerType.Bubble => await JsRuntime.InvokeAsync<BubbleLayerOptions>(GetJsInteropMethod(), MapId, layerDef.Id),
                MapLayerType.HeatMap => await JsRuntime.InvokeAsync<HeatMapLayerOptions>(GetJsInteropMethod(), MapId, layerDef.Id),
                MapLayerType.Image => await JsRuntime.InvokeAsync<ImageLayerOptions>(GetJsInteropMethod(), MapId, layerDef.Id),
                MapLayerType.Line => await JsRuntime.InvokeAsync<LineLayerOptions>(GetJsInteropMethod(), MapId, layerDef.Id),
                MapLayerType.Polygon => await JsRuntime.InvokeAsync<PolygonLayerOptions>(GetJsInteropMethod(), MapId, layerDef.Id),
                MapLayerType.PolygonExtrusion => await JsRuntime.InvokeAsync<PolygonExtLayerOptions>(GetJsInteropMethod(), MapId, layerDef.Id),
                MapLayerType.Symbol => await JsRuntime.InvokeAsync<SymbolLayerOptions>(GetJsInteropMethod(), MapId, layerDef.Id),
                MapLayerType.Tile => await JsRuntime.InvokeAsync<TileLayerOptions>(GetJsInteropMethod(), MapId, layerDef.Id),
                _ => null,
            };
        }

        public async Task SetOptions(MapLayerDef layerDef)
        {
            var json = layerDef.SerializeToJson();
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, json);
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => MapExtensions.GetJsModuleMethod(JsModule.Layers, name);

    }
}
