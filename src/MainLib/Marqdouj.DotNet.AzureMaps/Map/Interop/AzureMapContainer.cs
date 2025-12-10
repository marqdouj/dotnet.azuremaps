using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop
{
    /// <summary>
    /// Manages Azure Map JS Interactions
    /// </summary>
    public interface IAzureMapContainer
    {
        string MapId { get; }
        IAzureMapsLayers Layers { get; }
        IAzureMapsConfiguration Configuration { get; }
        IAzureMapsMaps Maps { get; }
    }

    internal sealed class AzureMapContainer : IAsyncDisposable, IAzureMapContainer
    {
        private readonly AzureMapReference mapReference;

        internal AzureMapContainer(IJSRuntime jsRuntime, AzureMap map)
        {
            mapReference = new AzureMapReference(jsRuntime, map);
            Factory = new AzureMapsFactory(mapReference);
            Configuration = new AzureMapsConfiguration(mapReference);
            Layers = new AzureMapsLayers(mapReference);
            Maps = new AzureMapsMaps(mapReference);
        }

        public string MapId => mapReference.MapId;

        internal AzureMapsFactory Factory { get; }

        public IAzureMapsConfiguration Configuration { get; }
        public IAzureMapsLayers Layers { get; }
        public IAzureMapsMaps Maps { get; }

        public async ValueTask DisposeAsync()
        {
            await Factory.RemoveMap();
            await mapReference.DisposeAsync();
        }

        internal static string GetMapInteropMethod(MapInteropModule module, [CallerMemberName] string name = "")
            => $"{MapExtensions.LIBRARY_NAME}.{module}.{name.ToJsonName()}";
    }
}
