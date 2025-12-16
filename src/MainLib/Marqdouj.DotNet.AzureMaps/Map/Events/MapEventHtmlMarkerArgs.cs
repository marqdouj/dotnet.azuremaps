using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventHtmlMarkerArgs : MapEventArgs<MapEventHtmlMarkerPayload>
    {
    }

    public class MapEventHtmlMarkerPayload : MapEventPayloadBase
    {
        [JsonInclude]
        public string? Type { get; internal set; }
    }
}
