using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventDataSourceArgs : MapEventArgs<MapEventDataSourcePayload> { }

    public class MapEventDataSourcePayload
    {
        /// <summary>
        /// DataSource Id associated with the event.
        /// </summary>
        [JsonInclude]
        public string? SourceId { get; internal set; }

        /// <summary>
        /// An array of Shape and Feature objects associated with the event.
        /// </summary>
        [JsonInclude] public List<MapEventShape>? Shapes { get; internal set; }

    }
}
