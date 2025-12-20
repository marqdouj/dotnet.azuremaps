using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Configuration
{
    public enum TrafficFlow
    {
        /// <summary>
        /// Display no traffic.
        /// </summary>
        None,

        /// <summary>
        /// The speed of the road relative to free-flow
        /// </summary>
        Relative,
    }

    /// <summary>
    /// The options for setting traffic on the map.
    /// </summary>
    public class TrafficOptions
    {
        /// <summary>
        /// The type of traffic flow to display:
        /// "none" is to display no traffic flow data
        /// "relative" is the speed of the road relative to free-flow
        /// Default is "none".
        /// </summary>
        [JsonIgnore]
        public TrafficFlow? Flow { get; set; } // Possible values: "none", "relative", "absolute", "relative-delay"

        [JsonInclude]
        [JsonPropertyName("flow")]
        public string? FlowJs { get => Flow?.ToString().ToLower(); set => Flow = value.JsonToEnumN<TrafficFlow>(); }

        /// <summary>
        /// Whether to display incidents on the map.
        /// Default is false.
        /// </summary>
        public bool? Incidents { get; set; }
    }
}
