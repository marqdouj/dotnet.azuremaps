using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using Marqdouj.DotNet.AzureMaps.Map.Options;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop
{
    public class MapInteropConfiguration
    {
        private readonly IJSRuntime jsRuntime;

        internal MapInteropConfiguration(IJSRuntime jsRuntime, string mapId)
        {
            this.jsRuntime = jsRuntime;
            MapId = mapId;
        }

        public string MapId { get; }

        /// <summary>
        /// The camera's current properties, as a combination of Camera and CameraBounds options
        /// </summary>
        /// <returns></returns>
        public async Task<MapCamera> GetCamera()
        {
            return await jsRuntime.InvokeAsync<MapCamera>(GetMapInteropMethod(), MapId);
        }

        public async Task SetMapOptions(MapOptions options)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, options);
        }

        public async Task<MapOptions> GetMapOptions()
        {
            return await jsRuntime.InvokeAsync<MapOptions>(GetMapInteropMethod(), MapId);
        }

        public async Task SetCamera(CameraOptions? camera, SetCameraBoundsOptions? cameraBounds = null, AnimationOptions? animation = null)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, new  SetCameraOptions(camera, cameraBounds, animation));
        }

        public async Task<ServiceOptions> GetServiceOptions()
        {
            return await jsRuntime.InvokeAsync<ServiceOptions>(GetMapInteropMethod(), MapId);
        }

        public async Task SetServiceOptions(ServiceOptions options)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, options);
        }

        public async Task<StyleOptions> GetStyle()
        {
            return await jsRuntime.InvokeAsync<StyleOptions>(GetMapInteropMethod(), MapId);
        }

        public async Task SetStyle(StyleOptions options)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, options);
        }

        public async Task<UserInteractionOptions> GetUserInteraction()
        {
            return await jsRuntime.InvokeAsync<UserInteractionOptions>(GetMapInteropMethod(), MapId);
        }

        public async Task SetUserInteraction(UserInteractionOptions options)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, options);
        }

        public async Task ZoomTo(Position center, double zoomLevel)
        {
            var camera = await GetCamera();
            var options = camera.ToCameraOptions();
            options.Center = center;
            options.Zoom = zoomLevel;
            await SetCamera(options);
        }

        private static string GetMapInteropMethod([CallerMemberName] string name = "")
        {
            return $"{MapExtensions.LIBRARY_NAME}.MapInterop.Configuration.{name.ToJsonName()}";
        }
    }
}
