namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventShapeArgs : MapEventArgs<MapEventShapePayload> { }

    public class MapEventShapePayload
    {
        public string? Id { get; set; }
    }
}
