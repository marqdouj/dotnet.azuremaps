using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.Configuration;
using Marqdouj.DotNet.AzureMaps.Map.Controls;
using Marqdouj.DotNet.AzureMaps.Map.Events;
using Marqdouj.DotNet.AzureMaps.Map.Geolocation;
using Marqdouj.DotNet.AzureMaps.Map.Interop;
using Marqdouj.DotNet.AzureMaps.Map.Layers;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop
{
    internal class MapContainerMaps(AzureMapJsInterop mapJsInterop) : IAzureMapsMaps
    {
        private readonly AzureMapJsInterop mapJsInterop = mapJsInterop;

        public async Task AddControls(IEnumerable<MapControl> controls)
        {
            await mapJsInterop.Maps.Controls.Add(controls.OrderBy(c => c.SortOrder));
        }

        public async Task AddEvents(IEnumerable<MapEventDef> mapEvents)
        {
            await mapJsInterop.Maps.Events.Add(mapEvents);
        }

        public async Task AddEvents(IEnumerable<MapEventType> mapEvents)
        {
            await mapJsInterop.Maps.Events.Add(mapEvents.Select(e => new MapEventDef(e, MapEventTarget.Map)));
        }

        public async Task AddMarkers(IEnumerable<HtmlMarkerDef> markers)
        {
            await mapJsInterop.Maps.Markers.Add(markers);
        }

        public async Task AddPopups(IEnumerable<PopupDef> Popups)
        {
            await mapJsInterop.Maps.Popups.Add(Popups);
        }

        public async Task ClearDatasource(string sourceId)
        {
            await mapJsInterop.Maps.Sources.ClearById([sourceId]);
        }

        public async Task CreateDatasource(DataSourceDef source)
        {
            await mapJsInterop.Maps.Sources.Add([source]);
        }

        public async Task<List<MapControl>> GetControls()
        {
            return await mapJsInterop.Maps.Controls.GetControls();
        }

        public async Task<List<MapEventShape>> GetDataSourceShapes(string sourceId)
        {
            return await mapJsInterop.Maps.Sources.GetShapes(sourceId);
        }

        public async Task<GeolocationResult> GetGeolocation(PositionOptions? options = null)
        {
            return await mapJsInterop.Maps.Geolocation.GetLocation(options);
        }

        public async Task WatchGeolocation(PositionOptions? options = null)
        {
            await mapJsInterop.Maps.Geolocation.WatchPosition(options);
        }

        public async Task ClearWatchGeolocation()
        {
            await mapJsInterop.Maps.Geolocation.ClearWatch();
        }

        public async Task<TrafficOptions> GetTraffic()
        {
            return await mapJsInterop.Maps.GetTraffic();
        }

        public async Task RemoveControls(IEnumerable<MapControl> controls)
        {
            await mapJsInterop.Maps.Controls.Remove(controls);
        }

        public async Task RemoveDatasource(string sourceId)
        {
            await mapJsInterop.Maps.Sources.RemoveById([sourceId]);
        }

        public async Task RemoveEvents(IEnumerable<MapEventDef> mapEvents)
        {
            await mapJsInterop.Maps.Events.Remove(mapEvents);
        }

        public async Task RemoveMarkers(IEnumerable<HtmlMarkerDef> markers)
        {
            await mapJsInterop.Maps.Markers.Remove(markers);
        }

        public async Task RemovePopups(IEnumerable<PopupDef> Popups)
        {
            await mapJsInterop.Maps.Popups.Remove(Popups);
        }

        public async Task SetTraffic(TrafficOptions? options)
        {
            await mapJsInterop.Maps.SetTraffic(options);
        }
    }
}
