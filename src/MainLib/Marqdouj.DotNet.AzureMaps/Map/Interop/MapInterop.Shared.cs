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
#pragma warning disable CS0618 // Type or member is obsolete
            => MapInterop.GetMapInteropMethod(module, name);
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
