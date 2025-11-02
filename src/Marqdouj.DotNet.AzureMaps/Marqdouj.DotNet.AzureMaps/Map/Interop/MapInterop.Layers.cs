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

        public async Task CreateLayer(MapLayerDef def, DataSourceDef? dsDef = null)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, def, dsDef);
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
