using Marqdouj.DotNet.AzureMaps.Map.Configuration;
using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using Marqdouj.DotNet.AzureMaps.Map.Interop;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop
{
    internal class MapContainerConfiguration(AzureMapJsInterop mapJsInterop) : IAzureMapsConfiguration
    {
        private readonly AzureMapJsInterop mapJsInterop = mapJsInterop;

        public async Task<CameraOptionsGet> GetCamera()
        {
            return await mapJsInterop.Maps.GetCamera();
        }

        public async Task<MapOptionsGet> GetMapOptions()
        {
            return await mapJsInterop.Maps.GetMapOptions();
        }

        public async Task<ServiceOptions> GetServiceOptions()
        {
            return await mapJsInterop.Maps.GetServiceOptions();
        }

        public async Task<StyleOptions> GetStyle()
        {
            return await mapJsInterop.Maps.GetStyle();
        }

        public async Task<UserInteractionOptions> GetUserInteraction()
        {
            return await mapJsInterop.Maps.GetUserInteraction();
        }

        public async Task SetCamera(CameraOptions? camera, CameraBoundsOptionsSet? cameraBounds = null, AnimationOptions? animation = null)
        {
            await mapJsInterop.Maps.SetCamera(camera, cameraBounds, animation);
        }

        public async Task SetMapOptions(MapOptionsSet options)
        {
            await mapJsInterop.Maps.SetMapOptions(options);
        }

        public async Task SetServiceOptions(ServiceOptions options)
        {
            await mapJsInterop.Maps.SetServiceOptions(options);
        }

        public async Task SetStyle(StyleOptions options)
        {
            await mapJsInterop.Maps.SetStyle(options);
        }

        public async Task SetUserInteraction(UserInteractionOptions options)
        {
            await mapJsInterop.Maps.SetUserInteraction(options);
        }

        public async Task ZoomTo(Position center, double zoomLevel)
        {
            await mapJsInterop.Maps.ZoomTo(center, zoomLevel);
        }
    }
}
