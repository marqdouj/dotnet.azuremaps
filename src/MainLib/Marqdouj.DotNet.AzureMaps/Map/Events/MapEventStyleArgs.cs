namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventStyleArgs : MapEventArgs<MapEventStylePayload> { }

	public class MapEventStylePayload : MapEventPayloadBase
    {
        public string? Style { get; set; }
    }
}
