using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.Interop.Features;
using Marqdouj.DotNet.AzureMaps.Map.Interop.Layers;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop
{
    public class MapInteropLayers
    {
        private readonly IJSRuntime jsRuntime;

        internal MapInteropLayers(IJSRuntime jsRuntime, string mapId)
        {
            this.jsRuntime = jsRuntime;
            MapId = mapId;
        }

        public string MapId { get; }

        /// <summary>
        /// Creates a new map layer on the client using the specified layer definition and optional data source.
        /// </summary>
        /// <param name="layerDef">The definition of the map layer to create. Specifies layer properties such as type, style, and
        /// configuration.</param>
        /// <param name="dataSourceDef">An optional data source definition to associate with the layer. If not provided, the data source from
        /// <paramref name="layerDef"/> is used.</param>
        /// <returns>A task that represents the asynchronous operation of creating the map layer.</returns>
        public async Task CreateLayer(MapLayerDef layerDef, DataSourceDef? dataSourceDef = null)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, layerDef, dataSourceDef ?? layerDef.GetDataSource());
        }

        public async Task RemoveLayer(string layerId)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, layerId);
        }

        public async Task AddMapFeature(MapFeatureDef feature, string datasourceId, bool replace = false)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, feature, datasourceId, replace);
        }

        private static string GetMapInteropMethod([CallerMemberName] string name = "")
        {
            return $"{MapExtensions.LIBRARY_NAME}.MapInterop.Layers.{name.ToJsonName()}";
        }
    }
}
