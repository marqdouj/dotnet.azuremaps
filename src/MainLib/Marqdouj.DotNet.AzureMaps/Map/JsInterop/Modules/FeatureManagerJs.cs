using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using Marqdouj.DotNet.AzureMaps.Map.Layers;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop.Modules
{
    public interface IAzureMapsFeatures
    {
        Task Add(IEnumerable<MapFeatureDef> features, string datasourceId, bool replace = false);
        Task Add(MapFeatureDef feature, string datasourceId, bool replace = false);
        Task AddProperty(MapFeatureDef feature, string name, object? value, string datasourceId);
        Task<object> GetCoordinates(MapFeatureDef feature, string datasourceId);
        Task<Properties> GetProperties(MapFeatureDef feature, string datasourceId);
        Task SetCoordinates(MapFeatureDef feature, string datasourceId);
        Task SetProperties(MapFeatureDef feature, string datasourceId, bool replace = false);
        Task Update(IEnumerable<MapFeatureDef> features, string datasourceId);
        Task Update(MapFeatureDef feature, string datasourceId);
    }

    internal class FeatureManagerJs(AzureMapDotNetRef mapReference) : IAzureMapsFeatures
    {
        private readonly AzureMapDotNetRef mapReference = mapReference;
        private string MapId => mapReference.MapId;
        private IJSRuntime JsRuntime => mapReference.JsRuntime;

        public async Task Add(MapFeatureDef feature, string datasourceId, bool replace = false)
        {
            await Add([feature], datasourceId, replace);
        }

        public async Task Add(IEnumerable<MapFeatureDef> features, string datasourceId, bool replace = false)
        {
            features.EnsureHasId();
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, features, datasourceId, replace);
        }

        public async Task Update(MapFeatureDef feature, string datasourceId)
        {
            await Update([feature], datasourceId);
        }

        public async Task Update(IEnumerable<MapFeatureDef> features, string datasourceId)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, features, datasourceId);
        }

        public async Task AddProperty(MapFeatureDef feature, string name, object? value, string datasourceId)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, feature.Id, name, value, datasourceId);
        }

        public async Task<Properties> GetProperties(MapFeatureDef feature, string datasourceId)
        {
            return await JsRuntime.InvokeAsync<Properties>(GetJsInteropMethod(), MapId, feature.Id, datasourceId);
        }

        public async Task SetProperties(MapFeatureDef feature, string datasourceId, bool replace = false)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, feature.Id, feature.Properties, datasourceId, replace);
        }

        public async Task<object> GetCoordinates(MapFeatureDef feature, string datasourceId)
        {
            return feature.GeometryType switch
            {
                GeometryType.Point => await JsRuntime.InvokeAsync<Position>(GetJsInteropMethod(), MapId, feature.Id, datasourceId),
                GeometryType.MultiPoint => await JsRuntime.InvokeAsync<List<Position>>(GetJsInteropMethod(), MapId, feature.Id, datasourceId),
                GeometryType.LineString => await JsRuntime.InvokeAsync<List<Position>>(GetJsInteropMethod(), MapId, feature.Id, datasourceId),
                GeometryType.MultiLineString => await JsRuntime.InvokeAsync<List<List<Position>>>(GetJsInteropMethod(), MapId, feature.Id, datasourceId),
                GeometryType.Polygon => await JsRuntime.InvokeAsync<List<List<Position>>>(GetJsInteropMethod(), MapId, feature.Id, datasourceId),
                GeometryType.MultiPolygon => await JsRuntime.InvokeAsync<List<List<List<Position>>>>(GetJsInteropMethod(), MapId, feature.Id, datasourceId),
                _ => throw new ArgumentOutOfRangeException(nameof(feature)),
            };
        }

        public async Task SetCoordinates(MapFeatureDef feature, string datasourceId)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, feature.Id, feature.Coordinates, datasourceId);
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => MapExtensions.GetJsModuleMethod(JsModule.Features, name);
    }
}
