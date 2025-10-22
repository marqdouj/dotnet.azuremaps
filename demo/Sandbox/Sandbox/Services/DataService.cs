using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using Marqdouj.DotNet.AzureMaps.Map.Interop.Layers;

namespace Sandbox.Services
{
    public interface IDataService
    {
        Task<List<Position>> GetBubbleLayerData();
        Task<string> GetHeatMapLayerUrl();
        Task<ImageLayerData> GetImageLayerData();
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
