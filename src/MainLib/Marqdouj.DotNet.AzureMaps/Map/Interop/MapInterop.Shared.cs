using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop
{
    internal class MapInteropShared(IJSRuntime jsRuntime, string mapId)
    {
        private readonly IJSRuntime jsRuntime = jsRuntime;

        public string MapId { get; } = mapId;

        public async Task RemoveDatasource(string sourceId)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(MapInteropModule.Maps), MapId, sourceId);
        }

        private static string GetMapInteropMethod(MapInteropModule module, [CallerMemberName] string name = "")
            => MapInterop.GetMapInteropMethod(module, name);
    }
}
