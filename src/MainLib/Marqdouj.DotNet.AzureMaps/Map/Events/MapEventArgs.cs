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

        public override string ToString()
        {
            return $"MapID:{MapId} Type:{Type}";
        }
    }

    public class MapEventArgs<T> : MapEventArgs where T : class
    {
        [JsonInclude]
        public T? Payload { get; internal set; }
    }
}
