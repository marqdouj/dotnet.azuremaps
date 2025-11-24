using System.Text.Json;

namespace Marqdouj.DotNet.AzureMaps.Map.Options
{
    /// <summary>
    /// Represents configuration options for initializing and customizing a map.
    /// </summary>
    /// <remarks>
    /// This class provides various settings to control the map's behavior, appearance, and services.
    /// Use the properties to configure specific aspects of the map, such as camera settings, style, and service
    /// options.
    /// </remarks>
    public class MapOptions : ICloneable
    {
        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

        private object? camera;

        /// <summary>
        /// CameraOptions | CameraBoundsOptions | Camera
        /// </summary>
        public object? Camera
        {
            get => camera;
            set
            {
                // JsonElement is assigned when deserializing from GetMapOptions
                if (value is JsonElement json)
                {
                    if (json.ValueKind == JsonValueKind.Null)
                    {
                        camera = null;
                        return;
                    }

                    camera = json.Deserialize<MapCamera>(jsonOptions);
                    return;
                }

                if (value is CameraOptions || value is CameraBoundsOptions || value == null)
                {
                    camera = value;
                }
                else
                {
                    throw new ArgumentException("Camera must be of type CameraOptions, CameraBoundsOptions or Null.");
                }
            }
        }

        public ServiceOptions? Service { get; set; }
        public StyleOptions? Style { get; set; }
        public UserInteractionOptions? UserInteraction { get; set; }

        public object Clone()
        {
            var clone = new MapOptions
            {
                Camera = ((ICloneable?)Camera)?.Clone(),
                Service = (ServiceOptions?)Service?.Clone(),
                Style = (StyleOptions?)Style?.Clone(),
                UserInteraction = (UserInteractionOptions?)UserInteraction?.Clone()
            };

            return clone;
        }
    }

    internal static class MapOptionExtensions
    {
        /// <summary>
        /// Merges the default (MapConfiguration) options with the user (Parameter) options
        /// </summary>
        /// <param name="source"></param>
        /// <param name="updated"></param>
        /// <returns></returns>
        public static MapOptions Merge(this MapOptions? source, MapOptions? updated)
        {
            var options = new MapOptions
            {
                Camera = updated?.Camera ?? source?.Camera,
                Service = updated?.Service ?? source?.Service,
                Style = updated?.Style ?? source?.Style,
                UserInteraction = updated?.UserInteraction ?? source?.UserInteraction
            };

            return options;
        }
    }
}
