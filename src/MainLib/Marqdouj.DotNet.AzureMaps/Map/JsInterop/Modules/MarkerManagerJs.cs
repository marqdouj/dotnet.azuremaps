using Marqdouj.DotNet.AzureMaps.Map.Common;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop.Modules
{
    internal class MarkerManagerJs(AzureMapDotNetRef mapReference)
    {
        private readonly AzureMapDotNetRef mapReference = mapReference;
        private string MapId => mapReference.MapId;
        private IJSRuntime JsRuntime => mapReference.JsRuntime;

        public async Task Add(IEnumerable<HtmlMarkerDef> items)
        {
            items.EnsureHasId();
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, items?.Cast<object>().ToList());
        }

        public async Task Remove(IEnumerable<HtmlMarkerDef> items)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, items?.Cast<object>().ToList());
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => MapExtensions.GetJsModuleMethod(JsModule.Markers, name);
    }
}
