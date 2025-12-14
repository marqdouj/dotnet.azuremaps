using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.Controls;
using Marqdouj.DotNet.AzureMaps.Map.Events;
using Marqdouj.DotNet.AzureMaps.Map.Layers;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop
{
    public interface IAzureMapsMaps
    {
        /// <summary>
        /// Adds the specified collection of map controls to the map.
        /// </summary>
        /// <param name="controls">An enumerable collection of <see cref="MapControl"/> objects to be added to the map. The controls are
        /// ordered by their <see cref="MapControl.SortOrder"/> property before being added. Cannot be null.</param>
        Task AddControls(IEnumerable<MapControl> controls);

        /// <summary>
        /// Adds the specifed events to the <see cref="MapEventDef.Target"/>
        /// </summary>
        /// <param name="mapEvents"></param>
        /// <returns></returns>
        Task AddEvents(IEnumerable<MapEventDef> mapEvents);
        Task AddEvents(IEnumerable<MapEventType> mapEvents);
        Task AddMarkers(IEnumerable<HtmlMarkerDef> markers);
        Task AddPopups(IEnumerable<PopupDef> Popups);

        /// <summary>
        /// Clears the specified data source.
        /// </summary>
        /// <param name="sourceId">The identifier of the data source to remove. Cannot be null or empty.</param>
        Task ClearDatasource(string sourceId);

        /// <summary>
        /// Creates a new data source on the map using the specified definition.
        /// </summary>
        /// <param name="source">The data source definition to be added to the map. Cannot be null.</param>
        Task CreateDatasource(DataSourceDef source);

        /// <summary>
        /// Retrieves a list of map controls associated with the current map.
        /// </summary>
        Task<List<MapControl>> GetControls();
        Task<List<MapEventShape>> GetDataSourceShapes(string sourceId);

        /// <summary>
        /// Removes the specified controls from the map.
        /// </summary>
        /// <param name="controls">A collection of <see cref="MapControl"/> instances to be removed from the map. Cannot be null.</param>
        Task RemoveControls(IEnumerable<MapControl> controls);

        /// <summary>
        /// Removes the data source identified by the specified source Id.
        /// </summary>
        /// <param name="sourceId">The unique identifier of the data source to remove. Cannot be null or empty.</param>
        Task RemoveDatasource(string sourceId);

        /// <summary>
        /// Removes the specifed events from the <see cref="MapEventDef.Target"/>
        /// </summary>
        /// <param name="mapEvents"></param>
        /// <returns></returns>
        Task RemoveEvents(IEnumerable<MapEventDef> mapEvents);
        Task RemoveMarkers(IEnumerable<HtmlMarkerDef> markers);
        Task RemovePopups(IEnumerable<PopupDef> Popups);
    }

    internal sealed class AzureMapsMaps(AzureMapReference mapReference) : IAzureMapsMaps
    {
        private readonly AzureMapReference mapReference = mapReference;
        private string MapId => mapReference.MapId;
        private IJSRuntime JsRuntime => mapReference.JsRuntime;

        public async Task AddControls(IEnumerable<MapControl> controls)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, controls.OrderBy(e => e.SortOrder).ToJson());
        }

        public async Task AddEvents(IEnumerable<MapEventType> mapEvents)
        {
            var events = mapEvents.Select(e => new MapEventDef(e, MapEventTarget.Map));
            await AddEvents(events);
        }

        public async Task AddEvents(IEnumerable<MapEventDef> mapEvents)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), mapReference.DotNetRef, MapId, mapEvents);
        }

        public async Task RemoveEvents(IEnumerable<MapEventDef> mapEvents)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, mapEvents);
        }

        public async Task CreateDatasource(DataSourceDef source)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, source);
        }

        public async Task ClearDatasource(string sourceId)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, sourceId);
        }

        public async Task<List<MapEventShape>> GetDataSourceShapes(string sourceId)
        {
            return await JsRuntime.InvokeAsync<List<MapEventShape>>(GetMapInteropMethod(), MapId, sourceId);
        }

        public async Task<List<MapControl>> GetControls()
        {
            var controls = await JsRuntime.InvokeAsync<List<MapControl>>(GetMapInteropMethod(), MapId);
            return [.. controls.OrderBy(e => e.Type)];
        }

        public async Task RemoveControls(IEnumerable<MapControl> controls)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, controls);
        }

        public async Task RemoveDatasource(string sourceId)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, sourceId);
        }

        public async Task AddMarkers(IEnumerable<HtmlMarkerDef> markers)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, markers);
        }

        public async Task RemoveMarkers(IEnumerable<HtmlMarkerDef> markers)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, markers);
        }

        public async Task AddPopups(IEnumerable<PopupDef> Popups)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, Popups);
        }

        public async Task RemovePopups(IEnumerable<PopupDef> Popups)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, Popups);
        }

        private static string GetMapInteropMethod([CallerMemberName] string name = "")
            => AzureMapContainer.GetMapInteropMethod(MapInteropModule.Maps, name);
    }
}
