using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using Marqdouj.DotNet.AzureMaps.Map.Interop;
using Marqdouj.DotNet.AzureMaps.Map.Options;

namespace Sandbox.Components.Pages.Maps
{
    internal static class MapExtensions
    {
        public static async Task ZoomTo(this MapInterop mapInterop, Position center, double zoomLevel)
        {
            if (mapInterop is null) return;

            var options = new CameraOptions { Center = center, Zoom = zoomLevel };
            await mapInterop.Configuration.SetCamera(options);
        }
    }
}
