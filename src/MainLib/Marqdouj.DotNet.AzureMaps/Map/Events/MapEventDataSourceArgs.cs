using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventDataSourceArgs : MapEventArgs<MapEventDataSourcePayload>
    {
        public override string ToString() => JsonSerializer.Serialize(this);
    }

    public class MapEventDataSourcePayload : MapEventPayloadBase
    {
        /// <summary>
        /// An array of Shape and Feature objects associated with the event.
        /// </summary>
        [JsonInclude] public List<MapEventShape>? Shapes { get; internal set; }

    }
}
