using Marqdouj.DotNet.AzureMaps.Map.GeoJson;

namespace Sandbox.Services
{
    public interface IDataService
    {
        Task<List<Position>> GetBubbleLayerData();
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

    }
}
