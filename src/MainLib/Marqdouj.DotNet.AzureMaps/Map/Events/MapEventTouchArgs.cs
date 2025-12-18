using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventTouchArgs : MapEventArgs<MapEventTouchPayload>
    {
        public override string ToString() => JsonSerializer.Serialize(this);
    }

    public class MapEventTouchPayload : MapEventPayloadBase
    {
        /// <summary>
        /// The pixel coordinate at the center of all touch points on the map, relative to the top left corner.
        /// </summary>
        [JsonInclude] public Pixel? Pixel { get; internal set; }

        /// <summary>
        /// The array of pixel coordinates of all touch points on the map.
        /// </summary>
        [JsonInclude] public List<Pixel>? Pixels { get; internal set; }

        /// <summary>
        /// The geographic location of the center of all touch points on the map.
        /// </summary>
        [JsonInclude] public Position? Position { get; internal set; }

        /// <summary>
        /// The geographical location of all touch points on the map.
        /// </summary>
        [JsonInclude] public List<Position>? Positions { get; internal set; }

        /// <summary>
        /// The shapes of all touch points on the map.
        /// </summary>
        [JsonInclude] public List<MapEventShape>? Shapes { get; internal set; }

        /// <summary>
        /// The id of the layer the event is attached to.
        /// </summary>
        [JsonInclude] public string? LayerId { get; internal set; }
    }
}
