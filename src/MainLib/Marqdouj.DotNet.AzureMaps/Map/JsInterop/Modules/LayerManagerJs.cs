using Marqdouj.DotNet.AzureMaps.Map.Events;
using Marqdouj.DotNet.AzureMaps.Map.Layers;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop.Modules
{
    internal class LayerManagerJs(AzureMapDotNetRef mapReference)
    {
        private readonly AzureMapDotNetRef mapReference = mapReference;
        private string MapId => mapReference.MapId;
        private IJSRuntime JsRuntime => mapReference.JsRuntime;

        public async Task Add(IEnumerable<MapLayerDef> items, IEnumerable<MapEventDef>? events = null)
        {
            items.EnsureHasId();
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, items?.Cast<object>().ToList(), events?.Cast<object>().ToList());
        }

        public async Task Remove(IEnumerable<MapLayerDef> items)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, items?.Cast<object>().ToList());
        }

        public async Task RemoveById(IEnumerable<string> layers)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, layers);
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => MapExtensions.GetJsModuleMethod(JsModule.Layers, name);

    }
}
