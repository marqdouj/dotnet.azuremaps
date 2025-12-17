using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventPopupArgs : MapEventArgs<MapEventPopupPayload>
    {
    }

    public class MapEventPopupPayload : MapEventPayloadBase
    {
        [JsonInclude]
        public string? Type { get; internal set; }
    }
}
