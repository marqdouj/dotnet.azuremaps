using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop.Modules
{
    public interface IAzureMapsData
    {
        IAzureMapsMercator Mercator { get; }
    }

    internal class DataJs(AzureMapDotNetRef mapReference) : IAzureMapsData
    {
        public IAzureMapsMercator Mercator { get; } = new MercatorJs(mapReference);
    }

    public interface IAzureMapsMercator
    {
        Task<MercatorPoint> FromPosition(Position position);
        Task<List<MercatorPoint>> FromPositions(IEnumerable<Position> positions);
        Task<double> MercatorScale(double latitude);
        Task<double> MeterInMercatorUnits(double latitude);
        Task<Dictionary<int, double>> ToFloat32Array(IEnumerable<Position> positions);
        Task<Position> ToPosition(MercatorPoint mercator);
        Task<List<Position>> ToPositions(IEnumerable<MercatorPoint> mercators);
    }

    internal class MercatorJs(AzureMapDotNetRef mapReference) : IAzureMapsMercator
    {
        private readonly AzureMapDotNetRef mapReference = mapReference;
        private string MapId => mapReference.MapId;
        private IJSRuntime JsRuntime => mapReference.JsRuntime;

        public async Task<MercatorPoint> FromPosition(Position position)
        {
            return await JsRuntime.InvokeAsync<MercatorPoint>(GetJsInteropMethod(), position);
        }

        public async Task<List<MercatorPoint>> FromPositions(IEnumerable<Position> positions)
        {
            return await JsRuntime.InvokeAsync<List<MercatorPoint>>(GetJsInteropMethod(), positions);
        }

        public async Task<Position> ToPosition(MercatorPoint mercator)
        {
            return await JsRuntime.InvokeAsync<Position>(GetJsInteropMethod(), mercator);
        }

        public async Task<List<Position>> ToPositions(IEnumerable<MercatorPoint> mercators)
        {
            return await JsRuntime.InvokeAsync<List<Position>>(GetJsInteropMethod(), mercators);
        }

        public async Task<Dictionary<int, double>> ToFloat32Array(IEnumerable<Position> positions)
        {
            return await JsRuntime.InvokeAsync<Dictionary<int, double>>(GetJsInteropMethod(), positions);
        }

        public async Task<double> MercatorScale(double latitude)
        {
            return await JsRuntime.InvokeAsync<double>(GetJsInteropMethod(), latitude);
        }

        public async Task<double> MeterInMercatorUnits(double latitude)
        {
            return await JsRuntime.InvokeAsync<double>(GetJsInteropMethod(), latitude);
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => MapExtensions.GetJsModuleMethod(JsModule.Mercator, name);
    }
}
