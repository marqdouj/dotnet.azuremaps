using Marqdouj.DotNet.AzureMaps.Map.Layers;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop
{
    public interface IAzureMapsLayers
    {
        /// <summary>
        /// Adds a map feature to the specified data source.
        /// </summary>
        /// <param name="feature">The definition of the map feature to add. Cannot be null.</param>
        /// <param name="datasourceId">The identifier of the data source to which the feature will be added. Cannot be null or empty.</param>
        /// <param name="replace">If <see langword="true"/>, replaces any existing feature with the same identifier; otherwise, adds the
        /// feature without replacing.</param>
        Task AddMapFeature(MapFeatureDef feature, string datasourceId, bool replace = false);
        Task AddMapFeatures(List<MapFeatureDef> features, string datasourceId, bool replace = false);

        /// <summary>
        /// Creates a new map layer on the client using the specified layer definition.
        /// </summary>
        /// <param name="layerDef">The definition of the map layer to create.</param>
        Task CreateLayer(MapLayerDef layerDef);

        /// <summary>
        /// Removes the specified layer from the map.
        /// </summary>
        /// <param name="layerId">The unique identifier of the layer to remove. Cannot be null or empty.</param>
        Task RemoveLayer(string layerId);

        /// <summary>
        /// Removes a layer and its associated datasource from the map.
        /// </summary>
        /// <param name="layerDef"></param>
        /// <returns></returns>
        Task RemoveLayer(MapLayerDef layerDef);
        Task UpdateMapFeature(MapFeatureDef feature, string datasourceId);
        Task UpdateMapFeatures(List<MapFeatureDef> features, string datasourceId);
    }

    internal sealed class AzureMapsLayers(AzureMapReference mapReference) : IAzureMapsLayers
    {
        private readonly AzureMapReference mapReference = mapReference;
        private string MapId => mapReference.MapId;
        private IJSRuntime JsRuntime => mapReference.JsRuntime;

        public async Task CreateLayer(MapLayerDef layerDef)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, layerDef, layerDef.DataSource);
        }

        public async Task RemoveLayer(string layerId)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, layerId);
        }

        public async Task RemoveLayer(MapLayerDef layerDef)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod("removeLayerAndDataSource"), MapId, layerDef, layerDef.DataSource);
        }

        public async Task AddMapFeature(MapFeatureDef feature, string datasourceId, bool replace = false)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, feature, datasourceId, replace);
        }

        public async Task AddMapFeatures(List<MapFeatureDef> features, string datasourceId, bool replace = false)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, features, datasourceId, replace);
        }

        public async Task UpdateMapFeature(MapFeatureDef feature, string datasourceId)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, feature, datasourceId);
        }

        public async Task UpdateMapFeatures(List<MapFeatureDef> features, string datasourceId)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, features, datasourceId);
        }

        private static string GetMapInteropMethod([CallerMemberName] string name = "")
            => AzureMapContainer.GetMapInteropMethod(MapInteropModule.Layers, name);
    }
}
