using System.Text.Json;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventSourceArgs : MapEventArgs<MapEventSourcePayload>
    {
        public override string ToString() => JsonSerializer.Serialize(this);
    }

    public class MapEventSourcePayload : MapEventPayloadBase
    {
    }
}
