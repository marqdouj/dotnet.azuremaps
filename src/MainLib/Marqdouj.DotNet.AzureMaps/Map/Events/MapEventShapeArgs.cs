using System.Text.Json;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventShapeArgs : MapEventArgs<MapEventShapePayload>
    {
        public override string ToString() => JsonSerializer.Serialize(this);
    }

    public class MapEventShapePayload : MapEventPayloadBase
    {
    }
}
