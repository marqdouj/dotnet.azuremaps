namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventStyleArgs : MapEventArgs<MapEventStylePayload> { }

	public class MapEventStylePayload
    {
        public string? Style { get; set; }
    }
}
