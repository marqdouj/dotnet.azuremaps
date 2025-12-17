using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventMouseArgs : MapEventArgs<MapEventMouseArgsPayload> { }

    public class MapEventMouseArgsPayload : MapEventPayloadBase
    {
        public MapEventMousePayload? Mouse { get; set; }
    }

    public class MapEventMousePayload
    {
        /// <summary>
        /// The id of the layer the event is attached to.
        /// </summary>
        [JsonInclude] public string? LayerId { get; internal set; }

        /// <summary>
        /// The pixel coordinate where the event occurred [x, y].
        /// </summary>
        [JsonInclude] public Pixel? Pixel { get; internal set; }

        /// <summary>
        /// The geographical location of all touch points on the map.
        /// </summary>
        [JsonInclude] public Position? Position { get; internal set; }

        /// <summary>
        /// An array of Shape and Feature objects that the mouse event occurred on.
        /// </summary>
        [JsonInclude] public List<MapEventShape>? Shapes { get; internal set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }

    public enum MapEventShapeSource
    {
        /// <summary>
        /// Source is a GeoJSON Feature
        /// </summary>
        Feature,

        /// <summary>
        /// Source is a GeoJSON Feature wrapped in a map shape
        /// </summary>
        Shape,
    }

    public class MapEventShape()
    {
        /// <summary>
        /// The id of the shape.
        /// </summary>
        [JsonInclude]
        public string? Id { get; internal set; }

        /// <summary>
        /// The type of geometry this shape contains.
        /// </summary>
        [JsonIgnore]
        public GeometryType? Type { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("type")]
        internal string? TypeJs { get => Type.EnumToJsonN(); set => Type = value.JsonToEnumN<GeometryType>(); }

        /// <summary>
        /// As shape can be added to the map as a GeoJSON Feature or an Azure Map Shape.
        /// Denotes which of these the shape represents.
        /// </summary>
        [JsonIgnore]
        public MapEventShapeSource? Source { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("source")]
        internal string? SourceJs { get => Source.EnumToJsonN(); set => Source = value.JsonToEnumN<MapEventShapeSource>(); }

        /// <summary>
        /// The bounding box of the shape
        /// </summary>
        [JsonInclude]
        public BoundingBox? Bbox { get; internal set; }

        /// <summary>
        /// The properties of the shape.
        /// </summary>
        [JsonInclude]
        public Properties? Properties { get; internal set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
