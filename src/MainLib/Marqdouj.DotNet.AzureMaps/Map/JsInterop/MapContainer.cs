using Marqdouj.DotNet.AzureMaps.Map.Interop;
using Marqdouj.DotNet.AzureMaps.Map.JsInterop.Core;
using Microsoft.JSInterop;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop
{
    internal class MapContainer : IAzureMapContainer, IAsyncDisposable
    {
        private readonly AzureMapJsInterop mapJsInterop;

        public MapContainer(IJSRuntime jsRuntime, AzureMap map)
        {
            mapJsInterop = new AzureMapJsInterop(jsRuntime, map);
            Configuration = new MapContainerConfiguration(mapJsInterop);
            Layers = new MapContainerLayers(mapJsInterop);
            Maps = new MapContainerMaps(mapJsInterop);
        }

        public string MapId => mapJsInterop.MapId;

        public IAzureMapsConfiguration Configuration { get; }
        public IAzureMapsLayers Layers { get; }
        public IAzureMapsMaps Maps { get; }

        internal FactoryJs Factory => mapJsInterop.Factory;

        public async ValueTask DisposeAsync()
        {
            await mapJsInterop.DisposeAsync();
        }
    }
}
