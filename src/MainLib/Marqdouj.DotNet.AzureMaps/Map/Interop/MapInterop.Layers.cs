using Marqdouj.DotNet.AzureMaps.Map.Layers;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop
{
    public class MapInteropLayers
    {
        private readonly IJSRuntime jsRuntime;
        private readonly MapInteropShared shared;

        internal MapInteropLayers(IJSRuntime jsRuntime, string mapId)
        {
            this.jsRuntime = jsRuntime;
            MapId = mapId;
            shared = new MapInteropShared(jsRuntime, mapId);
        }

        public string MapId { get; }

        /// <summary>
        /// Creates a new map layer on the client using the specified layer definition and optional data source.
        /// </summary>
        /// <param name="layerDef">The definition of the map layer to create.</param>
        public async Task CreateLayer(MapLayerDef layerDef)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, layerDef, layerDef.DataSource);
        }

        /// <summary>
        /// Removes a layer and its associated datasource from the map.
        /// </summary>
        /// <param name="layerDef"></param>
        /// <returns></returns>
        public async Task RemoveLayer(MapLayerDef layerDef)
        {
            if (!string.IsNullOrWhiteSpace(layerDef.Id))
                await RemoveLayer(layerDef.Id);

            if (!string.IsNullOrWhiteSpace(layerDef.DataSource?.Id))
                await shared.RemoveDatasource(layerDef.DataSource.Id);
        }

        /// <summary>
        /// Removes the specified layer from the map.
        /// </summary>
        /// <param name="layerId">The unique identifier of the layer to remove. Cannot be null or empty.</param>
        public async Task RemoveLayer(string layerId)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, layerId);
        }

        /// <summary>
        /// Adds a map feature to the specified data source.
        /// </summary>
        /// <param name="feature">The definition of the map feature to add. Cannot be null.</param>
        /// <param name="datasourceId">The identifier of the data source to which the feature will be added. Cannot be null or empty.</param>
        /// <param name="replace">If <see langword="true"/>, replaces any existing feature with the same identifier; otherwise, adds the
        /// feature without replacing.</param>
        public async Task AddMapFeature(MapFeatureDef feature, string datasourceId, bool replace = false)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, feature, datasourceId, replace);
        }

        private static string GetMapInteropMethod([CallerMemberName] string name = "")
#pragma warning disable CS0618 // Type or member is obsolete
            => MapInterop.GetMapInteropMethod(MapInteropModule.Layers, name);
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
