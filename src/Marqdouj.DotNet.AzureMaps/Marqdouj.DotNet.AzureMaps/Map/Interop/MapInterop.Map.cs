using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.Controls;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop
{
    public class MapInteropMap
    {
        private readonly IJSRuntime jsRuntime;

        internal MapInteropMap(IJSRuntime jsRuntime, string mapId)
        {
            this.jsRuntime = jsRuntime;
            MapId = mapId;
        }

        public string MapId { get; }

        public async ValueTask CreateDatasource(DataSourceDef source)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, source);
        }

        public async ValueTask RemoveDatasource(string sourceId)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, sourceId);
        }

        public async Task ClearDatasource(string sourceId)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, sourceId);
        }

        public async ValueTask AddControls(IEnumerable<MapControl> controls)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, controls.OrderBy(e => e.SortOrder).ToJson());
        }

        /// <summary>
        /// Gets a list of controls added to the map using 'AddControls'
        /// </summary>
        /// <returns></returns>
        public async ValueTask<List<MapControl>> GetControls()
        {
            var controls = await jsRuntime.InvokeAsync<List<MapControl>>(GetMapInteropMethod(), MapId);
            return [.. controls.OrderBy(e => e.Type)];
        }

        /// <summary>
        /// Removes the controls added to the map using 'AddControls'
        /// </summary>
        /// <returns></returns>
        public async ValueTask RemoveControls(IEnumerable<MapControl> controls)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, controls);
        }

        private static string GetMapInteropMethod([CallerMemberName] string name = "")
        {
            return $"{MapExtensions.LIBRARY_NAME}.MapInterop.Map.{name.ToJsonName()}";
        }
    }

    internal static class MapInteropMapExtensions
    {
        public static List<object>? ToJson(this IEnumerable<MapControl>? controls) 
            => controls?.Select(e => (object)e).ToList();
    }
}