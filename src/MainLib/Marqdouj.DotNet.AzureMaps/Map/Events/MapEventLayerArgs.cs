namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventLayerArgs : MapEventArgs<MapEventLayerPayload> { }

    public class MapEventLayerPayload
    {
        public string? Id { get; set; }
        public MapEventMousePayload? Mouse { get; set; }
        public MapEventTouchPayload? Touch { get; set; }
        public MapEventWheelPayload? Wheel { get; set; }
    }
}
