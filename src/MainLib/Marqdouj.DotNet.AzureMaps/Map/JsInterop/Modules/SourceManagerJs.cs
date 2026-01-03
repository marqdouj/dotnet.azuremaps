using Marqdouj.DotNet.AzureMaps.Map.Events;
using Marqdouj.DotNet.AzureMaps.Map.Layers;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop.Modules
{
    internal class SourceManagerJs(AzureMapDotNetRef mapReference)
    {
        private readonly AzureMapDotNetRef mapReference = mapReference;
        private string MapId => mapReference.MapId;
        private IJSRuntime JsRuntime => mapReference.JsRuntime;

        public async Task Add(IEnumerable<SourceDef> items)
        {
            items.EnsureHasId();
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, items?.Cast<object>().ToList());
        }

        public async Task Remove(IEnumerable<SourceDef> items)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, items?.Cast<object>().ToList());
        }

        public async Task RemoveById(IEnumerable<string> sourceIds)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, sourceIds);
        }

        public async Task Clear(IEnumerable<SourceDef> items)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, items?.Cast<object>().ToList());
        }

        public async Task ClearById(IEnumerable<string> sourceIds)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, sourceIds);
        }

        public async Task<List<MapEventShape>> GetShapes(string sourceId)
        {
            return await JsRuntime.InvokeAsync<List<MapEventShape>>(GetJsInteropMethod(), MapId, sourceId);
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => MapExtensions.GetJsModuleMethod(JsModule.Sources, name);
    }
}
