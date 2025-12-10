using Marqdouj.DotNet.AzureMaps.Map.Configuration;
using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop
{
    public interface IAzureMapsConfiguration
    {
        /// <summary>
        /// The camera's current properties, as a combination of Camera and CameraBounds options
        /// </summary>
        Task<CameraOptionsGet> GetCamera();

        /// <summary>
        /// Retrieves the current map options for the map.
        /// </summary>
        Task<MapOptionsGet> GetMapOptions();

        /// <summary>
        /// Retrieves the current service options for the map 
        /// </summary>
        Task<ServiceOptions> GetServiceOptions();

        /// <summary>
        /// Retrieves the current style options applied to the map.
        /// </summary>
        Task<StyleOptions> GetStyle();

        /// <summary>
        /// Retrieves the current user interaction options for the map.
        /// </summary>
        Task<UserInteractionOptions> GetUserInteraction();

        /// <summary>
        /// Sets the camera position, bounds, and animation for the map.
        /// Camera and CameraBounds options are mutually exclusive; only one should be provided at a time.
        /// Camera has precedence over CameraBounds if both are provided.
        /// </summary>
        /// <param name="camera">The camera options specifying the target position, zoom, bearing, and pitch to apply. If null, no camera
        /// position change is performed.</param>
        /// <param name="cameraBounds">The bounds options that constrain the camera movement to a specific geographic area. If null, no bounds
        /// constraint is applied.</param>
        /// <param name="animation">The animation options that control how the camera transition is animated. If null, the camera change occurs
        /// instantly without animation.</param>
        Task SetCamera(CameraOptions? camera, CameraBoundsOptionsSet? cameraBounds = null, AnimationOptions? animation = null);

        /// <summary>
        /// Applies the specified map options to the current map instance.
        /// </summary>
        /// <param name="options">The map options to apply to the map.
        /// Cannot be null.</param>
        Task SetMapOptions(MapOptionsSet options);

        /// <summary>
        /// Updates the map's service options using the specified configuration.
        /// </summary>
        /// <param name="options">The service options to apply to the map.</param>
        Task SetServiceOptions(ServiceOptions options);

        /// <summary>
        /// Applies the specified style options to the map.
        /// </summary>
        /// <param name="options">The style settings to apply to the map.</param>
        Task SetStyle(StyleOptions options);

        /// <summary>
        /// Configures user interaction settings for the map.
        /// </summary>
        /// <param name="options">The user interaction settings to apply to the map.</param>
        Task SetUserInteraction(UserInteractionOptions options);

        /// <summary>
        /// Zoom the map to a specific position and level.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="zoomLevel"></param>
        /// <returns></returns>
        Task ZoomTo(Position center, double zoomLevel);
    }

    internal sealed class AzureMapsConfiguration(AzureMapReference mapReference) : IAzureMapsConfiguration
    {
        private readonly AzureMapReference mapReference = mapReference;
        private string MapId => mapReference.MapId;
        private IJSRuntime JsRuntime => mapReference.JsRuntime;

        public async Task<CameraOptionsGet> GetCamera()
        {
            return await JsRuntime.InvokeAsync<CameraOptionsGet>(GetMapInteropMethod(), MapId);
        }

        public async Task SetCamera(CameraOptions? camera, CameraBoundsOptionsSet? cameraBounds = null, AnimationOptions? animation = null)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, new CameraOptionsSet(camera, cameraBounds, animation));
        }

        public async Task SetMapOptions(MapOptionsSet options)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, options);
        }

        public async Task<MapOptionsGet> GetMapOptions()
        {
            return await JsRuntime.InvokeAsync<MapOptionsGet>(GetMapInteropMethod(), MapId);
        }

        public async Task<ServiceOptions> GetServiceOptions()
        {
            return await JsRuntime.InvokeAsync<ServiceOptions>(GetMapInteropMethod(), MapId);
        }

        public async Task SetServiceOptions(ServiceOptions options)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, options);
        }

        public async Task<StyleOptions> GetStyle()
        {
            return await JsRuntime.InvokeAsync<StyleOptions>(GetMapInteropMethod(), MapId);
        }

        public async Task SetStyle(StyleOptions options)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, options);
        }

        public async Task<UserInteractionOptions> GetUserInteraction()
        {
            return await JsRuntime.InvokeAsync<UserInteractionOptions>(GetMapInteropMethod(), MapId);
        }

        public async Task SetUserInteraction(UserInteractionOptions options)
        {
            await JsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, options);
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
            => AzureMapContainer.GetMapInteropMethod(MapInteropModule.Configuration, name);
    }
}
