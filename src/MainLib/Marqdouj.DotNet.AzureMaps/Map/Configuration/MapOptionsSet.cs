namespace Marqdouj.DotNet.AzureMaps.Map.Configuration
{
    /// <summary>
    /// Used when setting the current options for the map.
    /// </summary>
    public class MapOptionsSet
    {
        public CameraOptionsSet? Camera { get; set; }
        public ServiceOptions? Service { get; set; }
        public StyleOptions? Style { get; set; }
        public UserInteractionOptions? UserInteraction { get; set; }

        internal MapOptions ToCreateMapOptions()
        {
            return new MapOptions
            {
                Camera = this.Camera?.Camera,
                CameraBounds = this.Camera?.CameraBounds,
                Service = this.Service,
                Style = this.Style,
                UserInteraction = this.UserInteraction
            };
        }
    }
}
