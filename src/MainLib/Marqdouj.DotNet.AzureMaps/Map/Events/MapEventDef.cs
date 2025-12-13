using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public enum MapEventTarget
    {
        Map,
        DataSource,
        HtmlMarker,
        Layer,
        Popup,
        Shape,
        StyleControl,
    }

    public class MapEventDef
    {
        [JsonConstructor]
        public MapEventDef() { }

        public MapEventDef(MapEventType type, MapEventTarget target)
        {
            Type = type;
            Target = target;
        }

        [JsonIgnore]
        public MapEventType? Type { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("type")]
        internal string? TypeJs { get => Type?.EnumToJson(); set => Type = value.JsonToEnumN<MapEventType>(); }

        /// <summary>
        /// Type of object associated with the event.
        /// </summary>
        [JsonIgnore]
        public MapEventTarget? Target { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("target")]
        internal string? TargetJs { get => Target.EnumToJsonN(); set => Target = value.JsonToEnumN<MapEventTarget>(); }

        /// <summary>
        /// Required for any Target other than 'Map'.
        /// For the StyleControl use the 'InteropId'
        /// </summary>
        public string? TargetId { get; set; }

        /// <summary>
        /// Required for targets that belong to a source, i.e. Shape requires DataSourceId.
        /// </summary>
        public string? TargetSourceId { get; set; }

        /// <summary>
        /// If true, adds the event once (for events that support 'once'); otherwise continuous. Default is 'false'.
        /// </summary>
        public bool Once { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
