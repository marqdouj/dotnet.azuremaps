namespace Marqdouj.DotNet.AzureMaps.Map.Interop
{
    /// <summary>
    /// Manages Azure Map JS Interactions
    /// </summary>
    public interface IAzureMapContainer
    {
        string MapId { get; }
        IAzureMapsLayers Layers { get; }
        IAzureMapsConfiguration Configuration { get; }
        IAzureMapsMaps Maps { get; }
    }
}
