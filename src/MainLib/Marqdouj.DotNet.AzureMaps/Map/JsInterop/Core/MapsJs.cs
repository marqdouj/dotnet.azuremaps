using Marqdouj.DotNet.AzureMaps.Map.Configuration;
using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using Marqdouj.DotNet.AzureMaps.Map.JsInterop.Modules;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop.Core
{
    internal class MapsJs(AzureMapDotNetRef mapReference)
    {
        private readonly AzureMapDotNetRef mapReference = mapReference;
        private string MapId => mapReference.MapId;
        private IJSRuntime JsRuntime => mapReference.JsRuntime;

        public ControlManagerJs Controls { get; } = new ControlManagerJs(mapReference);
        public EventManagerJs Events { get; } = new EventManagerJs(mapReference);
        public FeatureManagerJs Features { get; } = new FeatureManagerJs(mapReference);
        public LayerManagerJs Layers { get; } = new LayerManagerJs(mapReference);
        public GeolocationManagerJs Geolocation { get; } = new GeolocationManagerJs(mapReference);
        public MarkerManagerJs Markers { get; } = new MarkerManagerJs(mapReference);
        public PopupManagerJs Popups { get; } = new PopupManagerJs(mapReference);
        public SourceManagerJs Sources { get; } = new SourceManagerJs(mapReference);

        #region Camera
        public async Task<CameraOptionsGet> GetCamera()
        {
            return await JsRuntime.InvokeAsync<CameraOptionsGet>(GetJsInteropMethod(), MapId);
        }

        public async Task SetCamera(CameraOptions? camera, CameraBoundsOptionsSet? cameraBounds = null, AnimationOptions? animation = null)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, new CameraOptionsSet(camera, cameraBounds, animation));
        }

        public async Task ZoomTo(Position center, double zoomLevel)
        {
            var camera = await GetCamera();
            var options = camera.ToCameraOptions();
            options.Center = center;
            options.Zoom = zoomLevel;
            await SetCamera(options);
        }
        #endregion

        #region MapOptions
        public async Task SetMapOptions(MapOptionsSet options)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, options);
        }

        public async Task<MapOptionsGet> GetMapOptions()
        {
            return await JsRuntime.InvokeAsync<MapOptionsGet>(GetJsInteropMethod(), MapId);
        }
        #endregion

        #region Service
        public async Task<ServiceOptions> GetServiceOptions()
        {
            return await JsRuntime.InvokeAsync<ServiceOptions>(GetJsInteropMethod(), MapId);
        }

        public async Task SetServiceOptions(ServiceOptions options)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, options);
        }
        #endregion

        #region Style
        public async Task<StyleOptions> GetStyle()
        {
            return await JsRuntime.InvokeAsync<StyleOptions>(GetJsInteropMethod(), MapId);
        }

        public async Task SetStyle(StyleOptions options)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, options);
        }
        #endregion

        #region Traffic
        public async Task<TrafficOptions> GetTraffic()
        {
            return await JsRuntime.InvokeAsync<TrafficOptions>(GetJsInteropMethod(), MapId);
        }

        public async Task SetTraffic(TrafficOptions? options)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, options);
        }
        #endregion

        #region UserInteraction
        public async Task<UserInteractionOptions> GetUserInteraction()
        {
            return await JsRuntime.InvokeAsync<UserInteractionOptions>(GetJsInteropMethod(), MapId);
        }

        public async Task SetUserInteraction(UserInteractionOptions options)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, options);
        }
        #endregion

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => MapExtensions.GetJsModuleMethod(JsModule.Maps, name);
    }
}
