using Marqdouj.DotNet.AzureMaps.Map.Controls;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop.Modules
{
    internal class ControlManagerJs(AzureMapDotNetRef mapReference)
    {
        private readonly AzureMapDotNetRef mapReference = mapReference;
        private string MapId => mapReference.MapId;
        private IJSRuntime JsRuntime => mapReference.JsRuntime;

        public async Task Add(IEnumerable<MapControl> items)
        {
            items.EnsureHasId();
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, items?.Cast<object>().ToList());
        }

        public async Task Remove(IEnumerable<MapControl> items)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, items?.Cast<object>().ToList());
        }

        public async Task<List<MapControl>> GetControls()
        {
            var controls = await JsRuntime.InvokeAsync<List<MapControl>>(GetJsInteropMethod(), MapId);
            return [.. controls.OrderBy(e => e.Type)];
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => MapExtensions.GetJsModuleMethod(JsModule.Controls, name);
    }
}
