using Marqdouj.DotNet.AzureMaps.Map.Configuration;
using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
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
        public async Task<CameraOptionsGet> GetCamera()
        {
            return await jsRuntime.InvokeAsync<CameraOptionsGet>(GetMapInteropMethod(), MapId);
        }

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
        public async Task SetCamera(CameraOptions? camera, CameraBoundsOptionsSet? cameraBounds = null, AnimationOptions? animation = null)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, new CameraOptionsSet(camera, cameraBounds, animation));
        }

        /// <summary>
        /// Applies the specified map options to the current map instance.
        /// </summary>
        /// <param name="options">An object containing configuration settings for the map, such as display options, controls, and behavior.
        /// Cannot be null.</param>
        public async Task SetMapOptions(MapOptionsSet options)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, options);
        }

        /// <summary>
        /// Retrieves the current map options for the specified map instance.
        /// </summary>
        public async Task<MapOptionsGet> GetMapOptions()
        {
            return await jsRuntime.InvokeAsync<MapOptionsGet>(GetMapInteropMethod(), MapId);
        }

        /// <summary>
        /// Retrieves the current service options for the map instance.
        /// </summary>
        public async Task<ServiceOptions> GetServiceOptions()
        {
            return await jsRuntime.InvokeAsync<ServiceOptions>(GetMapInteropMethod(), MapId);
        }

        /// <summary>
        /// Updates the map's service options using the specified configuration.
        /// </summary>
        /// <param name="options">The service options to apply to the map. Cannot be null.</param>
        public async Task SetServiceOptions(ServiceOptions options)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, options);
        }

        /// <summary>
        /// Retrieves the current style options applied to the map.
        /// </summary>
        /// <returns>A <see cref="StyleOptions"/> object containing the map's current style settings.</returns>
        public async Task<StyleOptions> GetStyle()
        {
            return await jsRuntime.InvokeAsync<StyleOptions>(GetMapInteropMethod(), MapId);
        }

        /// <summary>
        /// Applies the specified style options to the map.
        /// </summary>
        /// <remarks>This method updates the map's appearance based on the provided style options. The
        /// changes are applied asynchronously and may not be immediately visible. If the map is not initialized, the
        /// operation may have no effect.</remarks>
        /// <param name="options">The style settings to apply to the map. Cannot be null.</param>
        public async Task SetStyle(StyleOptions options)
        {
            await jsRuntime.InvokeVoidAsync(GetMapInteropMethod(), MapId, options);
        }

        /// <summary>
        /// Retrieves the current user interaction options for the map.
        /// </summary>
        /// <remarks>The returned options reflect the current state and may change if the user modifies
        /// interaction settings.</remarks>
        /// <returns>A <see cref="UserInteractionOptions"/> instance containing the user's interaction settings for the map.</returns>
        public async Task<UserInteractionOptions> GetUserInteraction()
        {
            return await jsRuntime.InvokeAsync<UserInteractionOptions>(GetMapInteropMethod(), MapId);
        }

        /// <summary>
        /// Configures user interaction settings for the map.
        /// </summary>
        /// <remarks>Use this method to enable or disable specific user interactions, such as zooming or
        /// panning, on the map. The changes take effect immediately after the operation completes.</remarks>
        /// <param name="options">An object containing options that specify how users can interact with the map. Cannot be null.</param>
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
            => MapInterop.GetMapInteropMethod(MapInteropModule.Configuration, name);
    }
}
