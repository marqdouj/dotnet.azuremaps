using Marqdouj.DotNet.AzureMaps.Map.JsInterop.Core;
using Microsoft.JSInterop;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop
{
    internal class AzureMapJsInterop : IAsyncDisposable
    {
        private readonly AzureMapDotNetRef mapReference;

        public AzureMapJsInterop(IJSRuntime jsRuntime, AzureMap map)
        {
            mapReference = new AzureMapDotNetRef(jsRuntime, map);
            Factory = new FactoryJs(mapReference);
            Maps = new MapsJs(mapReference);
        }

        public string MapId => mapReference.MapId;
        internal IJSRuntime JsRuntime => mapReference.JsRuntime;
        internal FactoryJs Factory { get; }
        public MapsJs Maps { get; }

        public async ValueTask DisposeAsync()
        {
            await Factory.RemoveMap();
            await mapReference.DisposeAsync();
        }
    }
}
