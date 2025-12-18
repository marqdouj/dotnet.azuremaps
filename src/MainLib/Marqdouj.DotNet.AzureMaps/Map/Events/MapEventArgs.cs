using Marqdouj.DotNet.AzureMaps.Map.Common;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventArgs
    {
        [JsonInclude]
        public string MapId { get; internal set; } = "";

        [JsonIgnore]
        public MapEventType? Type { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("type")]
        internal string? TypeJs { get => Type.EnumToJsonN(); set => Type = value.JsonToEnumN<MapEventType>(); }

        [JsonIgnore]
        public MapEventTarget? Target { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("target")]
        internal string? TargetJs { get => Target.EnumToJsonN(); set => Target = value.JsonToEnumN<MapEventTarget>(); }

        public override string ToString()
        {
            return $"MapId:{MapId} Target:{Target} Type:{Type}";
        }
    }

    public class MapEventArgs<T> : MapEventArgs where T : MapEventPayloadBase
    {
        [JsonInclude]
        public T? Payload { get; internal set; }
    }

    public class MapEventPayloadBase
    {
        [JsonInclude]
        public string? Id { get; internal set; }

        [JsonInclude]
        public string? TargetId { get; internal set; }

        [JsonInclude]
        public JSInteropDef? JsInterop { get; internal set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
