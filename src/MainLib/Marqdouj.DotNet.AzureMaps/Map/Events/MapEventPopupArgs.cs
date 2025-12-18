using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventPopupArgs : MapEventArgs<MapEventPopupPayload>
    {
        public override string ToString() => JsonSerializer.Serialize(this);
    }

    public class MapEventPopupPayload : MapEventPayloadBase
    {
        [JsonInclude]
        public string? Type { get; internal set; }
    }
}
