using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventHtmlMarkerArgs : MapEventArgs<MapEventHtmlMarkerPayload>
    {
        public override string ToString() => JsonSerializer.Serialize(this);
    }

    public class MapEventHtmlMarkerPayload : MapEventPayloadBase
    {
        [JsonInclude]
        public string? Type { get; internal set; }
    }
}
