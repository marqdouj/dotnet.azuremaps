using System.Text.Json;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventStyleArgs : MapEventArgs<MapEventStylePayload>
    {
        public override string ToString() => JsonSerializer.Serialize(this);
    }

    public class MapEventStylePayload : MapEventPayloadBase
    {
        public string? Style { get; set; }
    }
}
