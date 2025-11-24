namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventSourceArgs : MapEventArgs<MapEventSourcePayload> { }

    public class MapEventSourcePayload
    {
        public string? Id { get; set; }
    }
}
