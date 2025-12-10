using Marqdouj.DotNet.AzureMaps.Map.Controls;
using Marqdouj.DotNet.AzureMaps.Map.Layers;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop
{
    public class MapInteropMaps
    {
        private readonly IJSRuntime jsRuntime;
        private readonly MapInteropShared shared;

        internal MapInteropMaps(IJSRuntime jsRuntime, string mapId)
        {
            this.jsRuntime = jsRuntime;
            MapId = mapId;
            shared = new MapInteropShared(jsRuntime, mapId);
        }

        public string MapId { get; }

        /// <summary>
        /// Adds the specified collection of map controls to the map.
        /// </summary>
        /// <param name="controls">An enumerable collection of <see cref="MapControl"/> objects to be added to the map. The controls are
        /// ordered by their <see cref="MapControl.SortOrder"/> property before being added. Cannot be null.</param>
        public async Task AddControls(IEnumerable<MapControl> controls)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, controls.OrderBy(e => e.SortOrder).ToJson());
        }

        /// <summary>
        /// Creates a new data source on the map using the specified definition.
        /// </summary>
        /// <param name="source">The data source definition to be added to the map. Cannot be null.</param>
        public async Task CreateDatasource(DataSourceDef source)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, source);
        }

        /// <summary>
        /// Clears the specified data source.
        /// </summary>
        /// <param name="sourceId">The identifier of the data source to remove. Cannot be null or empty.</param>
        public async Task ClearDatasource(string sourceId)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, sourceId);
        }

        /// <summary>
        /// Asynchronously retrieves a list of map controls associated with the current map, ordered by control type.
        /// </summary>
        /// <remarks>The returned controls are sorted by type to facilitate predictable ordering when
        /// displaying or processing them. This method does not modify the underlying map state.</remarks>
        /// <returns>A list of <see cref="MapControl"/> objects, ordered by their <see cref="MapControl.Type"/> property. The list will be empty
        /// if no controls are present.</returns>
        public async Task<List<MapControl>> GetControls()
        {
            var controls = await jsRuntime.InvokeAsync<List<MapControl>>(GetMapInteropMethod(), MapId);
            return [.. controls.OrderBy(e => e.Type)];
        }

        /// <summary>
        /// Removes the specified controls from the map.
        /// </summary>
        /// <param name="controls">A collection of <see cref="MapControl"/> instances to be removed from the map. Cannot be null.</param>
        public async Task RemoveControls(IEnumerable<MapControl> controls)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, controls);
        }

        /// <summary>
        /// Removes the data source identified by the specified source ID.
        /// </summary>
        /// <param name="sourceId">The unique identifier of the data source to remove. Cannot be null or empty.</param>
        public async Task RemoveDatasource(string sourceId)
        {
            await shared.RemoveDatasource(sourceId);
        }

        private static string GetMapInteropMethod([CallerMemberName] string name = "")
#pragma warning disable CS0618 // Type or member is obsolete
            => MapInterop.GetMapInteropMethod(MapInteropModule.Maps, name);
#pragma warning restore CS0618 // Type or member is obsolete
    }
}