using System.Text.Json;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventGeneralArgs : MapEventArgs<MapEventGeneralPayload>
    {
        public override string ToString() => JsonSerializer.Serialize(this);
    }

    public class MapEventGeneralPayload : MapEventPayloadBase
    {

    }
}
