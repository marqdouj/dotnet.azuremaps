using Microsoft.JSInterop;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop
{
    internal class AzureMapDotNetRef(IJSRuntime jsRuntime, AzureMap map) : IAsyncDisposable
    {
        public DotNetObjectReference<AzureMap> DotNetRef { get; } = DotNetObjectReference.Create(map)
                ?? throw new Exception("Unable to create DotNetObjectReference<AzureMap>.");

        public string MapId => DotNetRef.Value.Id ?? "";

        public IJSRuntime JsRuntime { get; } = jsRuntime;

        public async ValueTask DisposeAsync()
        {
            DotNetRef?.Dispose();
        }
    }
}
