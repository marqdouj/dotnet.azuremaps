using Marqdouj.DotNet.AzureMaps.Map.Configuration;
using Marqdouj.DotNet.AzureMaps.Map.Controls;
using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Sandbox.Components.Pages.Maps
{
    internal static class MapHelpers
    {
        public static DialogParameters GetLayerDialogParameters()
        {
            DialogParameters parameters = new()
            {
                Title = $"Layer Settings",
                PrimaryAction = "OK",
                PrimaryActionEnabled = true,
                SecondaryAction = "Cancel",
                Width = "60%",
                TrapFocus = true,
                Modal = false,
                PreventScroll = true
            };

            return parameters;
        }

        public static MapOptions GetDefaultCreateMapOptions()
        {
            // Initialize map options with a specific camera position
            return new MapOptions
            {
                Camera = new CameraOptions
                {
                    Center = new Position(-122.33, 47.6), // (Seattle, WA)
                    Zoom = 10.5,
                    Pitch = 0,
                }
            };
        }

        public static MapOptionsSet GetDefaultSetMapOptions()
        {
            // Initialize map options with a specific camera position
            return new MapOptionsSet
            {
                Camera = new CameraOptionsSet
                {
                    Camera = new CameraOptions
                    {
                        Center = new Position(-122.33, 47.6), // (Seattle, WA)
                        Zoom = 10.5,
                        Pitch = 0,
                    },
                },
                Style = new(),
            };
        }

        public static List<MapControl> GetDefaultControls()
        {
            var controls = new List<MapControl>()
            {
                 GetDefaultControl(MapControlType.Fullscreen),
                 GetDefaultControl(MapControlType.Zoom),
                 GetDefaultControl(MapControlType.Pitch),
                 GetDefaultControl(MapControlType.Compass),
                 GetDefaultControl(MapControlType.Style),
                 GetDefaultControl(MapControlType.Traffic),
                 GetDefaultControl(MapControlType.TrafficLegend),
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
                MapControlType.Style => new StyleControl(MapControlPosition.Top_Right) { Options = GetStyleControlOptions() },
                MapControlType.Traffic => new TrafficControl(MapControlPosition.Top_Right),
                MapControlType.TrafficLegend => new TrafficLegendControl(MapControlPosition.Bottom_Left),
                MapControlType.Zoom => new ZoomControl(MapControlPosition.Top_Right),
                _ => throw new ArgumentOutOfRangeException(nameof(controlType)),
            };
        }

        private static StyleControlOptions GetStyleControlOptions()
        {
            List<MapStyle> styles = [.. Enum.GetValues<MapStyle>()];
            styles.Remove(MapStyle.Blank);
            styles.Remove(MapStyle.Blank_Accessible);
            return new StyleControlOptions { MapStyles = styles };
        }
    }
}
