using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.Configuration;
using Marqdouj.DotNet.AzureMaps.Map.Controls;
using Marqdouj.DotNet.AzureMaps.Map.Events;
using Marqdouj.DotNet.AzureMaps.Map.Layers;

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
        Task<TrafficOptions> GetTraffic();

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
        Task SetTraffic(TrafficOptions? options);
    }
}
