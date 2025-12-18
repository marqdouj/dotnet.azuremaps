using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventErrorArgs : MapEventArgs<MapEventErrorPayload>
    {
        public override string ToString() => JsonSerializer.Serialize(this);
    }

    public class MapEventErrorPayload : MapEventPayloadBase
    {
       [JsonInclude] public string? Name { get; internal set; }
       [JsonInclude] public string? Message { get; internal set; }
       [JsonInclude] public string? Stack { get; internal set; }
       [JsonInclude] public string? Cause { get; internal set; }
    }
}
