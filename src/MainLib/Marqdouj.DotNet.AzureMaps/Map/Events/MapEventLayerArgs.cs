using System.Text.Json;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventLayerArgs : MapEventArgs<MapEventLayerPayload>
    {
        public override string ToString() => JsonSerializer.Serialize(this);
    }

    public class MapEventLayerPayload : MapEventPayloadBase
    {
        public MapEventMousePayload? Mouse { get; set; }
        public MapEventTouchPayload? Touch { get; set; }
        public MapEventWheelPayload? Wheel { get; set; }
    }
}
