using Marqdouj.DotNet.AzureMaps.Map.Events;
using Marqdouj.DotNet.AzureMaps.Map.Interop;
using Marqdouj.DotNet.AzureMaps.Map.Layers;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop
{
    internal class MapContainerLayers(AzureMapJsInterop mapJsInterop) : IAzureMapsLayers
    {
        private readonly AzureMapJsInterop mapJsInterop = mapJsInterop;

        public async Task AddMapFeature(MapFeatureDef feature, string datasourceId, bool replace = false)
        {
            await mapJsInterop.Maps.Features.Add(feature, datasourceId, replace);
        }

        public async Task AddMapFeatures(List<MapFeatureDef> features, string datasourceId, bool replace = false)
        {
            await mapJsInterop.Maps.Features.Add(features, datasourceId, replace);
        }

        public async Task CreateLayer(MapLayerDef layerDef, IEnumerable<MapEventDef>? events = null)
        {
            await CreateLayers([layerDef], events);
        }

        public async Task CreateLayers(IEnumerable<MapLayerDef> layerDefs, IEnumerable<MapEventDef>? events = null)
        {
            await mapJsInterop.Maps.Layers.Add(layerDefs, events);
        }

        public async Task RemoveLayer(string layerId)
        {
            await mapJsInterop.Maps.Layers.RemoveById([layerId]);
        }

        public async Task RemoveLayer(MapLayerDef layerDef)
        {
            await RemoveLayers([layerDef]);
        }

        public async Task RemoveLayers(IEnumerable<MapLayerDef> layerDefS)
        {
            await mapJsInterop.Maps.Layers.Remove(layerDefS);
        }

        public async Task<LayerOptions?> GetOptions(MapLayerDef layerDef)
        {
            return await mapJsInterop.Maps.Layers.GetOptions(layerDef);
        }

        public async Task SetOptions(MapLayerDef layerDef)
        {
            await mapJsInterop.Maps.Layers.SetOptions(layerDef);
        }

        public async Task UpdateMapFeature(MapFeatureDef feature, string datasourceId)
        {
            await mapJsInterop.Maps.Features.Update(feature, datasourceId);
        }

        public async Task UpdateMapFeatures(List<MapFeatureDef> features, string datasourceId)
        {
            await mapJsInterop.Maps.Features.Update(features, datasourceId);
        }
    }
}
