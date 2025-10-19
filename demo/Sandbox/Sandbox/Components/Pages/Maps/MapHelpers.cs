using Marqdouj.DotNet.AzureMaps.Map.Controls;

namespace Sandbox.Components.Pages.Maps
{
    internal static class MapHelpers
    {
        public static List<MapControl> GetDefaultControls()
        {
            var controls = new List<MapControl>()
            {
                 GetDefaultControl(MapControlType.Fullscreen),
                 GetDefaultControl(MapControlType.Zoom),
                 GetDefaultControl(MapControlType.Pitch),
                 GetDefaultControl(MapControlType.Compass),
                 GetDefaultControl(MapControlType.Style),
                 GetDefaultControl(MapControlType.Scale)
            };

            //Set the ZOrder based on position in the list
            var zOrder = 0;
            foreach (var control in controls)
            {
                control.SortOrder = zOrder;
                zOrder++;
            }

            return controls;
        }

        public static MapControl GetDefaultControl(MapControlType controlType)
        {
            return controlType switch
            {
                MapControlType.Compass => new CompassControl(MapControlPosition.Top_Right),
                MapControlType.Fullscreen => new FullscreenControl(MapControlPosition.Top_Right),
                MapControlType.Pitch => new PitchControl(MapControlPosition.Top_Right),
                MapControlType.Scale => new ScaleControl(MapControlPosition.Bottom_Right),
                MapControlType.Style => new StyleControl(MapControlPosition.Top_Right),
                MapControlType.Zoom => new ZoomControl(MapControlPosition.Top_Right),
                _ => throw new ArgumentOutOfRangeException(nameof(controlType)),
            };
        }
    }
}
