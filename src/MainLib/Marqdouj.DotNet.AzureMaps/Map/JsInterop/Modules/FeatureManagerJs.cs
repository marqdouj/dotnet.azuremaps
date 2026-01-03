using Marqdouj.DotNet.AzureMaps.Map.Layers;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop.Modules
{
    internal class FeatureManagerJs(AzureMapDotNetRef mapReference)
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

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => MapExtensions.GetJsModuleMethod(JsModule.Features, name);
    }
}
