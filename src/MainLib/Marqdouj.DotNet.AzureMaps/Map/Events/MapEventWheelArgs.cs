using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventWheelArgs : MapEventArgs<MapEventWheelPayload>
    {
        public override string ToString() => JsonSerializer.Serialize(this);
    }

    public class MapEventWheelPayload : MapEventPayloadBase
    {
        /// <summary>
        /// Wheel event type.
        /// </summary>
        [JsonInclude] public string? Type { get; internal set; }
    }
}
