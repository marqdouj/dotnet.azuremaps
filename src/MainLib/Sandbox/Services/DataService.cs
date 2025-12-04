using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using Marqdouj.DotNet.AzureMaps.Map.Layers;

namespace Sandbox.Services
{
    public interface IDataService
    {
        Task<List<Position>> GetBubbleLayerData();
        Task<string> GetHeatMapLayerUrl();
        Task<ImageLayerData> GetImageLayerData();
        Task<List<Position>> GetLineLayerData();
        Task<List<List<Position>>> GetPolygonExtLayerData();
        Task<List<List<Position>>> GetPolygonLayerData();
        Task<List<Position>> GetSymbolLayerData();
        Task<string> GetTileLayerUrl();
    }

    /// <summary>
    /// Simulates getting data from an API.
    /// </summary>
    internal class DataService : IDataService
    {
        public async Task<List<Position>> GetBubbleLayerData()
        {
            await Task.CompletedTask;

            return [
                new Position(-73.985708, 40.75773),
                new Position(-73.985600, 40.76542),
                new Position(-73.985550, 40.77900),
                new Position(-73.975550, 40.74859),
                new Position(-73.968900, 40.78859)
            ];
        }

        public async Task<string> GetHeatMapLayerUrl()
        {
            await Task.CompletedTask;
            var url = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_week.geojson";
            return url;
        }

        public async Task<ImageLayerData> GetImageLayerData()
        {
            await Task.CompletedTask;
            return new ImageLayerData();
        }

        public async Task<List<Position>> GetLineLayerData()
        {
            await Task.CompletedTask;

            List<Position> coordinates = [
                 new Position(-122.18822, 47.63208),
                 new Position(-122.18204, 47.63196),
                 new Position(-122.17243, 47.62976),
                 new Position(-122.16419, 47.63023),
                 new Position(-122.15852, 47.62942),
                 new Position(-122.15183, 47.62988),
                 new Position(-122.14256, 47.63451),
                 new Position(-122.13483, 47.64041),
                 new Position(-122.13466, 47.64422),
                 new Position(-122.13844, 47.65440),
                 new Position(-122.13277, 47.66515),
                 new Position(-122.12779, 47.66712),
                 new Position(-122.11595, 47.66712),
                 new Position(-122.11063, 47.66735),
                 new Position(-122.10668, 47.67035),
                 new Position(-122.10565, 47.67498)
            ];

            return coordinates;
        }

        public async Task<List<List<Position>>> GetPolygonExtLayerData()
        {
            await Task.CompletedTask;

            List<List<Position>> coordinates =
            [[
                new Position(-73.95838379859924, 40.80027995478159),
                new Position(-73.98154735565186, 40.76845986171129),
                new Position(-73.98124694824219, 40.767761062136955),
                new Position(-73.97361874580382, 40.76461637311633),
                new Position(-73.97306084632874, 40.76512830937617),
                new Position(-73.97259950637817, 40.76490890860481),
                new Position(-73.9494466781616,  40.79658450499243),
                new Position(-73.94966125488281, 40.79708807289436),
                new Position(-73.95781517028809, 40.80052360358227),
                new Position(-73.95838379859924, 40.80027995478159)
             ]];

            return coordinates;
        }

        public async Task<List<List<Position>>> GetPolygonLayerData()
        {
            await Task.CompletedTask;

            List<List<Position>> coordinates =
            [[
                new Position(-73.98235, 40.76799),
                new Position(-73.95785, 40.80044),
                new Position(-73.94928, 40.7968),
                new Position(-73.97317, 40.76437),
                new Position(-73.98235, 40.76799)
             ]];

            return coordinates;
        }

        public async Task<List<Position>> GetSymbolLayerData()
        {
            await Task.CompletedTask;

            List<Position> coordinates = [
                 new Position(-122.18822, 47.63208),
                 new Position(-122.18204, 47.63196),
                 new Position(-122.17243, 47.62976),
                 new Position(-122.16419, 47.63023),
                 new Position(-122.15852, 47.62942),
                 new Position(-122.15183, 47.62988),
                 new Position(-122.14256, 47.63451),
                 new Position(-122.13483, 47.64041),
                 new Position(-122.13466, 47.64422),
                 new Position(-122.13844, 47.65440),
                 new Position(-122.13277, 47.66515),
                 new Position(-122.12779, 47.66712),
                 new Position(-122.11595, 47.66712),
                 new Position(-122.11063, 47.66735),
                 new Position(-122.10668, 47.67035),
                 new Position(-122.10565, 47.67498)
            ];

            return coordinates;
        }

        public async Task<string> GetTileLayerUrl()
        {
            await Task.CompletedTask;
            var url = "https://tiles.openseamap.org/seamark/{z}/{x}/{y}.png";
            return url;
        }
    }

    public class ImageLayerData()
    {
        public string Url { get; } = "newark_nj_1922.jpg";

        public ImageCoordinates Coordinates { get; set; } =
            [
                new Position(-74.22655, 40.773941),
                new Position(-74.12544, 40.773941),
                new Position(-74.12544, 40.712216),
                new Position(-74.22655, 40.712216),
            ];
    }
}
